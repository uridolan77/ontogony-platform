namespace Ontogony.Artifacts;

/// <summary>
/// In-process pair of an <see cref="ArtifactRef"/> and the raw bytes returned by a store.
/// Not a wire DTO — never serialize this directly; emit the <see cref="Reference"/> instead.
/// </summary>
public sealed class ArtifactContent
{
    public ArtifactContent(ArtifactRef reference, ReadOnlyMemory<byte> bytes)
    {
        ArgumentNullException.ThrowIfNull(reference);
        Reference = reference;
        Bytes = bytes;
    }

    public ArtifactRef Reference { get; }

    public ReadOnlyMemory<byte> Bytes { get; }
}
