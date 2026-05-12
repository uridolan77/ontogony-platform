namespace Ontogony.Hashing;

/// <summary>
/// Computes SHA-256 digests as lowercase hex strings.
/// </summary>
public interface IContentHashService
{
    /// <summary>Hashes UTF-8 bytes of <paramref name="content"/>.</summary>
    string ComputeUtf8Sha256(string content);

    /// <summary>Hashes raw bytes.</summary>
    string ComputeBytesSha256(ReadOnlySpan<byte> bytes);
}
