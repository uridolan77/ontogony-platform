using Microsoft.Extensions.Options;

namespace Athanor.Infrastructure.Options;

public sealed class ProjectionOutboxOptionsValidator : IValidateOptions<ProjectionOutboxOptions>
{
    public ValidateOptionsResult Validate(string? name, ProjectionOutboxOptions options)
    {
        if (!options.Enabled)
        {
            return ValidateOptionsResult.Success;
        }

        if (options.BatchSize <= 0)
        {
            return ValidateOptionsResult.Fail("ProjectionOutbox:BatchSize must be > 0 when enabled.");
        }

        if (options.MaxAttempts <= 0)
        {
            return ValidateOptionsResult.Fail("ProjectionOutbox:MaxAttempts must be > 0 when enabled.");
        }

        return ValidateOptionsResult.Success;
    }
}

