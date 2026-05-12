using Microsoft.Extensions.Logging;
using Ontogony.Observability;

namespace Ontogony.Logging;

/// <summary>
/// Helpers for creating structured logging scopes from the current Ontogony correlation context.
/// </summary>
public static class OntogonyLoggingScope
{
    public static IDisposable BeginOntogonyScope(
        this ILogger logger,
        IReadOnlyDictionary<string, object?>? additionalFields = null,
        OntogonyLoggingOptions? options = null)
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
            foreach (var kv in additionalFields)
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
