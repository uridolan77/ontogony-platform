namespace Ontogony.Secrets;

/// <summary>
/// Rotation lifecycle state for a secret version (mechanical; no vault provider semantics).
/// </summary>
public enum SecretRotationState
{
    /// <summary>State unknown to the caller.</summary>
    Unknown = 0,

    /// <summary>Currently active material.</summary>
    Active = 1,

    /// <summary>Scheduled to become active.</summary>
    PendingRotation = 2,

    /// <summary>Superseded but may still decrypt historical data.</summary>
    Previous = 3,

    /// <summary>No longer valid.</summary>
    Revoked = 4
}
