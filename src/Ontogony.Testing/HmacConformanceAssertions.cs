using Ontogony.Security;

namespace Ontogony.Testing;

/// <summary>
/// Conformance helpers that verify a service uses <see cref="ServiceIdentityHmacSignatureHelper"/>
/// correctly for HMAC-SHA256 service-to-service authentication.
/// </summary>
public static class HmacConformanceAssertions
{
    /// <summary>
    /// Asserts that a round-trip sign → verify succeeds for the given inputs.
    /// Fails if the computed signature does not pass verification against the same secret.
    /// </summary>
    public static void AssertSignatureRoundTrip(
        string secret,
        string httpMethod,
        string pathAndQuery,
        string timestamp,
        string nonce,
        string bodySha256HexLower)
    {
        if (string.IsNullOrWhiteSpace(secret))
            throw new ArgumentException("secret cannot be empty.", nameof(secret));

        var signature = ServiceIdentityHmacSignatureHelper.ComputeSignatureBase64(
            secret, httpMethod, pathAndQuery, timestamp, nonce, bodySha256HexLower);

        AssertSignatureValid(secret, httpMethod, pathAndQuery, timestamp, nonce, bodySha256HexLower, signature);
    }

    /// <summary>
    /// Asserts that a given <paramref name="signature"/> is valid for the supplied canonical inputs.
    /// </summary>
    public static void AssertSignatureValid(
        string secret,
        string httpMethod,
        string pathAndQuery,
        string timestamp,
        string nonce,
        string bodySha256HexLower,
        string signature)
    {
        if (string.IsNullOrWhiteSpace(signature))
            throw new ArgumentException("signature cannot be empty.", nameof(signature));

        var expected = ServiceIdentityHmacSignatureHelper.ComputeSignatureBase64(
            secret, httpMethod, pathAndQuery, timestamp, nonce, bodySha256HexLower);

        if (!string.Equals(expected, signature, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"HMAC signature mismatch. Expected '{expected}' but received '{signature}'.");
        }
    }

    /// <summary>
    /// Asserts that a request with a tampered signature is detected as invalid.
    /// Throws <see cref="InvalidOperationException"/> if the tampered signature matches
    /// (which would indicate a broken verification implementation).
    /// </summary>
    public static void AssertTamperedSignatureIsRejected(
        string secret,
        string httpMethod,
        string pathAndQuery,
        string timestamp,
        string nonce,
        string bodySha256HexLower)
    {
        var tampered = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";
        var expected = ServiceIdentityHmacSignatureHelper.ComputeSignatureBase64(
            secret, httpMethod, pathAndQuery, timestamp, nonce, bodySha256HexLower);

        if (string.Equals(expected, tampered, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "Test setup error: tampered signature accidentally matches the real signature. Use a different secret.");
        }

        // If we try to validate tampered against the real expected, it must differ.
        if (string.Equals(expected, tampered, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "HMAC verification accepted a tampered signature. The signing implementation appears to be broken.");
        }
    }

    /// <summary>
    /// Asserts that the canonical signing string is built from the five inputs in the expected
    /// newline-delimited order: method, path, timestamp, nonce, bodyHash.
    /// </summary>
    public static void AssertCanonicalStringFormat(
        string httpMethod,
        string pathAndQuery,
        string timestamp,
        string nonce,
        string bodySha256HexLower)
    {
        var canonical = ServiceIdentityHmacSignatureHelper.BuildCanonical(
            httpMethod, pathAndQuery, timestamp, nonce, bodySha256HexLower);

        var parts = canonical.Split('\n');
        if (parts.Length != 5)
        {
            throw new InvalidOperationException(
                $"Canonical HMAC string must have exactly 5 newline-separated components but has {parts.Length}.");
        }

        if (parts[0] != httpMethod)
            throw new InvalidOperationException($"Expected canonical[0] to be httpMethod '{httpMethod}' but got '{parts[0]}'.");

        if (parts[1] != pathAndQuery)
            throw new InvalidOperationException($"Expected canonical[1] to be pathAndQuery '{pathAndQuery}' but got '{parts[1]}'.");

        if (parts[2] != timestamp)
            throw new InvalidOperationException($"Expected canonical[2] to be timestamp '{timestamp}' but got '{parts[2]}'.");

        if (parts[3] != nonce)
            throw new InvalidOperationException($"Expected canonical[3] to be nonce '{nonce}' but got '{parts[3]}'.");

        if (parts[4] != bodySha256HexLower)
            throw new InvalidOperationException($"Expected canonical[4] to be bodyHash '{bodySha256HexLower}' but got '{parts[4]}'.");
    }

    /// <summary>
    /// Asserts that <see cref="ServiceIdentityHmacSignatureHelper.ComputeBodyHashHexLower"/> produces
    /// a lowercase hex string of length 64 (SHA-256) for the given body bytes.
    /// </summary>
    public static void AssertBodyHashFormat(ReadOnlySpan<byte> body)
    {
        var hash = ServiceIdentityHmacSignatureHelper.ComputeBodyHashHexLower(body);

        if (hash.Length != 64)
        {
            throw new InvalidOperationException(
                $"Body hash must be 64 lowercase hex characters but was {hash.Length} characters.");
        }

        if (hash != hash.ToLowerInvariant())
        {
            throw new InvalidOperationException(
                $"Body hash must be lowercase hex but got '{hash}'.");
        }
    }
}
