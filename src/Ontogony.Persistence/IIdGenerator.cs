namespace Ontogony.Persistence;

/// <summary>Legacy identifier generator port; use <see cref="Ontogony.Primitives.IIdGenerator"/> instead.</summary>
[Obsolete("Use Ontogony.Primitives.IIdGenerator instead.")]
public interface IIdGenerator
{
    /// <summary>Creates a new GUID.</summary>
    Guid NewGuid();

    /// <summary>Creates a prefixed string identifier.</summary>
    string NewId(string prefix);
}

/// <summary>Legacy GUID-based identifier generator; use <see cref="Ontogony.Primitives.GuidIdGenerator"/> instead.</summary>
[Obsolete("Use Ontogony.Primitives.GuidIdGenerator instead.")]
public sealed class GuidIdGenerator : IIdGenerator
{
    private readonly Ontogony.Primitives.GuidIdGenerator _inner = new();

    /// <inheritdoc />
    public Guid NewGuid() => _inner.NewGuid();

    /// <inheritdoc />
    public string NewId(string prefix) => _inner.NewId(prefix);
}
