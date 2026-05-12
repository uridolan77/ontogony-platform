using System.Collections.Concurrent;

namespace Ontogony.Http;

/// <summary>
/// Tracks retry budget (retries per minute) per client.
/// </summary>
internal sealed class RetryBudgetTracker
{
    private readonly ConcurrentDictionary<string, BudgetWindow> _budgets = new();

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

        var now = DateTimeOffset.UtcNow;
        var window = _budgets.AddOrUpdate(clientName,
            new BudgetWindow { WindowStart = now, RetriesUsed = 1 },
            (_, current) =>
            {
                var minuteAgo = now.AddMinutes(-1);
                if (current.WindowStart < minuteAgo)
                {
                    // Window expired, reset
                    return new BudgetWindow { WindowStart = now, RetriesUsed = 1 };
                }

                // Within current window
                if (current.RetriesUsed >= budgetPerMinute)
                {
                    // Budget exhausted
                    return current;
                }

                current.RetriesUsed++;
                return current;
            });

        return window.RetriesUsed <= budgetPerMinute;
    }

    private sealed class BudgetWindow
    {
        public DateTimeOffset WindowStart { get; set; }
        public int RetriesUsed { get; set; }
    }
}
