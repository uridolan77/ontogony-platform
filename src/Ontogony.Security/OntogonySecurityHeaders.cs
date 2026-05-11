namespace Ontogony.Security;

/// <summary>
/// HTTP header names for Ontogony security and authentication.
/// These headers complement OntogonyEventHeaders from Ontogony.Contracts.
/// </summary>
public static class OntogonySecurityHeaders
{
    /// <summary>
    /// HTTP Authorization header (standard).
    /// Format: "Bearer {jwt_token}"
    /// </summary>
    public const string Authorization = "Authorization";

    /// <summary>
    /// Ontogony-specific actor ID header.
    /// Derived from OntogonyEventHeaders.ActorId.
    /// Used in header-mode authentication.
    /// </summary>
    public const string ActorId = "X-Ontogony-Actor-Id";

    /// <summary>
    /// Comma-separated list of roles for the current actor.
    /// Used in header-mode authentication.
    /// Example: "human-operator,admin"
    /// </summary>
    public const string Roles = "X-Ontogony-Roles";

    /// <summary>
    /// Actor type (human, service, agent, etc.).
    /// Used in header-mode authentication.
    /// </summary>
    public const string ActorType = "X-Ontogony-Actor-Type";

    /// <summary>
    /// JWT Bearer token prefix.
    /// </summary>
    public const string BearerPrefix = "Bearer ";

    /// <summary>
    /// Extract and validate Bearer token from Authorization header.
    /// Returns null if header is missing or malformed.
    /// </summary>
    public static string? ExtractBearerToken(string authorizationHeader)
    {
        if (string.IsNullOrWhiteSpace(authorizationHeader))
            return null;

        if (!authorizationHeader.StartsWith(BearerPrefix, StringComparison.OrdinalIgnoreCase))
            return null;

        var token = authorizationHeader[BearerPrefix.Length..].Trim();
        return string.IsNullOrWhiteSpace(token) ? null : token;
    }
}
