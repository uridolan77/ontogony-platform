namespace Ontogony.Replay.Contracts;

public sealed record ReplayEligibility(
    ReplayTarget Target,
    IReadOnlyList<string> EligibleModes,
    string RecommendedMode,
    IReadOnlyList<ReplayUnavailableMode> UnavailableModes,
    ReplaySafetyPosture SafetyPosture,
    string Confidence = "medium",
    IReadOnlyList<string>? RequiredSourceData = null,
    IReadOnlyList<string>? AvailableSourceData = null,
    IReadOnlyList<string>? MissingSourceData = null);
