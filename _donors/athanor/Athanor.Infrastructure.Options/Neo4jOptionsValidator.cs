using Athanor.Infrastructure.Neo4j;
using Microsoft.Extensions.Options;

namespace Athanor.Infrastructure.Options;

public sealed class Neo4jOptionsValidator : IValidateOptions<Neo4jOptions>
{
    public ValidateOptionsResult Validate(string? name, Neo4jOptions options)
    {
        if (!options.Enabled)
        {
            return ValidateOptionsResult.Success;
        }

        if (string.IsNullOrWhiteSpace(options.Uri))
        {
            return ValidateOptionsResult.Fail("Neo4j:Uri is required when Neo4j is enabled.");
        }

        if (string.IsNullOrWhiteSpace(options.Username))
        {
            return ValidateOptionsResult.Fail("Neo4j:Username is required when Neo4j is enabled.");
        }

        if (string.IsNullOrWhiteSpace(options.Password))
        {
            return ValidateOptionsResult.Fail("Neo4j:Password is required when Neo4j is enabled.");
        }

        if (string.IsNullOrWhiteSpace(options.Prefix))
        {
            return ValidateOptionsResult.Fail("Neo4j:Prefix is required when Neo4j is enabled.");
        }

        return ValidateOptionsResult.Success;
    }
}

