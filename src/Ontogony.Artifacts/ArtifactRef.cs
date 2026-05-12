namespace Ontogony.Artifacts;

/// <summary>
/// Mechanical reference to a stored artifact. Carries identity, content fingerprint, size,
/// and opaque transport/scope hints — but never the payload bytes themselves.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="MediaType"/>, <see cref="ContentEncoding"/>, <see cref="StorageTier"/>, and
/// <see cref="Classification"/> are <b>opaque strings</b>. Ontogony does not define provider tiers,
/// sensitivity registries, or retention policy; callers use whatever vocabulary their host or product
/// already uses (for example <c>application/json</c>, <c>gzip</c>, <c>local</c>, <c>sensitive</c>).
/// </para>
/// <para>
/// <b>Identity (dedupe) metadata</b>: <see cref="ContentHash"/>, <see cref="TenantId"/>,
/// <see cref="WorkspaceId"/>, <see cref="ProjectId"/>, <see cref="MediaType"/>,
/// <see cref="ContentEncoding"/>, and <see cref="Classification"/>. Reference implementations
/// must treat agreement on all of these as the same artifact.
/// </para>
/// <para>
/// <b>Hint metadata</b>: <see cref="StorageTier"/> and <see cref="Uri"/> describe locator details
/// and do <i>not</i> participate in dedupe identity.
/// </para>
/// <para>
/// <see cref="ContentHash"/> is computed over the <b>stored raw bytes</b> (i.e. the bytes the store
/// accepted, whatever encoding <see cref="ContentEncoding"/> labels them as).
/// </para>
/// </remarks>
public sealed record ArtifactRef
{
    /// <summary>Stable, opaque identifier for this artifact (caller- or store-assigned).</summary>
    public required string ArtifactId { get; init; }

    /// <summary>Lowercase hex content fingerprint (SHA-256 over the raw bytes).</summary>
    public required string ContentHash { get; init; }

    /// <summary>Opaque media type (no platform registry).</summary>
    public required string MediaType { get; init; }

    /// <summary>Length of the stored bytes; matches the bytes that produced <see cref="ContentHash"/>.</summary>
    public required long SizeBytes { get; init; }

    /// <summary>Opaque content encoding (for example <c>identity</c>, <c>gzip</c>, <c>br</c>).</summary>
    public string? ContentEncoding { get; init; }

    /// <summary>Opaque storage tier hint (for example <c>inline</c>, <c>local</c>, <c>remote</c>).</summary>
    public string? StorageTier { get; init; }

    /// <summary>Opaque sensitivity classification (no platform policy attached).</summary>
    public string? Classification { get; init; }

    /// <summary>Optional opaque locator URI; the scheme is caller-defined.</summary>
    public string? Uri { get; init; }

    public string? TenantId { get; init; }
    public string? WorkspaceId { get; init; }
    public string? ProjectId { get; init; }

    public DateTimeOffset CreatedAt { get; init; }
}
