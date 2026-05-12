namespace Ontogony.Replay.Contracts;

public sealed record ReplayDeterminismHints(
    string? Seed = null,
    string? Temperature = null,
    string? TopP = null,
    bool ExternalProviderMayBeNonDeterministic = true,
    IReadOnlyDictionary<string, string>? ProviderParameters = null);
