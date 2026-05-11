namespace Athanor.Infrastructure.Options;

public sealed class EmbeddingOptions
{
    public const string SectionName = "Embeddings";

    public string Provider { get; init; } = "deterministic";

    /// <summary>
    /// Deterministic embeddings are intended for development/test only.
    /// In non-development environments they are blocked unless explicitly allowed.
    /// </summary>
    public bool AllowDeterministicInNonDevelopment { get; init; } = false;
}

