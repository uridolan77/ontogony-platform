using System.Diagnostics;
using Ontogony.Contracts.Events;

namespace Ontogony.Observability;

/// <summary>
/// Async-local correlation context for logs, outgoing HTTP headers, event envelopes, and background work.
/// </summary>
public static class OntogonyCorrelationContext
{
    private static readonly AsyncLocal<CorrelationState?> CurrentValue = new();

    public static CorrelationState? Current => CurrentValue.Value;
    public static string? TraceId => CurrentValue.Value?.TraceId;

    public static CorrelationState CurrentOrCreate()
    {
        if (CurrentValue.Value is not null)
        {
            return CurrentValue.Value;
        }

        var created = new CorrelationState(
            CreateFallbackTraceId(),
            Guid.NewGuid().ToString("n"),
            TraceParent: Activity.Current?.Id,
            TraceState: Activity.Current?.TraceStateString);

        CurrentValue.Value = created;
        return created;
    }

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

    public static CorrelationState? FromHeaders(IReadOnlyDictionary<string, string?> headers)
    {
        if (headers is null)
        {
            return null;
        }

        var traceId = FirstNonEmpty(headers,
            OntogonyEventHeaders.TraceId,
            OntogonyEventHeaders.LegacyAthanorTraceId,
            OntogonyEventHeaders.LegacyAgentorTraceId,
            OntogonyEventHeaders.ConexusRequestId);

        var traceParent = FirstNonEmpty(headers, OntogonyEventHeaders.TraceParent);
        if (string.IsNullOrWhiteSpace(traceId) &&
            !string.IsNullOrWhiteSpace(traceParent) &&
            ActivityContext.TryParse(traceParent, FirstNonEmpty(headers, OntogonyEventHeaders.TraceState), out var parsedContext))
        {
            traceId = parsedContext.TraceId.ToHexString();
        }

        if (string.IsNullOrWhiteSpace(traceId))
        {
            return null;
        }

        return new CorrelationState(
            traceId.Trim(),
            Guid.NewGuid().ToString("n"),
            TenantId: FirstNonEmpty(headers, OntogonyEventHeaders.TenantId),
            WorkspaceId: FirstNonEmpty(headers, OntogonyEventHeaders.WorkspaceId),
            ProjectId: FirstNonEmpty(headers, OntogonyEventHeaders.ProjectId),
            ActorId: FirstNonEmpty(headers, OntogonyEventHeaders.ActorId),
            SessionId: FirstNonEmpty(headers, OntogonyEventHeaders.SessionId),
            TraceParent: traceParent,
            TraceState: FirstNonEmpty(headers, OntogonyEventHeaders.TraceState));
    }

    public static CorrelationState? FromEnvelope<TPayload>(OntogonyEnvelope<TPayload> envelope)
    {
        if (envelope is null)
        {
            return null;
        }

        return new CorrelationState(
            envelope.TraceId,
            Guid.NewGuid().ToString("n"),
            TenantId: envelope.TenantId,
            WorkspaceId: envelope.WorkspaceId,
            ProjectId: envelope.ProjectId,
            ActorId: envelope.ActorId,
            SessionId: envelope.SessionId);
    }

    public static IReadOnlyDictionary<string, string> ToMetadata(CorrelationState? state = null)
    {
        var source = state ?? CurrentValue.Value;
        if (source is null)
        {
            return new Dictionary<string, string>();
        }

        var metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [OntogonyEventHeaders.TraceId] = source.TraceId,
            ["operation_id"] = source.OperationId
        };

        AddIfNotEmpty(metadata, OntogonyEventHeaders.ActorId, source.ActorId);
        AddIfNotEmpty(metadata, OntogonyEventHeaders.TenantId, source.TenantId);
        AddIfNotEmpty(metadata, OntogonyEventHeaders.WorkspaceId, source.WorkspaceId);
        AddIfNotEmpty(metadata, OntogonyEventHeaders.ProjectId, source.ProjectId);
        AddIfNotEmpty(metadata, OntogonyEventHeaders.SessionId, source.SessionId);
        AddIfNotEmpty(metadata, OntogonyEventHeaders.TraceParent, source.TraceParent);
        AddIfNotEmpty(metadata, OntogonyEventHeaders.TraceState, source.TraceState);

        return metadata;
    }

    private static string CreateFallbackTraceId()
    {
        if (ActivityTraceId.CreateRandom() is var traceId && traceId != default)
        {
            return traceId.ToHexString();
        }

        return Guid.NewGuid().ToString("n");
    }

    private static string? FirstNonEmpty(IReadOnlyDictionary<string, string?> headers, params string[] keys)
    {
        foreach (var key in keys)
        {
            if (headers.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }
        }

        return null;
    }

    private static void AddIfNotEmpty(IDictionary<string, string> metadata, string key, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            metadata[key] = value;
        }
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
    string? SessionId = null,
    string? TraceParent = null,
    string? TraceState = null);
