using Conexus.Domain.Sampling;

namespace Conexus.Application.Sampling;

public interface ISamplingProfileRegistry
{
    IReadOnlyCollection<SamplingProfile> List();
    SamplingProfile? TryGet(string profileId);
}

public interface ISamplingPolicyResolver
{
    Task<SamplingPolicyResolution> ResolveAsync(
        SamplingPolicyResolveRequest request,
        CancellationToken cancellationToken = default);
}

public interface ISamplingPolicyValidator
{
    Task<IReadOnlyList<SamplingPolicyNotice>> ValidateAsync(
        SamplingPolicyResolveRequest request,
        SamplingProfile effectiveProfile,
        CancellationToken cancellationToken = default);
}

public interface IProviderSamplingParameterTranslator
{
    IReadOnlyDictionary<string, object?> Translate(
        SamplingProfile profile,
        string? provider,
        string? model,
        out IReadOnlyList<SamplingPolicyNotice> warnings);
}
