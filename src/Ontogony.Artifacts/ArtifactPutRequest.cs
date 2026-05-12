namespace Ontogony.Artifacts;

/// <summary>
/// Input to <see cref="IArtifactStore.PutAsync(ArtifactPutRequest, CancellationToken)"/>. Carries
/// the raw bytes plus opaque transport and scope hints used by the store to derive an
/// <see cref="ArtifactRef"/>. For payloads that should not be buffered in full, use
/// <see cref="ArtifactStreamPutRequest"/> instead.
/// </summary>
/// <remarks>
/// Not a wire DTO — equality semantics for the <see cref="Content"/> buffer are intentionally
/// reference-based. Wrap the resulting <see cref="ArtifactRef"/> (not this request) in
/// <c>OntogonyEnvelope&lt;ArtifactRef&gt;</c> when emitting events.
/// </remarks>
public sealed class ArtifactPutRequest
{
    /// <summary>Opaque media type of <see cref="Content"/>.</summary>
    public required string MediaType { get; init; }

    /// <summary>Raw artifact bytes.</summary>
    public required ReadOnlyMemory<byte> Content { get; init; }

    /// <summary>Opaque content encoding label.</summary>
    public string? ContentEncoding { get; init; }

    /// <summary>Opaque storage tier hint.</summary>
    public string? StorageTier { get; init; }

    /// <summary>Opaque sensitivity classification.</summary>
    public string? Classification { get; init; }

    /// <summary>Optional opaque locator URI.</summary>
    public string? Uri { get; init; }

    /// <summary>Optional tenant scope for dedupe identity.</summary>
    public string? TenantId { get; init; }

    /// <summary>Optional workspace scope for dedupe identity.</summary>
    public string? WorkspaceId { get; init; }

    /// <summary>Optional project scope for dedupe identity.</summary>
    public string? ProjectId { get; init; }

    /// <summary>Optional caller-supplied identifier. When present, the store must honor or reject it deterministically.</summary>
    public string? SuggestedArtifactId { get; init; }
}
