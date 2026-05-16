using System.Diagnostics;
using Ontogony.Contracts.Events;

namespace Ontogony.Observability;

/// <summary>
/// Async-local correlation context for logs, outgoing HTTP headers, event envelopes, and background work.
/// </summary>
public static class OntogonyCorrelationContext
{
    private static readonly string[] DefaultAcceptedIncomingTraceHeaders =
    [
        OntogonyEventHeaders.TraceId,
        OntogonyEventHeaders.LegacyAthanorTraceId,
        OntogonyEventHeaders.LegacyAgentorTraceId,
        OntogonyEventHeaders.ConexusRequestId
    ];

    private static readonly AsyncLocal<CorrelationState?> CurrentValue = new();

    /// <summary>Current async-local correlation state, if any.</summary>
    public static CorrelationState? Current => CurrentValue.Value;

    /// <summary>Convenience for <see cref="Current"/>?.TraceId.</summary>
    public static string? TraceId => CurrentValue.Value?.TraceId;

    /// <summary>Returns existing state or creates a new one with a generated trace id.</summary>
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

    /// <summary>Replaces the current correlation state until the returned scope is disposed.</summary>
    public static IDisposable Push(CorrelationState state)
    {
        var prior = CurrentValue.Value;
        CurrentValue.Value = state;
        return new PopScope(prior);
    }

    /// <summary>Creates and pushes a <see cref="CorrelationState"/> for <paramref name="traceId"/>.</summary>
    public static IDisposable Push(string traceId, string? operationId = null)
    {
        return Push(new CorrelationState(traceId, operationId ?? Guid.NewGuid().ToString("n")));
    }

    /// <summary>
    /// Resolves correlation from headers using the default canonical trace header and default accepted legacy aliases
    /// (same defaults as <see cref="OntogonyObservabilityOptions"/>).
    /// </summary>
    public static CorrelationState? FromHeaders(IReadOnlyDictionary<string, string?> headers) =>
        FromHeaders(headers, OntogonyEventHeaders.TraceId, DefaultAcceptedIncomingTraceHeaders);

    /// <summary>
    /// Resolves correlation from headers. <paramref name="canonicalTraceHeaderName"/> is always checked first for a trace id,
    /// then each entry in <paramref name="acceptedIncomingTraceHeaders"/> in order (duplicates and the canonical name are skipped).
    /// </summary>
    public static CorrelationState? FromHeaders(
        IReadOnlyDictionary<string, string?> headers,
        string canonicalTraceHeaderName,
        IReadOnlyList<string>? acceptedIncomingTraceHeaders)
    {
        if (headers is null)
        {
            return null;
        }

        var traceHeaderOrder = BuildTraceHeaderLookupOrder(canonicalTraceHeaderName, acceptedIncomingTraceHeaders);
        var traceId = FirstNonEmpty(headers, traceHeaderOrder);

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

        var operationId = ResolveOperationId(headers) ?? Guid.NewGuid().ToString("n");

        return new CorrelationState(
            traceId.Trim(),
            operationId,
            TenantId: FirstNonEmpty(headers, OntogonyEventHeaders.TenantId),
            WorkspaceId: FirstNonEmpty(headers, OntogonyEventHeaders.WorkspaceId),
            ProjectId: FirstNonEmpty(headers, OntogonyEventHeaders.ProjectId),
            ActorId: FirstNonEmpty(headers, OntogonyEventHeaders.ActorId),
            SessionId: FirstNonEmpty(headers, OntogonyEventHeaders.SessionId),
            TraceParent: traceParent,
            TraceState: FirstNonEmpty(headers, OntogonyEventHeaders.TraceState));
    }

    private static string? ResolveOperationId(IReadOnlyDictionary<string, string?> headers) =>
        FirstNonEmpty(headers, new[] { OntogonyEventHeaders.CorrelationId, OntogonyEventHeaders.LegacyCorrelationId });

    private static string[] BuildTraceHeaderLookupOrder(
        string canonicalTraceHeaderName,
        IReadOnlyList<string>? acceptedIncomingTraceHeaders)
    {
        var canonical = string.IsNullOrWhiteSpace(canonicalTraceHeaderName)
            ? OntogonyEventHeaders.TraceId
            : canonicalTraceHeaderName.Trim();

        var ordered = new List<string> { canonical };
        var source = acceptedIncomingTraceHeaders ?? DefaultAcceptedIncomingTraceHeaders;
        foreach (var header in source)
        {
            if (string.IsNullOrWhiteSpace(header))
            {
                continue;
            }

            var trimmed = header.Trim();
            if (ordered.Exists(existing => string.Equals(existing, trimmed, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            ordered.Add(trimmed);
        }

        return ordered.ToArray();
    }

    /// <summary>Builds correlation state from an envelope's trace and tenancy fields.</summary>
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

    /// <summary>Maps correlation fields to a small string dictionary for logging or metadata.</summary>
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

    private static string? FirstNonEmpty(IReadOnlyDictionary<string, string?> headers, string key)
    {
        if (headers.TryGetValue(key, out var value) && !string.IsNullOrWhiteSpace(value))
        {
            return value.Trim();
        }

        return null;
    }

    private static string? FirstNonEmpty(IReadOnlyDictionary<string, string?> headers, IReadOnlyList<string> keys)
    {
        for (var i = 0; i < keys.Count; i++)
        {
            var key = keys[i];
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

/// <summary>
/// Immutable correlation tuple carried on the async flow (trace, operation, tenancy).
/// </summary>
/// <param name="TraceId">Distributed trace id.</param>
/// <param name="OperationId">Per-request or per-unit-of-work id.</param>
/// <param name="TenantId">Optional tenant id.</param>
/// <param name="WorkspaceId">Optional workspace id.</param>
/// <param name="ProjectId">Optional project id.</param>
/// <param name="ActorId">Optional actor id.</param>
/// <param name="SessionId">Optional session id.</param>
/// <param name="TraceParent">Optional W3C traceparent.</param>
/// <param name="TraceState">Optional W3C tracestate.</param>
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
