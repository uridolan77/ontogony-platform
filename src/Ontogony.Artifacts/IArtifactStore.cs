namespace Ontogony.Artifacts;

/// <summary>
/// Mechanical port for content-addressed artifact storage. Implementations are responsible
/// for durability, deduplication, and concurrency; this package only ships an in-memory
/// reference implementation suitable for tests and single-process hosts.
/// </summary>
public interface IArtifactStore
{
    /// <summary>
    /// Stores the supplied bytes and returns an <see cref="ArtifactPutResult"/> referencing the
    /// (possibly already existing) artifact.
    /// </summary>
    Task<ArtifactPutResult> PutAsync(ArtifactPutRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the artifact bytes for <paramref name="artifactId"/> or throws
    /// <see cref="ArtifactNotFoundException"/> when not present.
    /// </summary>
    Task<ArtifactContent> GetAsync(string artifactId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the artifact bytes for <paramref name="artifactId"/> or <c>null</c> when not present.
    /// </summary>
    Task<ArtifactContent?> TryGetAsync(string artifactId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the metadata for <paramref name="artifactId"/> without loading the bytes,
    /// or <c>null</c> when not present.
    /// </summary>
    Task<ArtifactRef?> GetReferenceAsync(string artifactId, CancellationToken cancellationToken = default);

    /// <summary>Returns <c>true</c> when an artifact with <paramref name="artifactId"/> is known.</summary>
    Task<bool> ExistsAsync(string artifactId, CancellationToken cancellationToken = default);
}
