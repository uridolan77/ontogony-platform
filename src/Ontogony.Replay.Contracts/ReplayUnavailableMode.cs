namespace Ontogony.Replay.Contracts;

public sealed record ReplayUnavailableMode(
    string Mode,
    string ReasonCode,
    string Message);
