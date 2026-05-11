namespace Ontogony.Observability;

/// <summary>
/// Async-local correlation context for logs, outgoing HTTP headers, event envelopes, and background work.
/// </summary>
public static class OntogonyCorrelationContext
{
    private static readonly AsyncLocal<CorrelationState?> CurrentValue = new();

    public static CorrelationState? Current => CurrentValue.Value;
    public static string? TraceId => CurrentValue.Value?.TraceId;

    public static IDisposable Push(CorrelationState state)
    {
        var prior = CurrentValue.Value;
        CurrentValue.Value = state;
        return new PopScope(prior);
    }

    public static IDisposable Push(string traceId, string? operationId = null)
    {
        return Push(new CorrelationState(traceId, operationId ?? Guid.NewGuid().ToString("n")));
    }

    private sealed class PopScope : IDisposable
    {
        private readonly CorrelationState? _prior;
        private bool _disposed;

        public PopScope(CorrelationState? prior) => _prior = prior;

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            CurrentValue.Value = _prior;
        }
    }
}

public sealed record CorrelationState(
    string TraceId,
    string OperationId,
    string? TenantId = null,
    string? WorkspaceId = null,
    string? ProjectId = null,
    string? ActorId = null,
    string? SessionId = null);
