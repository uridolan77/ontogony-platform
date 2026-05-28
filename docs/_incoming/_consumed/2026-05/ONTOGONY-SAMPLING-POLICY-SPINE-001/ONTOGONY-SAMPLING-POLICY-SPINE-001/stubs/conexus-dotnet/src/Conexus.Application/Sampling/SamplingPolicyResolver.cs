using Conexus.Domain.Sampling;

namespace Conexus.Application.Sampling;

public sealed class SamplingPolicyResolver : ISamplingPolicyResolver
{
    private readonly ISamplingProfileRegistry _registry;
    private readonly ISamplingPolicyValidator _validator;
    private readonly IProviderSamplingParameterTranslator _translator;

    public SamplingPolicyResolver(
        ISamplingProfileRegistry registry,
        ISamplingPolicyValidator validator,
        IProviderSamplingParameterTranslator translator)
    {
        _registry = registry;
        _validator = validator;
        _translator = translator;
    }

    public async Task<SamplingPolicyResolution> ResolveAsync(
        SamplingPolicyResolveRequest request,
        CancellationToken cancellationToken = default)
    {
        var warnings = new List<SamplingPolicyNotice>();
        var violations = new List<SamplingPolicyNotice>();

        var effectiveProfileId = request.RequestedProfileId ?? DefaultProfileFor(request);
        var profile = _registry.TryGet(effectiveProfileId);

        if (profile is null)
        {
            violations.Add(new SamplingPolicyNotice(
                "UnknownSamplingProfile",
                "error",
                $"Sampling profile '{effectiveProfileId}' is not registered.",
                "DeterministicContract"));

            profile = _registry.TryGet("DeterministicContract")
                ?? throw new InvalidOperationException("Missing required DeterministicContract sampling profile.");
        }

        violations.AddRange(await _validator.ValidateAsync(request, profile, cancellationToken));

        if (request.RequestedParameters is not null && request.RequestedProfileId is null)
        {
            warnings.Add(new SamplingPolicyNotice(
                "LegacyRawSamplingParameters",
                "warning",
                "Raw sampling parameters were supplied without samplingProfileId; migrate this call site to a named sampling profile."));
        }

        var providerParameters = _translator.Translate(profile, request.Provider, request.Model, out var translationWarnings);
        warnings.AddRange(translationWarnings);

        var decision = violations.Any(v => v.Severity == "error")
            ? SamplingPolicyDecision.Denied
            : warnings.Count > 0
                ? SamplingPolicyDecision.AllowedWithWarnings
                : SamplingPolicyDecision.Allowed;

        return new SamplingPolicyResolution(
            ResolutionId: $"spres_{Guid.NewGuid():N}",
            RequestedProfileId: request.RequestedProfileId,
            EffectiveProfileId: profile.Id,
            Decision: decision,
            PolicyBasis: $"operationKind={request.OperationKind};taskKind={request.TaskKind};riskTier={request.RiskTier};outputContract={request.OutputContract}",
            EffectiveParameters: new SamplingParameters(
                profile.Temperature,
                profile.TopP,
                profile.CandidateCount,
                profile.Seed,
                profile.PresencePenalty,
                profile.FrequencyPenalty),
            ProviderParameters: providerParameters,
            Warnings: warnings,
            Violations: violations,
            ResolvedAtUtc: DateTimeOffset.UtcNow);
    }

    private static string DefaultProfileFor(SamplingPolicyResolveRequest request)
    {
        var key = $"{request.OperationKind}:{request.TaskKind}:{request.OutputContract}".ToLowerInvariant();

        if (key.Contains("schema_mapping") || key.Contains("routing") || key.Contains("policy_decision"))
            return "DeterministicContract";

        if (key.Contains("json") || key.Contains("extraction"))
            return "ExtractionStrict";

        if (key.Contains("repair"))
            return "RepairRetry";

        if (key.Contains("creative") || key.Contains("ideation") || key.Contains("naming"))
            return "CreativeIdeation";

        if (key.Contains("alternative") || key.Contains("diversity"))
            return "DiversityProbe";

        return "AnalyticalReasoning";
    }
}
