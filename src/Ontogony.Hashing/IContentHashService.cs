namespace Ontogony.Hashing;

public interface IContentHashService
{
    string ComputeUtf8Sha256(string content);
    string ComputeBytesSha256(ReadOnlySpan<byte> bytes);
}
