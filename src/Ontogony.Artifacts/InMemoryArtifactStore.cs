using System.Collections.Concurrent;
using Ontogony.Hashing;
using Ontogony.Primitives;

namespace Ontogony.Artifacts;

/// <summary>
/// Thread-safe in-memory <see cref="IArtifactStore"/> reference implementation. Intended for
/// tests, examples, and single-process hosts. Not a durable or multi-node store.
/// </summary>
/// <remarks>
/// <para>
/// <b>Identity (dedupe) metadata:</b> <c>ContentHash</c>, <c>TenantId</c>, <c>WorkspaceId</c>,
/// <c>ProjectId</c>, <c>MediaType</c>, <c>ContentEncoding</c>, <c>Classification</c>. Two writes
/// agreeing on every identity field return the same <see cref="ArtifactRef"/> with
/// <see cref="ArtifactPutResult.Existed"/> set to <c>true</c>.
/// </para>
/// <para>
/// <b>Hint metadata:</b> <c>StorageTier</c> and <c>Uri</c>. These describe the locator but do not
/// affect dedupe identity.
/// </para>
/// <para>
/// When a <see cref="ArtifactPutRequest.SuggestedArtifactId"/> is supplied and already exists, the
/// store returns the existing entry only when the content hash and identity scope match; otherwise
/// it throws <see cref="InvalidOperationException"/> to prevent silent overwrite.
/// </para>
/// <para>
/// Reads (<see cref="GetAsync"/> / <see cref="TryGetAsync"/>) return a defensive copy of the stored
/// bytes so consumers cannot mutate the in-process backing array.
/// </para>
/// </remarks>
public sealed class InMemoryArtifactStore : IArtifactStore
{
    private readonly ConcurrentDictionary<string, StoredArtifact> _byId = new(StringComparer.Ordinal);
    private readonly ConcurrentDictionary<string, string> _scopeToId = new(StringComparer.Ordinal);
    private readonly IContentHashService _hasher;
    private readonly IClock _clock;
    private readonly IIdGenerator _ids;
    private readonly object _sync = new();

    public InMemoryArtifactStore(
        IContentHashService? hasher = null,
        IClock? clock = null,
        IIdGenerator? ids = null)
    {
        _hasher = hasher ?? new Sha256ContentHashService();
        _clock = clock ?? new SystemClock();
        _ids = ids ?? new GuidIdGenerator();
    }

    /// <inheritdoc />
    public Task<ArtifactPutResult> PutAsync(ArtifactPutRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (string.IsNullOrWhiteSpace(request.MediaType))
        {
            throw new ArgumentException("MediaType is required.", nameof(request));
        }

        cancellationToken.ThrowIfCancellationRequested();

        var bytes = request.Content.ToArray();
        var contentHash = _hasher.ComputeBytesSha256(bytes);
        var scope = new ArtifactScope(
            request.TenantId,
            request.WorkspaceId,
            request.ProjectId,
            request.MediaType,
            request.ContentEncoding,
            request.Classification);

        return Task.FromResult(Store(
            contentHash,
            bytes,
            scope,
            request.StorageTier,
            request.Uri,
            request.SuggestedArtifactId));
    }

    /// <inheritdoc />
    public async Task<ArtifactPutResult> PutAsync(ArtifactStreamPutRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (string.IsNullOrWhiteSpace(request.MediaType))
        {
            throw new ArgumentException("MediaType is required.", nameof(request));
        }

        if (request.ContentStream is null)
        {
            throw new ArgumentException("ContentStream is required.", nameof(request));
        }

        cancellationToken.ThrowIfCancellationRequested();

        byte[] bytes;
        using (var buffer = new MemoryStream())
        {
            await request.ContentStream.CopyToAsync(buffer, cancellationToken).ConfigureAwait(false);
            bytes = buffer.ToArray();
        }

        if (request.ExpectedSizeBytes is { } expectedSize && bytes.LongLength != expectedSize)
        {
            throw new InvalidOperationException(
                $"Artifact size mismatch: expected {expectedSize}, drained {bytes.LongLength}.");
        }

        var contentHash = _hasher.ComputeBytesSha256(bytes);
        if (request.ExpectedContentHash is { Length: > 0 } expectedHash
            && !string.Equals(expectedHash, contentHash, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Artifact content hash mismatch: expected '{expectedHash}', drained '{contentHash}'.");
        }

        var scope = new ArtifactScope(
            request.TenantId,
            request.WorkspaceId,
            request.ProjectId,
            request.MediaType,
            request.ContentEncoding,
            request.Classification);

        return Store(
            contentHash,
            bytes,
            scope,
            request.StorageTier,
            request.Uri,
            request.SuggestedArtifactId);
    }

    /// <inheritdoc />
    public Task<ArtifactContent> GetAsync(string artifactId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(artifactId))
        {
            throw new ArgumentException("ArtifactId is required.", nameof(artifactId));
        }

        cancellationToken.ThrowIfCancellationRequested();

        if (!_byId.TryGetValue(artifactId, out var stored))
        {
            throw new ArtifactNotFoundException(artifactId);
        }

        return Task.FromResult(new ArtifactContent(stored.Reference, CopyBytes(stored.Bytes)));
    }

    /// <inheritdoc />
    public Task<ArtifactContent?> TryGetAsync(string artifactId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(artifactId))
        {
            throw new ArgumentException("ArtifactId is required.", nameof(artifactId));
        }

        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(_byId.TryGetValue(artifactId, out var stored)
            ? new ArtifactContent(stored.Reference, CopyBytes(stored.Bytes))
            : null);
    }

    /// <inheritdoc />
    public Task<ArtifactRef?> GetReferenceAsync(string artifactId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(artifactId))
        {
            throw new ArgumentException("ArtifactId is required.", nameof(artifactId));
        }

        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(_byId.TryGetValue(artifactId, out var stored) ? stored.Reference : null);
    }

    /// <inheritdoc />
    public Task<bool> ExistsAsync(string artifactId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(artifactId))
        {
            throw new ArgumentException("ArtifactId is required.", nameof(artifactId));
        }

        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(_byId.ContainsKey(artifactId));
    }

    /// <summary>Number of artifacts currently held by this store.</summary>
    public int Count => _byId.Count;

    private ArtifactPutResult Store(
        string contentHash,
        byte[] bytes,
        ArtifactScope scope,
        string? storageTier,
        string? uri,
        string? suggestedArtifactId)
    {
        var scopeKey = scope.BuildKey(contentHash);

        lock (_sync)
        {
            if (suggestedArtifactId is { Length: > 0 } suggested
                && _byId.TryGetValue(suggested, out var existingById))
            {
                if (string.Equals(existingById.Reference.ContentHash, contentHash, StringComparison.Ordinal)
                    && scope.Matches(existingById.Reference))
                {
                    return new ArtifactPutResult(existingById.Reference, Existed: true);
                }

                throw new InvalidOperationException(
                    $"An artifact with id '{suggested}' already exists with different content or scope.");
            }

            if (_scopeToId.TryGetValue(scopeKey, out var existingId)
                && _byId.TryGetValue(existingId, out var existingScope))
            {
                return new ArtifactPutResult(existingScope.Reference, Existed: true);
            }

            var id = !string.IsNullOrWhiteSpace(suggestedArtifactId)
                ? suggestedArtifactId!
                : _ids.NewId("art");

            var reference = new ArtifactRef
            {
                ArtifactId = id,
                ContentHash = contentHash,
                MediaType = scope.MediaType,
                SizeBytes = bytes.LongLength,
                ContentEncoding = scope.ContentEncoding,
                StorageTier = storageTier,
                Classification = scope.Classification,
                Uri = uri,
                TenantId = scope.TenantId,
                WorkspaceId = scope.WorkspaceId,
                ProjectId = scope.ProjectId,
                CreatedAt = _clock.UtcNow
            };

            _byId[id] = new StoredArtifact(reference, bytes);
            _scopeToId[scopeKey] = id;

            return new ArtifactPutResult(reference, Existed: false);
        }
    }

    private static ReadOnlyMemory<byte> CopyBytes(byte[] source)
    {
        var copy = new byte[source.LongLength];
        Buffer.BlockCopy(source, 0, copy, 0, source.Length);
        return copy;
    }

    private readonly record struct ArtifactScope(
        string? TenantId,
        string? WorkspaceId,
        string? ProjectId,
        string MediaType,
        string? ContentEncoding,
        string? Classification)
    {
        public string BuildKey(string contentHash) =>
            string.Join('|',
                contentHash,
                TenantId ?? string.Empty,
                WorkspaceId ?? string.Empty,
                ProjectId ?? string.Empty,
                MediaType,
                ContentEncoding ?? string.Empty,
                Classification ?? string.Empty);

        public bool Matches(ArtifactRef reference) =>
            string.Equals(reference.TenantId, TenantId, StringComparison.Ordinal)
            && string.Equals(reference.WorkspaceId, WorkspaceId, StringComparison.Ordinal)
            && string.Equals(reference.ProjectId, ProjectId, StringComparison.Ordinal)
            && string.Equals(reference.MediaType, MediaType, StringComparison.Ordinal)
            && string.Equals(reference.ContentEncoding, ContentEncoding, StringComparison.Ordinal)
            && string.Equals(reference.Classification, Classification, StringComparison.Ordinal);
    }

    private sealed class StoredArtifact
    {
        public StoredArtifact(ArtifactRef reference, byte[] bytes)
        {
            Reference = reference;
            Bytes = bytes;
        }

        public ArtifactRef Reference { get; }

        public byte[] Bytes { get; }
    }
}
