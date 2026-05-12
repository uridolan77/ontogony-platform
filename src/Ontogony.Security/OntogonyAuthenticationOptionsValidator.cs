using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Ontogony.Security;

/// <summary>
/// Startup validation for <see cref="OntogonyAuthenticationOptions"/> against the host environment.
/// </summary>
/// <param name="hostEnvironment">Host environment used to gate unsafe dev-only settings.</param>
public sealed class OntogonyAuthenticationOptionsValidator(IHostEnvironment hostEnvironment)
    : IValidateOptions<OntogonyAuthenticationOptions>
{
    /// <inheritdoc />
    public ValidateOptionsResult Validate(string? name, OntogonyAuthenticationOptions options)
    {
        if (options.Mode == OntogonyAuthenticationMode.Disabled
            && !options.AllowDisabledOutsideDevelopment
            && !IsLocalOrTestEnvironment(hostEnvironment))
        {
            return ValidateOptionsResult.Fail(
                "Ontogony authentication mode 'Disabled' is blocked outside Development/Test by default. " +
                "Set AllowDisabledOutsideDevelopment=true to override explicitly.");
        }

        if (options.Mode == OntogonyAuthenticationMode.Header
            && string.IsNullOrWhiteSpace(options.HeaderActorIdHeaderName))
        {
            return ValidateOptionsResult.Fail(
                "HeaderActorIdHeaderName is required when authentication mode is Header.");
        }

        if (options.Mode != OntogonyAuthenticationMode.Jwt)
        {
            return ValidateOptionsResult.Success;
        }

        if (options.JwtActorIdClaimTypes.Count == 0)
        {
            return ValidateOptionsResult.Fail(
                "JwtActorIdClaimTypes must include at least one claim type when authentication mode is Jwt.");
        }

        if (string.IsNullOrWhiteSpace(options.JwtRoleClaimType))
        {
            return ValidateOptionsResult.Fail(
                "JwtRoleClaimType is required when authentication mode is Jwt.");
        }

        if (string.IsNullOrWhiteSpace(options.JwtAuthority) && !options.JwtAcceptUnvalidatedBearerTokens)
        {
            return ValidateOptionsResult.Fail(
                "JWT mode requires either JwtAuthority (validated JWT) or JwtAcceptUnvalidatedBearerTokens=true.");
        }

        if (options.JwtAcceptUnvalidatedBearerTokens
            && string.IsNullOrWhiteSpace(options.JwtAuthority)
            && !IsLocalOrTestEnvironment(hostEnvironment)
            && !options.JwtAllowUnvalidatedTokensOutsideDevelopment)
        {
            return ValidateOptionsResult.Fail(
                "Unvalidated JWT parsing without JwtAuthority is blocked outside Development/Test unless " +
                "JwtAllowUnvalidatedTokensOutsideDevelopment=true.");
        }

        return ValidateOptionsResult.Success;
    }

    private static bool IsLocalOrTestEnvironment(IHostEnvironment environment)
        => environment.IsDevelopment()
           || environment.IsEnvironment("Test")
           || environment.IsEnvironment("Testing");
}
