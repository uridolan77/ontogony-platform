namespace Ontogony.Security;

/// <summary>
/// Generic actor types used by security accessors.
/// Keep actor types separate from role assignments.
/// </summary>
public static class OntogonyActorTypes
{
    /// <summary>Human user actor.</summary>
    public const string Human = "human";

    /// <summary>Service principal actor.</summary>
    public const string Service = "service";

    /// <summary>Autonomous agent actor.</summary>
    public const string Agent = "agent";
}
