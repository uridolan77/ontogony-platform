using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Ontogony.Security;

public enum OntogonyAuthenticationMode
{
    Disabled,
    Header,
    Jwt
}

public sealed class OntogonyAuthenticationOptions
{
    public const string SectionName = "Ontogony:Security:Authentication";

    public OntogonyAuthenticationMode Mode { get; set; } = OntogonyAuthenticationMode.Header;

    public bool AllowDisabledOutsideDevelopment { get; set; }

    [Required]
    public string HeaderActorIdHeaderName { get; set; } = "X-Ontogony-Actor-Id";

    public List<string> JwtActorIdClaimTypes { get; set; } =
    [
        ClaimTypes.NameIdentifier,
        "sub",
        "oid"
    ];

    [Required]
    public string JwtRoleClaimType { get; set; } = ClaimTypes.Role;

    public string? JwtAuthority { get; set; }

    public string? JwtAudience { get; set; }

    public bool JwtAcceptUnvalidatedBearerTokens { get; set; }

    public bool JwtAllowUnvalidatedTokensOutsideDevelopment { get; set; }
}