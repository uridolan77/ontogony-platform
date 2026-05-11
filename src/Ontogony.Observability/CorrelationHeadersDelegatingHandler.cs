using System.Net.Http.Headers;
using Ontogony.Contracts.Events;

namespace Ontogony.Observability;

public sealed class CorrelationHeadersDelegatingHandler : DelegatingHandler
{
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
        }

        return base.SendAsync(request, cancellationToken);
    }

    private static void AddIfMissing(HttpRequestHeaders headers, string name, string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || headers.Contains(name)) return;
        headers.TryAddWithoutValidation(name, value);
    }
}
