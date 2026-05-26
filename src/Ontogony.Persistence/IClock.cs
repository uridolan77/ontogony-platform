using Ontogony.Primitives;

namespace Ontogony.Persistence;

/// <summary>Legacy clock port; use <see cref="Ontogony.Primitives.IClock"/> instead.</summary>
[Obsolete("Use Ontogony.Primitives.IClock instead.")]
public interface IClock
{
    /// <summary>Current UTC time.</summary>
    DateTimeOffset UtcNow { get; }
}

/// <summary>Legacy system clock; use <see cref="Ontogony.Primitives.SystemClock"/> instead.</summary>
[Obsolete("Use Ontogony.Primitives.SystemClock instead.")]
public sealed class SystemClock : IClock
{
    private readonly Ontogony.Primitives.SystemClock _inner = new();

    /// <inheritdoc />
    public DateTimeOffset UtcNow => _inner.UtcNow;
}
