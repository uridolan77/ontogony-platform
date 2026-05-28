namespace Conexus.Domain.Sampling;

public enum SamplingPolicyDecision
{
    Allowed,
    AllowedWithWarnings,
    Denied,
    RequiresApproval
}

public sealed record SamplingParameters(
    decimal? Temperature,
    decimal? TopP,
    int? CandidateCount,
    int? Seed,
    decimal? PresencePenalty,
    decimal? FrequencyPenalty);

public sealed record SamplingPolicyNotice(
    string Code,
    string Severity,
    string Message,
    string? RecommendedProfileId = null);

public sealed record SamplingProfile(
    string Id,
    string DisplayName,
    string Intent,
    decimal Temperature,
    decimal TopP,
    int CandidateCount,
    decimal? PresencePenalty,
    decimal? FrequencyPenalty,
    int? Seed,
    bool JsonSchemaStrictPreferred,
    bool DirectExecutionAllowed,
    bool AllowsTools,
    bool AllowsSideEffects,
    string DeterminismGuarantee,
    IReadOnlyList<string> AllowedTaskKinds,
    IReadOnlyList<string> BlockedOutputContracts,
    string MaxRiskTier,
    IReadOnlyList<string> Notes);

public sealed record SamplingPolicyResolveRequest(
    string Caller,
    string OperationKind,
    string TaskKind,
    string RiskTier,
    string OutputContract,
    string? RequestedProfileId = null,
    SamplingParameters? RequestedParameters = null,
    bool DirectExecution = false,
    bool ToolUse = false,
    bool SideEffecting = false,
    string? Provider = null,
    string? Model = null,
    IReadOnlyDictionary<string, string>? Metadata = null);

public sealed record SamplingPolicyResolution(
    string ResolutionId,
    string? RequestedProfileId,
    string EffectiveProfileId,
    SamplingPolicyDecision Decision,
    string PolicyBasis,
    SamplingParameters EffectiveParameters,
    IReadOnlyDictionary<string, object?> ProviderParameters,
    IReadOnlyList<SamplingPolicyNotice> Warnings,
    IReadOnlyList<SamplingPolicyNotice> Violations,
    DateTimeOffset ResolvedAtUtc);
