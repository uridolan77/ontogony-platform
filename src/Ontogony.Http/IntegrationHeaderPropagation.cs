using System.Net.Http.Headers;
using Ontogony.Contracts.Events;
using Ontogony.Observability;

namespace Ontogony.Http;

/// <summary>
/// Shared helpers for outbound Ontogony integration header propagation.
/// </summary>
internal static class IntegrationHeaderPropagation
{
    internal static void Apply(
        HttpRequestHeaders headers,
        CorrelationState? correlation,
        IntegrationOutboundState? integration,
        IReadOnlyList<IOutboundActorPropagator> propagators)
    {
        if (correlation is not null)
        {
            AddIfMissing(headers, OntogonyIntegrationHeaders.CorrelationId, correlation.TraceId);
            AddIfMissing(headers, OntogonyEventHeaders.TraceId, correlation.TraceId);
            AddIfMissing(headers, OntogonyIntegrationHeaders.ActorId, correlation.ActorId);
            AddIfMissing(headers, OntogonyEventHeaders.ActorId, correlation.ActorId);
            AddIfMissing(headers, OntogonyIntegrationHeaders.TenantId, correlation.TenantId);
            AddIfMissing(headers, OntogonyEventHeaders.TenantId, correlation.TenantId);
            AddIfMissing(headers, OntogonyIntegrationHeaders.WorkspaceId, correlation.WorkspaceId);
            AddIfMissing(headers, OntogonyEventHeaders.WorkspaceId, correlation.WorkspaceId);
            AddIfMissing(headers, OntogonyEventHeaders.ProjectId, correlation.ProjectId);
            AddIfMissing(headers, OntogonyEventHeaders.SessionId, correlation.SessionId);
            AddIfMissing(headers, OntogonyEventHeaders.TraceParent, correlation.TraceParent);
            AddIfMissing(headers, OntogonyEventHeaders.TraceState, correlation.TraceState);
        }

        if (integration is not null)
        {
            AddIfMissing(headers, OntogonyIntegrationHeaders.IdempotencyKey, integration.IdempotencyKey);
            AddIfMissing(headers, OntogonyIntegrationHeaders.ActorType, integration.ActorType);
            AddRolesIfMissing(headers, integration.ActorRoles);
        }

        foreach (var propagator in propagators)
        {
            if (!propagator.TryGetActor(out var actor))
            {
                continue;
            }

            AddIfMissing(headers, OntogonyIntegrationHeaders.ActorId, actor.ActorId);
            AddIfMissing(headers, OntogonyEventHeaders.ActorId, actor.ActorId);
            AddIfMissing(headers, OntogonyIntegrationHeaders.ActorType, actor.ActorType);
            AddRolesIfMissing(headers, actor.Roles);
        }
    }

    internal static bool HasIdempotencyKey(HttpRequestHeaders headers)
    {
        foreach (var name in new[]
                 {
                     OntogonyIntegrationHeaders.IdempotencyKey,
                     OntogonyIntegrationHeaders.LegacyIdempotencyKey,
                     OntogonyEventHeaders.IdempotencyKey,
                 })
        {
            if (headers.TryGetValues(name, out var values) && values.Any(static v => !string.IsNullOrWhiteSpace(v)))
            {
                return true;
            }
        }

        return false;
    }

    private static void AddRolesIfMissing(HttpRequestHeaders headers, IReadOnlyList<string>? roles)
    {
        if (roles is null || roles.Count == 0)
        {
            return;
        }

        var joined = string.Join(',', roles);
        AddIfMissing(headers, OntogonyIntegrationHeaders.ActorRoles, joined);
        AddIfMissing(headers, OntogonyIntegrationHeaders.LegacyActorRoles, joined);
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
