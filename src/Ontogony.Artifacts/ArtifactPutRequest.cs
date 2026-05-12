namespace Ontogony.Artifacts;

/// <summary>
/// Input to <see cref="IArtifactStore.PutAsync"/>. Carries the raw bytes plus opaque transport
/// and scope hints used by the store to derive an <see cref="ArtifactRef"/>.
/// </summary>
/// <remarks>
/// Not a wire DTO — equality semantics for the <see cref="Content"/> buffer are intentionally
/// reference-based. Wrap the resulting <see cref="ArtifactRef"/> (not this request) in
/// <c>OntogonyEnvelope&lt;ArtifactRef&gt;</c> when emitting events.
/// </remarks>
public sealed class ArtifactPutRequest
{
    public required string MediaType { get; init; }

    public required ReadOnlyMemory<byte> Content { get; init; }

    public string? ContentEncoding { get; init; }

    public string? StorageTier { get; init; }

    public string? Classification { get; init; }

    public string? Uri { get; init; }

    public string? TenantId { get; init; }

    public string? WorkspaceId { get; init; }

    public string? ProjectId { get; init; }

    /// <summary>Optional caller-supplied identifier. When present, the store must honor or reject it deterministically.</summary>
    public string? SuggestedArtifactId { get; init; }
}
