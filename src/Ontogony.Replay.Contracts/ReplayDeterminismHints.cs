namespace Ontogony.Replay.Contracts;

/// <summary>Opaque determinism hints attached to a replay manifest.</summary>
public sealed record ReplayDeterminismHints(
    string? Seed = null,
    string? Temperature = null,
    string? TopP = null,
    bool ExternalProviderMayBeNonDeterministic = true,
    IReadOnlyDictionary<string, string>? ProviderParameters = null);
