using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Ontogony.Security;

/// <summary>
/// Authentication mode for Ontogony services.
/// </summary>
public enum OntogonyAuthenticationMode
{
    /// <summary>
    /// No authentication. Useful for local development only.
    /// Blocked in Production unless explicitly allowed via AllowDisabledOutsideDevelopment.
    /// </summary>
    Disabled,

    /// <summary>
    /// Header-based actor context (not authentication).
    /// Trusts upstream infrastructure/middleware to set headers.
    /// WARNING: Do NOT expose to public internet. Requires trusted-upstream guarantee.
    /// </summary>
    Header,

    /// <summary>
    /// JWT Bearer token authentication via standard Authorization header.
    /// Requires JwtAuthority for validation unless unvalidated tokens are explicitly allowed.
    /// </summary>
    Jwt
}

/// <summary>
/// Configuration for Ontogony security and authentication.
/// Supports three modes: Disabled (dev only), Header (trusted upstream), and JWT (token-based).
/// </summary>
public sealed class OntogonyAuthenticationOptions
{
    /// <summary>Default configuration section path for binding.</summary>
    public const string SectionName = "Ontogony:Security:Authentication";

    /// <summary>
    /// Authentication mode. Default: Header
    /// </summary>
    public OntogonyAuthenticationMode Mode { get; set; } = OntogonyAuthenticationMode.Header;

    /// <summary>
    /// If true, allow Disabled mode outside Development and Test environments.
    /// Default: false (security-safe default)
    /// DANGER: Enabling this outside development is a security risk.
    /// </summary>
    public bool AllowDisabledOutsideDevelopment { get; set; }

    // =============================================================
    // Header Mode Configuration
    // =============================================================

    /// <summary>
    /// Header name containing actor ID in Header mode.
    /// Default: X-Ontogony-Actor-Id
    /// TRUST NOTE: Header mode trusts upstream to set this correctly.
    /// Do not use on public routes without authentication upstream.
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string HeaderActorIdHeaderName { get; set; } = "X-Ontogony-Actor-Id";

    // =============================================================
    // JWT Mode Configuration
    // =============================================================

    /// <summary>
    /// Claim types to check for actor ID in JWT mode, in priority order.
    /// Defaults: sub (OIDC standard), oid (Azure), name identifier (.NET)
    /// </summary>
    public List<string> JwtActorIdClaimTypes { get; set; } =
    [
        "sub",                      // OIDC standard
        ClaimTypes.NameIdentifier,  // .NET default
        "oid"                       // Azure AD
    ];

    /// <summary>
    /// Claim type for role extraction in JWT mode.
    /// Default: http://schemas.microsoft.com/ws/2008/06/identity/claims/role
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string JwtRoleClaimType { get; set; } = ClaimTypes.Role;

    /// <summary>
    /// JWT token issuer authority URL.
    /// Required for JWT token validation unless unvalidated tokens are explicitly allowed.
    /// Example: https://login.microsoftonline.com/{tenant}/v2.0
    /// </summary>
    public string? JwtAuthority { get; set; }

    /// <summary>
    /// JWT token expected audience.
    /// Optional. If set, token audience must match.
    /// </summary>
    public string? JwtAudience { get; set; }

    /// <summary>
    /// If true, accept JWT Bearer tokens without signature validation.
    /// DANGER: This is only safe in local development.
    /// Blocked in Production/Staging unless JwtAllowUnvalidatedTokensOutsideDevelopment is true.
    /// </summary>
    public bool JwtAcceptUnvalidatedBearerTokens { get; set; }

    /// <summary>
    /// If true, allow unvalidated JWT tokens in non-Development environments.
    /// DANGER: This is a significant security risk. Only use with explicit intent.
    /// </summary>
    public bool JwtAllowUnvalidatedTokensOutsideDevelopment { get; set; }

    // =============================================================
    // Validation Method
    // =============================================================

    /// <summary>
    /// Validate the options against the configured environment and mode.
    /// Throws if configuration is unsafe.
    /// </summary>
    public void Validate(string? environment = null)
    {
        var isProduction = environment != "Development" && environment != "Test";

        // Validate Disabled mode
        if (Mode == OntogonyAuthenticationMode.Disabled)
        {
            if (isProduction && !AllowDisabledOutsideDevelopment)
            {
                throw new InvalidOperationException(
                    "Authentication mode is Disabled in a Production/non-Development environment. " +
                    "This is a security risk. Set AllowDisabledOutsideDevelopment=true only if you understand the implications.");
            }
        }

        // Validate Header mode - it trusts upstream infrastructure
        if (Mode == OntogonyAuthenticationMode.Header)
        {
            if (string.IsNullOrWhiteSpace(HeaderActorIdHeaderName))
            {
                throw new InvalidOperationException(
                    "Header mode requires HeaderActorIdHeaderName to be configured.");
            }
        }

        // Validate JWT mode
        if (Mode == OntogonyAuthenticationMode.Jwt)
        {
            if (JwtAcceptUnvalidatedBearerTokens)
            {
                if (isProduction && !JwtAllowUnvalidatedTokensOutsideDevelopment)
                {
                    throw new InvalidOperationException(
                        "JWT unvalidated tokens are only allowed in Development/Test. " +
                        "Set JwtAllowUnvalidatedTokensOutsideDevelopment=true only if you understand the security implications.");
                }
            }
            else if (string.IsNullOrWhiteSpace(JwtAuthority))
            {
                throw new InvalidOperationException(
                    "JWT mode requires either JwtAuthority for validation or JwtAcceptUnvalidatedBearerTokens for testing.");
            }
        }
    }
}