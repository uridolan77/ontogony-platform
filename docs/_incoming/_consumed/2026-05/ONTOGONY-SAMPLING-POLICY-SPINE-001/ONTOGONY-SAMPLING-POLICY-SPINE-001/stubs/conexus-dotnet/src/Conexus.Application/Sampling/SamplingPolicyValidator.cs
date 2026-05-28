using Conexus.Domain.Sampling;

namespace Conexus.Application.Sampling;

public sealed class SamplingPolicyValidator : ISamplingPolicyValidator
{
    private static readonly HashSet<string> HighRisk = new(StringComparer.OrdinalIgnoreCase)
    {
        "high",
        "critical"
    };

    private static readonly HashSet<string> DeterministicSafeProfiles = new(StringComparer.OrdinalIgnoreCase)
    {
        "DeterministicContract",
        "ExtractionStrict",
        "RepairRetry"
    };

    public Task<IReadOnlyList<SamplingPolicyNotice>> ValidateAsync(
        SamplingPolicyResolveRequest request,
        SamplingProfile effectiveProfile,
        CancellationToken cancellationToken = default)
    {
        var notices = new List<SamplingPolicyNotice>();

        if (request.DirectExecution && !effectiveProfile.DirectExecutionAllowed)
        {
            notices.Add(new SamplingPolicyNotice(
                "DiversityProbeCannotDirectlyExecute",
                "error",
                $"Profile '{effectiveProfile.Id}' cannot be used for direct execution.",
                "DeterministicContract"));
        }

        if (request.SideEffecting && !effectiveProfile.AllowsSideEffects)
        {
            notices.Add(new SamplingPolicyNotice(
                "SideEffectRequiresDirectExecutionSafeProfile",
                "error",
                $"Profile '{effectiveProfile.Id}' cannot be used for side-effecting operations.",
                "DeterministicContract"));
        }

        if (request.ToolUse && !effectiveProfile.AllowsTools)
        {
            notices.Add(new SamplingPolicyNotice(
                "ToolUseRequiresDirectExecutionSafeProfile",
                "error",
                $"Profile '{effectiveProfile.Id}' does not allow tool use.",
                "DeterministicContract"));
        }

        if (HighRisk.Contains(request.RiskTier) && !DeterministicSafeProfiles.Contains(effectiveProfile.Id))
        {
            notices.Add(new SamplingPolicyNotice(
                "HighRiskOperationRequiresDeterministicProfile",
                "error",
                $"Risk tier '{request.RiskTier}' requires a deterministic/strict profile, not '{effectiveProfile.Id}'.",
                "DeterministicContract"));
        }

        if (effectiveProfile.BlockedOutputContracts.Contains(request.OutputContract, StringComparer.OrdinalIgnoreCase))
        {
            notices.Add(new SamplingPolicyNotice(
                "CreativeProfileNotAllowedForContractBoundTask",
                "error",
                $"Profile '{effectiveProfile.Id}' is blocked for outputContract='{request.OutputContract}'.",
                "DeterministicContract"));
        }

        if (!effectiveProfile.AllowedTaskKinds.Contains(request.TaskKind, StringComparer.OrdinalIgnoreCase))
        {
            notices.Add(new SamplingPolicyNotice(
                "ProfileNotNormallyAllowedForTaskKind",
                "warning",
                $"Profile '{effectiveProfile.Id}' is not in the normal allowed set for taskKind='{request.TaskKind}'."));
        }

        if (effectiveProfile.DeterminismGuarantee == "provider-dependent")
        {
            notices.Add(new SamplingPolicyNotice(
                "ProviderDependentDeterminism",
                "info",
                "Determinism depends on provider seed/backend behavior and is not guaranteed by temperature alone."));
        }

        return Task.FromResult<IReadOnlyList<SamplingPolicyNotice>>(notices);
    }
}
