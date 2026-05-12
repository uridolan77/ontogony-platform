namespace Ontogony.Quotas;

public sealed record QuotaWindow(DateTimeOffset StartsAt, DateTimeOffset EndsAt)
{
    public static QuotaWindow Fixed(DateTimeOffset now, TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(duration), "Window duration must be positive.");
        }

        var ticks = now.UtcTicks / duration.Ticks * duration.Ticks;
        var start = new DateTimeOffset(ticks, TimeSpan.Zero);
        return new QuotaWindow(start, start.Add(duration));
    }
}
