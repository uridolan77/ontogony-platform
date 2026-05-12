namespace Ontogony.Secrets;

public enum SecretRotationState
{
    Unknown = 0,
    Active = 1,
    PendingRotation = 2,
    Previous = 3,
    Revoked = 4
}
