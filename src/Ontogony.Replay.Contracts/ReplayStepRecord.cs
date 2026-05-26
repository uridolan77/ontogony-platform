namespace Ontogony.Replay.Contracts;

/// <summary>Single replay step boundary record.</summary>
public sealed record ReplayStepRecord(
    string StepId,
    string StepKind,
    string? ExecutionStepId = null,
    string? RequestId = null,
    IReadOnlyList<ReplayInputRef>? Inputs = null,
    IReadOnlyList<ReplayOutputRef>? Outputs = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
