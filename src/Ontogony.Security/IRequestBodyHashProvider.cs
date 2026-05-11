using Microsoft.AspNetCore.Http;

namespace Ontogony.Security;

/// <summary>
/// Computes a lowercase hex SHA-256 of the raw HTTP request body for HMAC verification.
/// Implementations should reset the request body stream position when buffering allows.
/// </summary>
public interface IRequestBodyHashProvider
{
    string ComputeSha256HexLower(HttpRequest request);
}
