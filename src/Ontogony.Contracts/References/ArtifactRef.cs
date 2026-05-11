namespace Ontogony.Contracts.References;

/// <summary>
/// Reference to an artifact (file, model output, code, etc.) in the event system.
/// </summary>
public sealed record ArtifactRef(
    string ArtifactId,
    string ArtifactType,
    string? Label = null,
    string? ContentHash = null,
    string? MimeType = null);
