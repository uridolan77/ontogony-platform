namespace Ontogony.AI.Contracts;

/// <summary>
/// Descriptive capability flags for a provider model (not a selector or ranking score).
/// </summary>
/// <remarks>
/// <paramref name="Provider"/> and <paramref name="Model"/> are opaque strings; same intent as <see cref="LlmRequestEnvelope.Provider"/> / <see cref="LlmRequestEnvelope.Model"/>.
/// </remarks>
public sealed record ModelCapabilityDescriptor(
    string Provider,
    string Model,
    bool SupportsStreaming,
    bool SupportsTools,
    bool SupportsJsonMode,
    int? MaxContextTokens,
    string? DescriptorVersion);
