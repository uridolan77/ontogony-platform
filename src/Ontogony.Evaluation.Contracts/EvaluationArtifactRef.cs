namespace Ontogony.Evaluation.Contracts;

/// <summary>Reference to an evaluation evidence artifact (opaque <see cref="Role"/> and locator).</summary>
public sealed record EvaluationArtifactRef(
    string ArtifactId,
    string? ContentHash = null,
    string? MediaType = null,
    string? Role = null,
    string? LocatorUri = null,
    long? SizeBytes = null);
