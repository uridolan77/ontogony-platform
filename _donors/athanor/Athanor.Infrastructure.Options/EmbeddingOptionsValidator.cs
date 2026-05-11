using Microsoft.Extensions.Options;

namespace Athanor.Infrastructure.Options;

public sealed class EmbeddingOptionsValidator : IValidateOptions<EmbeddingOptions>
{
    public ValidateOptionsResult Validate(string? name, EmbeddingOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Provider))
        {
            return ValidateOptionsResult.Fail("Embeddings:Provider is required.");
        }

        var provider = options.Provider.Trim();
        if (!string.Equals(provider, "deterministic", StringComparison.OrdinalIgnoreCase))
        {
            return ValidateOptionsResult.Fail(
                $"Embeddings:Provider '{options.Provider}' is not supported. Only 'deterministic' is supported in PR39.");
        }

        return ValidateOptionsResult.Success;
    }
}

