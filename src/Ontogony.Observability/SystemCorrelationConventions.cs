using System.Diagnostics;
using Ontogony.Contracts.Events;

namespace Ontogony.Observability;

/// <summary>
/// Frozen mappings between HTTP headers, OpenTelemetry span attributes, and structured log scope keys.
/// Product repos should use these conventions so traces, logs, and operator deep links align.
/// </summary>
public static class SystemCorrelationConventions
{
    /// <summary>Canonical trace header (<see cref="OntogonyEventHeaders.TraceId"/>).</summary>
    public const string TraceHeader = OntogonyEventHeaders.TraceId;

    /// <summary>Canonical correlation header (<see cref="OntogonyEventHeaders.CorrelationId"/>).</summary>
    public const string CorrelationHeader = OntogonyEventHeaders.CorrelationId;

    /// <summary>Structured logging scope key for trace id (camelCase, Serilog-friendly).</summary>
    public const string LogScopeTraceId = "traceId";

    /// <summary>Structured logging scope key for operation / correlation id.</summary>
    public const string LogScopeOperationId = "operationId";

    /// <summary>HTTP header → OTEL span attribute for correlation fields carried on <see cref="CorrelationState"/>.</summary>
    public static IReadOnlyList<HeaderSpanMapping> CorrelationHeaderSpanMappings { get; } =
    [
        new(TraceHeader, OntogonySpanAttributes.TraceId, LogScopeTraceId),
        new(CorrelationHeader, OntogonySpanAttributes.OperationId, LogScopeOperationId),
        new(OntogonyEventHeaders.ActorId, OntogonySpanAttributes.ActorId, "actorId"),
        new(OntogonyEventHeaders.TenantId, OntogonySpanAttributes.TenantId, "tenantId"),
        new(OntogonyEventHeaders.WorkspaceId, OntogonySpanAttributes.WorkspaceId, "workspaceId"),
        new(OntogonyEventHeaders.ProjectId, OntogonySpanAttributes.ProjectId, "projectId"),
        new(OntogonyEventHeaders.SessionId, OntogonySpanAttributes.SessionId, "sessionId"),
    ];

    /// <summary>Applies <paramref name="state"/> to <paramref name="activity"/> using frozen span attribute keys.</summary>
    public static void ApplyToActivity(Activity? activity, CorrelationState state)
    {
        if (activity is null)
        {
            return;
        }

        activity.SetTag(OntogonySpanAttributes.TraceId, state.TraceId);
        activity.SetTag(OntogonySpanAttributes.OperationId, state.OperationId);
        SetIfPresent(activity, OntogonySpanAttributes.ActorId, state.ActorId);
        SetIfPresent(activity, OntogonySpanAttributes.TenantId, state.TenantId);
        SetIfPresent(activity, OntogonySpanAttributes.WorkspaceId, state.WorkspaceId);
        SetIfPresent(activity, OntogonySpanAttributes.ProjectId, state.ProjectId);
        SetIfPresent(activity, OntogonySpanAttributes.SessionId, state.SessionId);
    }

    /// <summary>Builds structured logging scope fields from correlation state.</summary>
    public static IReadOnlyDictionary<string, object?> ToLogScope(CorrelationState state, string? serviceName = null)
    {
        var scope = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
        {
            [LogScopeTraceId] = state.TraceId,
            [LogScopeOperationId] = state.OperationId,
        };

        if (!string.IsNullOrWhiteSpace(serviceName))
        {
            scope["service"] = serviceName;
        }

        AddIfPresent(scope, "actorId", state.ActorId);
        AddIfPresent(scope, "tenantId", state.TenantId);
        AddIfPresent(scope, "workspaceId", state.WorkspaceId);
        AddIfPresent(scope, "projectId", state.ProjectId);
        AddIfPresent(scope, "sessionId", state.SessionId);

        return scope;
    }

    private static void SetIfPresent(Activity activity, string key, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            activity.SetTag(key, value);
        }
    }

    private static void AddIfPresent(IDictionary<string, object?> scope, string key, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            scope[key] = value;
        }
    }
}

/// <summary>Triple mapping for header propagation, span tags, and log scopes.</summary>
/// <param name="HttpHeader">Incoming/outgoing HTTP header name.</param>
/// <param name="SpanAttribute">OpenTelemetry span attribute key.</param>
/// <param name="LogScopeKey">Structured logging scope property name.</param>
public readonly record struct HeaderSpanMapping(string HttpHeader, string SpanAttribute, string LogScopeKey);
