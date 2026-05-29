namespace Ontogony.SystemTests.Infrastructure;

/// <summary>Header names aligned with Ontogony.Http / Ontogony.Contracts.Events (harness does not reference platform assemblies).</summary>
public static class OntogonyHeaders
{
    public const string CorrelationId = "X-Correlation-ID";
    public const string TraceId = "X-Ontogony-Trace-Id";
    public const string ActorId = "X-Ontogony-Actor-Id";
    public const string ActorType = "X-Ontogony-Actor-Type";
    public const string ActorRoles = "X-Ontogony-Actor-Roles";
    public const string LegacyActorRoles = "X-Ontogony-Roles";
    public const string IdempotencyKey = "X-Ontogony-Idempotency-Key";
    public const string LegacyIdempotencyKey = "Idempotency-Key";
    public const string ConexusAdminKey = "X-Conexus-Admin-Key";
}
