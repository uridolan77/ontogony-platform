namespace Ontogony.Quotas;

/// <summary>
/// Declares a mechanical quota ceiling for a metric within a time window (product defines semantics of ids and metrics).
/// </summary>
/// <param name="LimitId">Stable limit identifier.</param>
/// <param name="Scope">Who/what the limit applies to.</param>
/// <param name="Metric">Metric name (often <see cref="QuotaMetric"/> constants).</param>
/// <param name="MaxAmount">Maximum allowed amount within each window.</param>
/// <param name="Window">Window duration (see <see cref="QuotaWindow.Fixed"/>).</param>
/// <param name="Unit">Optional display unit (opaque).</param>
public sealed record QuotaLimit(
    string LimitId,
    QuotaScope Scope,
    string Metric,
    decimal MaxAmount,
    TimeSpan Window,
    string? Unit = null);
