using Microsoft.Extensions.Logging;
using Ontogony.Observability;
using Ontogony.Redaction;

namespace Ontogony.Logging;

/// <summary>
/// Helpers for creating structured logging scopes from the current Ontogony correlation context.
/// </summary>
public static class OntogonyLoggingScope
{
    /// <summary>
    /// Begins a logging scope with correlation fields. When <paramref name="redactor"/> is non-null,
    /// <paramref name="additionalFields"/> are passed through <see cref="IRedactor.RedactFields"/> before merge (field-name rules only).
    /// </summary>
    public static IDisposable BeginOntogonyScope(
        this ILogger logger,
        IReadOnlyDictionary<string, object?>? additionalFields = null,
        OntogonyLoggingOptions? options = null,
        IRedactor? redactor = null)
    {
        ArgumentNullException.ThrowIfNull(logger);
        options ??= new OntogonyLoggingOptions();

        var state = OntogonyCorrelationContext.Current;
        var fields = new Dictionary<string, object?>
        {
            [OntogonyLogFields.TraceId] = state?.TraceId,
            [OntogonyLogFields.OperationId] = state?.OperationId,
            [OntogonyLogFields.ServiceName] = options.ServiceName,
            [OntogonyLogFields.ServiceVersion] = options.ServiceVersion,
            [OntogonyLogFields.Environment] = options.Environment
        };

        if (options.IncludeTenantId) fields[OntogonyLogFields.TenantId] = state?.TenantId;
        if (options.IncludeWorkspaceId) fields[OntogonyLogFields.WorkspaceId] = state?.WorkspaceId;
        if (options.IncludeProjectId) fields[OntogonyLogFields.ProjectId] = state?.ProjectId;
        if (options.IncludeActorId) fields[OntogonyLogFields.ActorId] = state?.ActorId;
        if (options.IncludeSessionId) fields[OntogonyLogFields.SessionId] = state?.SessionId;

        if (additionalFields is not null)
        {
            var merged = redactor is null
                ? additionalFields
                : redactor.RedactFields(additionalFields);

            foreach (var kv in merged)
            {
                fields[kv.Key] = kv.Value;
            }
        }

        return logger.BeginScope(fields) ?? NullScope.Instance;
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();
        public void Dispose() { }
    }
}
