using System.Security.Cryptography;
using System.Text;

namespace Ontogony.Hashing;

/// <summary>
/// <see cref="IContentHashService"/> backed by <see cref="System.Security.Cryptography.SHA256"/>.
/// </summary>
public sealed class Sha256ContentHashService : IContentHashService
{
    /// <inheritdoc />
    public string ComputeUtf8Sha256(string content) =>
        ComputeBytesSha256(Encoding.UTF8.GetBytes(content));

    /// <inheritdoc />
    public string ComputeBytesSha256(ReadOnlySpan<byte> bytes)
    {
        Span<byte> hash = stackalloc byte[32];
        SHA256.HashData(bytes, hash);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
