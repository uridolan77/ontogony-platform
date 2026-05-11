namespace Ontogony.Persistence;

public interface IIdGenerator
{
    Guid NewGuid();
    string NewId(string prefix);
}

public sealed class GuidIdGenerator : IIdGenerator
{
    public Guid NewGuid() => Guid.NewGuid();
    public string NewId(string prefix) => $"{prefix}_{Guid.NewGuid():N}";
}
