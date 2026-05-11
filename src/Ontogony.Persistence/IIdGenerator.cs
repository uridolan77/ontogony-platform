namespace Ontogony.Persistence;

[Obsolete("Use Ontogony.Primitives.IIdGenerator instead.")]
public interface IIdGenerator
{
    Guid NewGuid();
    string NewId(string prefix);
}

[Obsolete("Use Ontogony.Primitives.GuidIdGenerator instead.")]
public sealed class GuidIdGenerator : IIdGenerator
{
    private readonly Ontogony.Primitives.GuidIdGenerator _inner = new();

    public Guid NewGuid() => _inner.NewGuid();

    public string NewId(string prefix) => _inner.NewId(prefix);
}
