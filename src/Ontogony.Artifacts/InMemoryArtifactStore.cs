using System.Collections.Concurrent;
using Ontogony.Hashing;
using Ontogony.Primitives;

namespace Ontogony.Artifacts;

/// <summary>
/// Thread-safe in-memory <see cref="IArtifactStore"/> reference implementation. Intended for
/// tests, examples, and single-process hosts. Not a durable or multi-node store.
/// </summary>
/// <remarks>
/// Deduplication key: <c>ContentHash + TenantId + WorkspaceId + ProjectId + MediaType + Classification</c>.
/// When a <see cref="ArtifactPutRequest.SuggestedArtifactId"/> is supplied and already exists, the
/// store returns the existing entry only when the content hash and scope match; otherwise it throws
/// <see cref="InvalidOperationException"/> to prevent silent overwrite.
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

        var bytes = request.Content;
        var contentHash = _hasher.ComputeBytesSha256(bytes.Span);
        var scopeKey = BuildScopeKey(
            contentHash,
            request.TenantId,
            request.WorkspaceId,
            request.ProjectId,
            request.MediaType,
            request.Classification);

        lock (_sync)
        {
            if (request.SuggestedArtifactId is { Length: > 0 } suggested
                && _byId.TryGetValue(suggested, out var existingById))
            {
                if (string.Equals(existingById.Reference.ContentHash, contentHash, StringComparison.Ordinal)
                    && ScopeMatches(existingById.Reference, request))
                {
                    return Task.FromResult(new ArtifactPutResult(existingById.Reference, Existed: true));
                }

                throw new InvalidOperationException(
                    $"An artifact with id '{suggested}' already exists with different content or scope.");
            }

            if (_scopeToId.TryGetValue(scopeKey, out var existingId)
                && _byId.TryGetValue(existingId, out var existingScope))
            {
                return Task.FromResult(new ArtifactPutResult(existingScope.Reference, Existed: true));
            }

            var id = !string.IsNullOrWhiteSpace(request.SuggestedArtifactId)
                ? request.SuggestedArtifactId!
                : _ids.NewId("art");

            var stored = bytes.ToArray();
            var reference = new ArtifactRef
            {
                ArtifactId = id,
                ContentHash = contentHash,
                MediaType = request.MediaType,
                SizeBytes = stored.LongLength,
                ContentEncoding = request.ContentEncoding,
                StorageTier = request.StorageTier,
                Classification = request.Classification,
                Uri = request.Uri,
                TenantId = request.TenantId,
                WorkspaceId = request.WorkspaceId,
                ProjectId = request.ProjectId,
                CreatedAt = _clock.UtcNow
            };

            _byId[id] = new StoredArtifact(reference, stored);
            _scopeToId[scopeKey] = id;

            return Task.FromResult(new ArtifactPutResult(reference, Existed: false));
        }
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

        return Task.FromResult(new ArtifactContent(stored.Reference, stored.Bytes));
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
            ? new ArtifactContent(stored.Reference, stored.Bytes)
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

    private static bool ScopeMatches(ArtifactRef reference, ArtifactPutRequest request) =>
        string.Equals(reference.TenantId, request.TenantId, StringComparison.Ordinal)
        && string.Equals(reference.WorkspaceId, request.WorkspaceId, StringComparison.Ordinal)
        && string.Equals(reference.ProjectId, request.ProjectId, StringComparison.Ordinal)
        && string.Equals(reference.MediaType, request.MediaType, StringComparison.Ordinal)
        && string.Equals(reference.Classification, request.Classification, StringComparison.Ordinal);

    private static string BuildScopeKey(
        string contentHash,
        string? tenantId,
        string? workspaceId,
        string? projectId,
        string mediaType,
        string? classification) =>
        string.Join('|',
            contentHash,
            tenantId ?? string.Empty,
            workspaceId ?? string.Empty,
            projectId ?? string.Empty,
            mediaType,
            classification ?? string.Empty);

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
