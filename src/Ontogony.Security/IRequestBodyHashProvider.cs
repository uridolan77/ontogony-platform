using Microsoft.AspNetCore.Http;

namespace Ontogony.Security;

/// <summary>
/// Computes a lowercase hex SHA-256 of the raw HTTP request body for HMAC verification.
/// Implementations should reset the request body stream position when buffering allows.
/// </summary>
public interface IRequestBodyHashProvider
{
    /// <summary>
    /// Computes the body digest or reports <see cref="RequestBodyHashResult.TooLarge"/> when the body exceeds configured limits.
    /// </summary>
    RequestBodyHashResult TryComputeSha256HexLower(HttpRequest request);
}
