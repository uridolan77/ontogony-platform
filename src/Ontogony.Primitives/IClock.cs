namespace Ontogony.Primitives;

/// <summary>
/// Testable abstraction over wall-clock time (UTC).
/// </summary>
public interface IClock
{
    /// <summary>Current instant in UTC.</summary>
    DateTimeOffset UtcNow { get; }
}

/// <summary>
/// Default <see cref="IClock"/> backed by <see cref="DateTimeOffset.UtcNow"/>.
/// </summary>
public sealed class SystemClock : IClock
{
    /// <inheritdoc />
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}