namespace Ontogony.Quotas;

/// <summary>
/// Half-open time window <c>[StartsAt, EndsAt)</c> for quota accounting.
/// </summary>
/// <param name="StartsAt">Inclusive window start (UTC).</param>
/// <param name="EndsAt">Exclusive window end (UTC).</param>
public sealed record QuotaWindow(DateTimeOffset StartsAt, DateTimeOffset EndsAt)
{
    /// <summary>Computes the fixed window of <paramref name="duration"/> containing <paramref name="now"/>.</summary>
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
