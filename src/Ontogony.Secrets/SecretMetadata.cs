namespace Ontogony.Secrets;

/// <summary>
/// Metadata describing a logical secret without exposing its value.
/// </summary>
/// <param name="Reference">Secret reference identity.</param>
/// <param name="RotationState">Rotation lifecycle state.</param>
/// <param name="CreatedAt">Creation timestamp.</param>
/// <param name="UpdatedAt">Last update timestamp.</param>
/// <param name="ExpiresAt">Optional expiry.</param>
/// <param name="Metadata">Optional small opaque key/value metadata.</param>
public sealed record SecretMetadata(
    SecretRef Reference,
    SecretRotationState RotationState,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt = null,
    DateTimeOffset? ExpiresAt = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
