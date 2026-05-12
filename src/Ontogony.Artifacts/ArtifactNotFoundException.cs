namespace Ontogony.Artifacts;

/// <summary>
/// Thrown by <see cref="IArtifactStore.GetAsync"/> when the requested artifact id is unknown.
/// </summary>
public sealed class ArtifactNotFoundException : Exception
{
    public ArtifactNotFoundException(string artifactId)
        : base($"Artifact '{artifactId}' was not found.")
    {
        ArtifactId = artifactId;
    }

    public string ArtifactId { get; }
}
