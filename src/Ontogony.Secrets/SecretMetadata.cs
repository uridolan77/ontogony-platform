namespace Ontogony.Secrets;

public sealed record SecretMetadata(
    SecretRef Reference,
    SecretRotationState RotationState,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt = null,
    DateTimeOffset? ExpiresAt = null,
    IReadOnlyDictionary<string, string>? Metadata = null);
