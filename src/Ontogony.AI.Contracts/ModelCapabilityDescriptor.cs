namespace Ontogony.AI.Contracts;

/// <summary>
/// Descriptive capability flags for a provider model (not a selector or ranking score).
/// </summary>
public sealed record ModelCapabilityDescriptor(
    string Provider,
    string Model,
    bool SupportsStreaming,
    bool SupportsTools,
    bool SupportsJsonMode,
    int? MaxContextTokens,
    string? DescriptorVersion);
