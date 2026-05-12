using System.Collections.Concurrent;
using Ontogony.Primitives;

namespace Ontogony.Http;

/// <summary>
/// Tracks retry budget (retries per minute) per client.
/// </summary>
internal sealed class RetryBudgetTracker
{
    private readonly ConcurrentDictionary<string, BudgetWindow> _budgets = new(StringComparer.Ordinal);
    private readonly IClock _clock;

    public RetryBudgetTracker(IClock clock)
    {
        _clock = clock;
    }

    /// <summary>
    /// Attempt to consume one retry from the budget for the given client.
    /// </summary>
    /// <returns>True if the retry was allowed; false if budget exhausted.</returns>
    public bool TryConsumeRetry(string clientName, int budgetPerMinute)
    {
        if (budgetPerMinute <= 0)
        {
            // Budget disabled
            return true;
        }

        var now = _clock.UtcNow;
        var window = _budgets.GetOrAdd(clientName, _ => new BudgetWindow(now));

        lock (window.Sync)
        {
            var minuteAgo = now.AddMinutes(-1);
            if (window.WindowStart < minuteAgo)
            {
                window.WindowStart = now;
                window.RetriesUsed = 0;
            }

            if (window.RetriesUsed >= budgetPerMinute)
            {
                return false;
            }

            window.RetriesUsed++;
            return true;
        }
    }

    private sealed class BudgetWindow
    {
        public BudgetWindow(DateTimeOffset windowStart)
        {
            WindowStart = windowStart;
        }

        public object Sync { get; } = new();

        public DateTimeOffset WindowStart { get; set; }

        public int RetriesUsed { get; set; }
    }
}
