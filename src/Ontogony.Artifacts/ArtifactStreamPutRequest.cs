namespace Ontogony.Artifacts;

/// <summary>
/// Stream-based companion to <see cref="ArtifactPutRequest"/>. Lets implementations write large
/// artifacts (prompts, responses, document snapshots, embedding batches, tool outputs, replay
/// bundles) without buffering the entire payload in memory.
/// </summary>
/// <remarks>
/// <see cref="ExpectedContentHash"/>, when supplied, must equal the lowercase hex SHA-256 of the
/// bytes drained from <see cref="ContentStream"/>; mismatch is a hard error. <see cref="ExpectedSizeBytes"/>
/// is checked the same way when supplied. The store does <b>not</b> dispose <see cref="ContentStream"/>;
/// the caller retains ownership.
/// </remarks>
public sealed class ArtifactStreamPutRequest
{
    /// <summary>Opaque media type of the stream payload.</summary>
    public required string MediaType { get; init; }

    /// <summary>Readable stream of artifact bytes.</summary>
    public required Stream ContentStream { get; init; }

    /// <summary>When set, the store must reject the write if the drained length differs.</summary>
    public long? ExpectedSizeBytes { get; init; }

    /// <summary>When set, the store must reject the write if the computed content hash differs.</summary>
    public string? ExpectedContentHash { get; init; }

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

    /// <summary>Optional caller-supplied artifact id.</summary>
    public string? SuggestedArtifactId { get; init; }
}
