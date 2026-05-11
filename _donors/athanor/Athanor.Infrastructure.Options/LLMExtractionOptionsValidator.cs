using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Athanor.Infrastructure.Options;

public sealed class LLMExtractionOptionsValidator : IValidateOptions<LLMExtractionOptions>
{
    private readonly IHostEnvironment _hostEnvironment;

    public LLMExtractionOptionsValidator(IHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

    public ValidateOptionsResult Validate(string? name, LLMExtractionOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ProviderKind))
        {
            return ValidateOptionsResult.Fail("LLMExtraction:ProviderKind is required.");
        }

        var kind = options.ProviderKind.Trim();
        var isFake = string.Equals(kind, "Fake", StringComparison.OrdinalIgnoreCase);
        var isOpenAi = string.Equals(kind, "OpenAICompatible", StringComparison.OrdinalIgnoreCase);
        if (!isFake && !isOpenAi)
        {
            return ValidateOptionsResult.Fail(
                $"LLMExtraction:ProviderKind '{kind}' is not supported. Use 'Fake' or 'OpenAICompatible'.");
        }

        if (isOpenAi)
        {
            if (!options.EnableLiveProvider)
            {
                return ValidateOptionsResult.Fail(
                    "LLMExtraction:ProviderKind OpenAICompatible requires LLMExtraction:EnableLiveProvider=true.");
            }

            if (!_hostEnvironment.IsDevelopment() && !options.AllowLiveProviderInNonDevelopment)
            {
                return ValidateOptionsResult.Fail(
                    "LLMExtraction: live OpenAI-compatible provider is not allowed outside Development unless LLMExtraction:AllowLiveProviderInNonDevelopment=true.");
            }

            if (string.IsNullOrWhiteSpace(options.EndpointBaseUrl))
            {
                return ValidateOptionsResult.Fail(
                    "LLMExtraction:EndpointBaseUrl is required when ProviderKind is OpenAICompatible.");
            }

            if (!Uri.TryCreate(options.EndpointBaseUrl.Trim(), UriKind.Absolute, out var endpointUri))
            {
                return ValidateOptionsResult.Fail(
                    "LLMExtraction:EndpointBaseUrl must be an absolute URI.");
            }

            if (!string.Equals(endpointUri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
            {
                var httpAllowed =
                    _hostEnvironment.IsDevelopment() && options.AllowHttpEndpointInDevelopment;
                if (!httpAllowed || !string.Equals(endpointUri.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase))
                {
                    return ValidateOptionsResult.Fail(
                        "LLMExtraction:EndpointBaseUrl must use HTTPS unless running in Development with LLMExtraction:AllowHttpEndpointInDevelopment=true for HTTP endpoints.");
                }
            }

            if (string.IsNullOrWhiteSpace(options.ModelName))
            {
                return ValidateOptionsResult.Fail(
                    "LLMExtraction:ModelName is required when ProviderKind is OpenAICompatible.");
            }

            if (string.IsNullOrWhiteSpace(options.SecretEnvVarName))
            {
                return ValidateOptionsResult.Fail(
                    "LLMExtraction:SecretEnvVarName is required when ProviderKind is OpenAICompatible.");
            }

            var secretName = options.SecretEnvVarName.Trim();
            var secretValue = Environment.GetEnvironmentVariable(secretName);
            if (string.IsNullOrEmpty(secretValue))
            {
                return ValidateOptionsResult.Fail(
                    $"LLMExtraction: environment variable '{secretName}' (SecretEnvVarName) is missing or empty.");
            }

            var authStyle = string.IsNullOrWhiteSpace(options.AuthHeaderStyle)
                ? "bearer"
                : options.AuthHeaderStyle.Trim();
            if (!string.Equals(authStyle, "bearer", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(authStyle, "api-key-header", StringComparison.OrdinalIgnoreCase))
            {
                return ValidateOptionsResult.Fail(
                    "LLMExtraction:AuthHeaderStyle must be 'bearer' or 'api-key-header'.");
            }
        }

        if (options.RequestTimeout <= TimeSpan.Zero || options.RequestTimeout > TimeSpan.FromHours(1))
        {
            return ValidateOptionsResult.Fail(
                "LLMExtraction:RequestTimeout must be greater than zero and at most one hour.");
        }

        if (options.MaxRetryAttempts < 0 || options.MaxRetryAttempts > 10)
        {
            return ValidateOptionsResult.Fail(
                "LLMExtraction:MaxRetryAttempts must be between 0 and 10.");
        }

        if (options.MaxOutputTokensBudget is int b && b <= 0)
        {
            return ValidateOptionsResult.Fail(
                "LLMExtraction:MaxOutputTokensBudget must be positive when set.");
        }

        if (options.MaxSourceTextChars < 1 || options.MaxSourceTextChars > 10_000_000)
        {
            return ValidateOptionsResult.Fail(
                "LLMExtraction:MaxSourceTextChars must be between 1 and 10000000.");
        }

        if (options.MaxChunksPerGeneratedExtraction < 1 || options.MaxChunksPerGeneratedExtraction > 10_000)
        {
            return ValidateOptionsResult.Fail(
                "LLMExtraction:MaxChunksPerGeneratedExtraction must be between 1 and 10000.");
        }

        if (options.MaxChunkTextChars < 1 || options.MaxChunkTextChars > 10_000_000)
        {
            return ValidateOptionsResult.Fail(
                "LLMExtraction:MaxChunkTextChars must be between 1 and 10000000.");
        }

        if (options.MaxGeneratedExtractionProposals < 1 || options.MaxGeneratedExtractionProposals > 100)
        {
            return ValidateOptionsResult.Fail(
                "LLMExtraction:MaxGeneratedExtractionProposals must be between 1 and 100.");
        }

        return ValidateOptionsResult.Success;
    }
}
