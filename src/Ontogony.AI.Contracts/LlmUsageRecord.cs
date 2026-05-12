namespace Ontogony.AI.Contracts;

/// <summary>
/// Token usage reported by a provider (opaque <see cref="Tokenizer"/> and <see cref="ProviderUsageHash"/>).
/// </summary>
public sealed record LlmUsageRecord(int? InputTokens, int? OutputTokens, int? TotalTokens, string? Tokenizer, string? ProviderUsageHash)
{
    /// <summary>
    /// Returns <see cref="TotalTokens"/> when set; otherwise sums <see cref="InputTokens"/> and <see cref="OutputTokens"/> when both are known.
    /// </summary>
    public int? ResolveTotalTokensOrSum() =>
        TotalTokens ?? (InputTokens is int i && OutputTokens is int o ? i + o : null);
}
