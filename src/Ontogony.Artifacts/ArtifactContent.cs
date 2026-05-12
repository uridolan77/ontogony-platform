namespace Ontogony.Artifacts;

/// <summary>
/// In-process pair of an <see cref="ArtifactRef"/> and the raw bytes returned by a store.
/// Not a wire DTO — never serialize this directly; emit the <see cref="Reference"/> instead.
/// </summary>
public sealed class ArtifactContent
{
    /// <summary>Creates a content wrapper with defensive copy semantics owned by the caller.</summary>
    /// <param name="reference">Artifact identity and metadata.</param>
    /// <param name="bytes">Raw payload bytes.</param>
    public ArtifactContent(ArtifactRef reference, ReadOnlyMemory<byte> bytes)
    {
        ArgumentNullException.ThrowIfNull(reference);
        Reference = reference;
        Bytes = bytes;
    }

    /// <summary>Artifact identity and metadata.</summary>
    public ArtifactRef Reference { get; }

    /// <summary>Raw payload bytes.</summary>
    public ReadOnlyMemory<byte> Bytes { get; }
}
