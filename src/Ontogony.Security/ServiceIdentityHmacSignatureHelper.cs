using System.Security.Cryptography;
using System.Text;

namespace Ontogony.Security;

/// <summary>
/// Helpers to build the HMAC signing string and signatures for <see cref="ServiceIdentityCurrentActorAccessor"/> HMAC mode.
/// </summary>
public static class ServiceIdentityHmacSignatureHelper
{
    /// <summary>
    /// Builds the canonical UTF-8 string signed by HMAC mode (newline-separated components).
    /// </summary>
    public static string BuildCanonical(
        string httpMethod,
        string pathAndQuery,
        string timestamp,
        string nonce,
        string bodySha256HexLower)
    {
        ArgumentNullException.ThrowIfNull(httpMethod);
        ArgumentNullException.ThrowIfNull(pathAndQuery);
        ArgumentNullException.ThrowIfNull(timestamp);
        ArgumentNullException.ThrowIfNull(nonce);
        ArgumentNullException.ThrowIfNull(bodySha256HexLower);

        return $"{httpMethod}\n{pathAndQuery}\n{timestamp}\n{nonce}\n{bodySha256HexLower}";
    }

    /// <summary>Lowercase hex SHA-256 of the given body bytes.</summary>
    public static string ComputeBodyHashHexLower(ReadOnlySpan<byte> body) =>
        Convert.ToHexString(SHA256.HashData(body)).ToLowerInvariant();

    /// <summary>Computes HMAC-SHA256 over the UTF-8 canonical string.</summary>
    public static byte[] ComputeHmacUtf8(string secret, string canonical)
    {
        ArgumentNullException.ThrowIfNull(secret);
        ArgumentNullException.ThrowIfNull(canonical);

        var key = Encoding.UTF8.GetBytes(secret);
        var data = Encoding.UTF8.GetBytes(canonical);
        using var hmac = new HMACSHA256(key);
        return hmac.ComputeHash(data);
    }

    /// <summary>Base64 encodes <see cref="ComputeHmacUtf8"/> for use in <see cref="OntogonyServiceIdentityHeaders.Signature"/>.</summary>
    public static string ComputeSignatureBase64(
        string secret,
        string httpMethod,
        string pathAndQuery,
        string timestamp,
        string nonce,
        string bodySha256HexLower)
    {
        var canonical = BuildCanonical(httpMethod, pathAndQuery, timestamp, nonce, bodySha256HexLower);
        return Convert.ToBase64String(ComputeHmacUtf8(secret, canonical));
    }
}
