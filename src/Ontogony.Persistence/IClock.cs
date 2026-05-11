using Ontogony.Primitives;

namespace Ontogony.Persistence;

[Obsolete("Use Ontogony.Primitives.IClock instead.")]
public interface IClock
{
    DateTimeOffset UtcNow { get; }
}

[Obsolete("Use Ontogony.Primitives.SystemClock instead.")]
public sealed class SystemClock : IClock
{
    private readonly Ontogony.Primitives.SystemClock _inner = new();

    public DateTimeOffset UtcNow => _inner.UtcNow;
}
