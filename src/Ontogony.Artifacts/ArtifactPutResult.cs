namespace Ontogony.Artifacts;

/// <summary>
/// Result of an artifact write. <see cref="Existed"/> is <c>true</c> when the store deduplicated
/// against an existing entry with matching content and scope.
/// </summary>
public sealed record ArtifactPutResult(ArtifactRef Reference, bool Existed);
