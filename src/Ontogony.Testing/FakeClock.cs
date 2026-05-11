using Ontogony.Persistence;

namespace Ontogony.Testing;

public sealed class FakeClock : IClock
{
    public FakeClock(DateTimeOffset? initial = null)
    {
        UtcNow = initial ?? new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
    }

    public DateTimeOffset UtcNow { get; private set; }

    public void Advance(TimeSpan by) => UtcNow = UtcNow.Add(by);
}
