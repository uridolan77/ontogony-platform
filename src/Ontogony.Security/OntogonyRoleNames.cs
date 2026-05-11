namespace Ontogony.Security;

/// <summary>
/// Generic role names used across Ontogony services.
/// Do NOT add business-specific roles (GovernanceApprover, Canonizer, etc.).
/// Those belong in individual product services.
/// </summary>
public static class OntogonyRoleNames
{
    /// <summary>
    /// Human user with administrative capabilities.
    /// </summary>
    public const string HumanOperator = "human-operator";

    /// <summary>
    /// Service-to-service identity (not a person).
    /// </summary>
    public const string Service = "service";

    /// <summary>
    /// AI agent (autonomous or semi-autonomous).
    /// </summary>
    public const string Agent = "agent";

    /// <summary>
    /// Administrative role with elevated permissions.
    /// </summary>
    public const string Admin = "admin";

    /// <summary>
    /// All recognized generic role names.
    /// </summary>
    public static readonly string[] AllRoles = { HumanOperator, Service, Agent, Admin };

    /// <summary>
    /// Validate that all roles are from the generic set.
    /// </summary>
    public static bool AreAllValid(params string[] roles)
    {
        if (roles == null || roles.Length == 0)
            return false;

        return roles.All(r => AllRoles.Contains(r, StringComparer.OrdinalIgnoreCase));
    }
}
