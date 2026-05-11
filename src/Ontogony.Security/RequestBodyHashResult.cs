namespace Ontogony.Security;

/// <summary>
/// Result of computing a lowercase hex SHA-256 over the raw HTTP request body for HMAC verification.
/// </summary>
public readonly record struct RequestBodyHashResult(bool TooLarge, string? HexLower)
{
    /// <summary>True when hashing succeeded and <see cref="HexLower"/> is the 64-character lowercase hex digest.</summary>
    public bool Succeeded => !TooLarge && HexLower is not null;
}
