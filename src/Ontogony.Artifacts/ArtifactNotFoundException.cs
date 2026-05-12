namespace Ontogony.Artifacts;

/// <summary>
/// Thrown by <see cref="IArtifactStore.GetAsync"/> when the requested artifact id is unknown.
/// </summary>
/// <remarks>
/// The message embeds the missing id for diagnostics. Hosts that surface exceptions to external
/// callers should map this through their error middleware (for example <c>Ontogony.Errors</c>)
/// so that artifact ids are not leaked verbatim in public responses.
/// </remarks>
public sealed class ArtifactNotFoundException : Exception
{
    /// <summary>Creates an exception for the missing <paramref name="artifactId"/>.</summary>
    public ArtifactNotFoundException(string artifactId)
        : base($"Artifact '{artifactId}' was not found.")
    {
        ArtifactId = artifactId;
    }

    /// <summary>Artifact id that was not found.</summary>
    public string ArtifactId { get; }
}
