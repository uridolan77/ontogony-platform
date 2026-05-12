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
    public required string MediaType { get; init; }

    public required Stream ContentStream { get; init; }

    /// <summary>When set, the store must reject the write if the drained length differs.</summary>
    public long? ExpectedSizeBytes { get; init; }

    /// <summary>When set, the store must reject the write if the computed content hash differs.</summary>
    public string? ExpectedContentHash { get; init; }

    public string? ContentEncoding { get; init; }

    public string? StorageTier { get; init; }

    public string? Classification { get; init; }

    public string? Uri { get; init; }

    public string? TenantId { get; init; }

    public string? WorkspaceId { get; init; }

    public string? ProjectId { get; init; }

    public string? SuggestedArtifactId { get; init; }
}
