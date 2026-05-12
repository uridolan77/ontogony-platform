using System.Net.Http.Headers;
using System.Diagnostics;
using Ontogony.Contracts.Events;
using Ontogony.Observability;

namespace Ontogony.Http;

/// <summary>
/// Propagates Ontogony correlation headers on outbound <see cref="HttpClient"/> calls when present on the async context.
/// </summary>
public sealed class CorrelationHeadersDelegatingHandler : DelegatingHandler
{
    /// <inheritdoc />
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var state = OntogonyCorrelationContext.Current;
        if (state is not null)
        {
            AddIfMissing(request.Headers, OntogonyEventHeaders.TraceId, state.TraceId);
            AddIfMissing(request.Headers, OntogonyEventHeaders.ActorId, state.ActorId);
            AddIfMissing(request.Headers, OntogonyEventHeaders.TenantId, state.TenantId);
            AddIfMissing(request.Headers, OntogonyEventHeaders.WorkspaceId, state.WorkspaceId);
            AddIfMissing(request.Headers, OntogonyEventHeaders.ProjectId, state.ProjectId);
            AddIfMissing(request.Headers, OntogonyEventHeaders.SessionId, state.SessionId);
            AddIfMissing(request.Headers, OntogonyEventHeaders.TraceParent, state.TraceParent ?? Activity.Current?.Id);
            AddIfMissing(request.Headers, OntogonyEventHeaders.TraceState, state.TraceState ?? Activity.Current?.TraceStateString);
        }
        else
        {
            AddIfMissing(request.Headers, OntogonyEventHeaders.TraceParent, Activity.Current?.Id);
            AddIfMissing(request.Headers, OntogonyEventHeaders.TraceState, Activity.Current?.TraceStateString);
        }

        return base.SendAsync(request, cancellationToken);
    }

    private static void AddIfMissing(HttpRequestHeaders headers, string name, string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || headers.Contains(name))
        {
            return;
        }

        headers.TryAddWithoutValidation(name, value);
    }
}
