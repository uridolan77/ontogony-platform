using Ontogony.Contracts.Events;
using Ontogony.Http;

namespace Ontogony.SystemCompatibility;

/// <summary>
/// Frozen cross-service propagation headers enforced by PLATFORM-9-001.
/// </summary>
public static class SystemCompatibilityPropagationHeaders
{
    public static IReadOnlyList<string> Required { get; } =
    [
        OntogonyEventHeaders.TraceParent,
        OntogonyIntegrationHeaders.LegacyCorrelationId,
        OntogonyIntegrationHeaders.ActorId,
        OntogonyIntegrationHeaders.ActorType,
        OntogonyIntegrationHeaders.ActorRoles,
        OntogonyIntegrationHeaders.LegacyIdempotencyKey,
        "X-Allagma-Run-Id"
    ];
}
