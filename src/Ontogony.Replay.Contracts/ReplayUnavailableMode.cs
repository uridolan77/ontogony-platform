namespace Ontogony.Replay.Contracts;

/// <summary>Replay mode that is unavailable for a target with a reason code.</summary>
public sealed record ReplayUnavailableMode(
    string Mode,
    string ReasonCode,
    string Message);
