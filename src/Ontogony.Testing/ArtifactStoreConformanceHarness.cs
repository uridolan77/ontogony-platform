using Ontogony.Artifacts;

namespace Ontogony.Testing;

/// <summary>
/// Conformance helpers for <see cref="IArtifactStore"/> hash/provenance semantics.
/// </summary>
public static class ArtifactStoreConformanceHarness
{
    /// <summary>
    /// Verifies put/get round-trip, content hash stability, and dedupe on identical scope+bytes.
    /// </summary>
    public static async Task AssertPutGetAndDedupeAsync(IArtifactStore store)
    {
        ArgumentNullException.ThrowIfNull(store);

        var payload = "conformance-artifact-payload"u8.ToArray();
        var request = new ArtifactPutRequest
        {
            MediaType = "application/octet-stream",
            Content = payload,
            TenantId = "tenant-conformance",
            ProjectId = "project-conformance",
            Classification = "test"
        };

        var first = await store.PutAsync(request);
        var second = await store.PutAsync(request);

        if (string.IsNullOrWhiteSpace(first.Reference.ArtifactId))
            throw new InvalidOperationException("First put did not return an artifact id.");

        if (!second.Existed)
            throw new InvalidOperationException("Expected duplicate put to report Existed=true.");

        if (!string.Equals(first.Reference.ArtifactId, second.Reference.ArtifactId, StringComparison.Ordinal))
            throw new InvalidOperationException("Dedupe identity did not return the same artifact id.");

        if (string.IsNullOrWhiteSpace(first.Reference.ContentHash))
            throw new InvalidOperationException("ContentHash must be populated on ArtifactRef.");

        var content = await store.GetAsync(first.Reference.ArtifactId);
        if (content.Bytes.Length != payload.Length)
            throw new InvalidOperationException("Stored bytes length mismatch.");

        var reference = await store.GetReferenceAsync(first.Reference.ArtifactId);
        if (reference is null || reference.ContentHash != first.Reference.ContentHash)
            throw new InvalidOperationException("GetReferenceAsync did not return matching hash metadata.");
    }

    /// <summary>Verifies <see cref="IArtifactStore.ExistsAsync"/> and <see cref="IArtifactStore.TryGetAsync"/>.</summary>
    public static async Task AssertExistsAndTryGetAsync(IArtifactStore store)
    {
        ArgumentNullException.ThrowIfNull(store);

        var missing = await store.TryGetAsync("artifact-does-not-exist-conformance");
        if (missing is not null)
            throw new InvalidOperationException("TryGetAsync should return null for unknown artifact id.");

        if (await store.ExistsAsync("artifact-does-not-exist-conformance"))
            throw new InvalidOperationException("ExistsAsync should be false for unknown artifact id.");
    }
}
