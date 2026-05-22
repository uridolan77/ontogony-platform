using Ontogony.Contracts.Events;

namespace Ontogony.Http;

/// <summary>
/// Frozen cross-service HTTP propagation headers (PLATFORM-9-003).
/// Services prove outbound propagation via <c>Ontogony.Testing.HeaderPropagationConformanceAssertions</c>.
/// </summary>
public static class OntogonyPropagationHeaderContract
{
    /// <summary>Allagma run spine header (product-owned name; propagated via <see cref="IntegrationOutboundState.AdditionalHeaders"/>).</summary>
    public const string AllagmaRunId = "X-Allagma-Run-Id";

    /// <summary>
    /// Headers every Ontogony runtime service must document and support on outbound integration calls
    /// when the corresponding context values are present.
    /// </summary>
    public static IReadOnlyList<string> FrozenRequired { get; } =
    [
        OntogonyEventHeaders.TraceParent,
        OntogonyIntegrationHeaders.LegacyCorrelationId,
        OntogonyIntegrationHeaders.ActorId,
        OntogonyIntegrationHeaders.ActorType,
        OntogonyIntegrationHeaders.ActorRoles,
        OntogonyIntegrationHeaders.IdempotencyKey,
        AllagmaRunId,
    ];

    /// <summary>
    /// Frozen headers plus canonical Ontogony aliases operators should prefer in new integrations.
    /// </summary>
    public static IReadOnlyList<string> FrozenWithCanonicalAliases { get; } =
    [
        .. FrozenRequired,
        OntogonyEventHeaders.TraceId,
        OntogonyIntegrationHeaders.CorrelationId,
    ];

    /// <summary>
    /// Legacy inbound/outbound aliases accepted for interop; gate docs must mention each entry.
    /// </summary>
    public static IReadOnlyList<string> LegacyInteropAliases { get; } =
    [
        OntogonyIntegrationHeaders.LegacyIdempotencyKey,
        OntogonyIntegrationHeaders.LegacyActorRoles,
    ];
}
