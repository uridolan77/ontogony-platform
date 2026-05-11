using System.Security.Cryptography;
using System.Text;

namespace Ontogony.Hashing;

public sealed class Sha256ContentHashService : IContentHashService
{
    public string ComputeUtf8Sha256(string content) =>
        ComputeBytesSha256(Encoding.UTF8.GetBytes(content));

    public string ComputeBytesSha256(ReadOnlySpan<byte> bytes)
    {
        Span<byte> hash = stackalloc byte[32];
        SHA256.HashData(bytes, hash);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
