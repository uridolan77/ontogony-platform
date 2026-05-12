using Ontogony.Primitives;

namespace Ontogony.Quotas;

/// <summary>
/// In-memory quota ledger for tests and single-process hosts. Not suitable for distributed production limiting.
/// </summary>
public sealed class InMemoryQuotaLedger : IQuotaLedger
{
    private readonly IClock _clock;
    private readonly object _gate = new();
    private readonly Dictionary<string, decimal> _usage = new(StringComparer.Ordinal);

    /// <summary>Creates an in-memory ledger, optionally with a fixed clock.</summary>
    /// <param name="clock">Clock for window boundaries; defaults to <see cref="Ontogony.Primitives.SystemClock"/>.</param>
    public InMemoryQuotaLedger(IClock? clock = null)
    {
        _clock = clock ?? new SystemClock();
    }

    /// <inheritdoc />
    public Task<QuotaDecision> TryConsumeAsync(QuotaConsumptionRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken.ThrowIfCancellationRequested();

        if (request.Amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request), "Quota consumption amount cannot be negative.");
        }

        var now = request.OccurredAt ?? _clock.UtcNow;
        var window = QuotaWindow.Fixed(now, request.Limit.Window);
        var key = BuildKey(request.Limit, window);

        lock (_gate)
        {
            _usage.TryGetValue(key, out var usedBefore);
            var usedAfter = usedBefore + request.Amount;
            if (usedAfter > request.Limit.MaxAmount)
            {
                return Task.FromResult(new QuotaDecision(
                    QuotaOutcome.Rejected,
                    request.Limit,
                    window,
                    usedBefore,
                    request.Amount,
                    usedBefore,
                    Math.Max(0, request.Limit.MaxAmount - usedBefore),
                    "quota_exceeded"));
            }

            _usage[key] = usedAfter;
            return Task.FromResult(new QuotaDecision(
                QuotaOutcome.Allowed,
                request.Limit,
                window,
                usedBefore,
                request.Amount,
                usedAfter,
                Math.Max(0, request.Limit.MaxAmount - usedAfter)));
        }
    }

    /// <inheritdoc />
    public Task<decimal> GetUsedAsync(QuotaLimit limit, DateTimeOffset? at = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(limit);
        cancellationToken.ThrowIfCancellationRequested();

        var window = QuotaWindow.Fixed(at ?? _clock.UtcNow, limit.Window);
        var key = BuildKey(limit, window);
        lock (_gate)
        {
            _usage.TryGetValue(key, out var used);
            return Task.FromResult(used);
        }
    }

    private static string BuildKey(QuotaLimit limit, QuotaWindow window) =>
        string.Join("|",
            limit.LimitId,
            limit.Scope.ScopeType,
            limit.Scope.ScopeId,
            limit.Metric,
            window.StartsAt.ToUnixTimeMilliseconds(),
            window.EndsAt.ToUnixTimeMilliseconds());
}
