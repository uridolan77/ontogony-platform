using Microsoft.Extensions.Options;

namespace Athanor.Infrastructure.Options;

public sealed class BackgroundWorkersOptionsValidator : IValidateOptions<BackgroundWorkersOptions>
{
    public ValidateOptionsResult Validate(string? name, BackgroundWorkersOptions options)
    {
        if (options.PollIntervalMs <= 0)
        {
            return ValidateOptionsResult.Fail("BackgroundWorkers:PollIntervalMs must be > 0.");
        }

        return ValidateOptionsResult.Success;
    }
}

