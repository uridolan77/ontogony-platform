namespace Ontogony.Http;

/// <summary>
/// Canonical HTTP header names for Ontogony service-to-service integration calls.
/// </summary>
public static class OntogonyIntegrationHeaders
{
    /// <summary>Ontogony idempotency key for unsafe-method retries.</summary>
    public const string IdempotencyKey = "X-Ontogony-Idempotency-Key";

    /// <summary>Actor identifier.</summary>
    public const string ActorId = "X-Ontogony-Actor-Id";

    /// <summary>Actor type classifier (opaque string).</summary>
    public const string ActorType = "X-Ontogony-Actor-Type";

    /// <summary>Comma-separated actor roles.</summary>
    public const string ActorRoles = "X-Ontogony-Actor-Roles";

    /// <summary>Tenant scope identifier.</summary>
    public const string TenantId = "X-Ontogony-Tenant-Id";

    /// <summary>Workspace scope identifier.</summary>
    public const string WorkspaceId = "X-Ontogony-Workspace-Id";

    /// <summary>Correlation identifier for cross-service tracing.</summary>
    public const string CorrelationId = "X-Ontogony-Correlation-Id";

    /// <summary>Legacy correlation header accepted for inbound interop.</summary>
    public const string LegacyCorrelationId = "X-Correlation-ID";

    /// <summary>Legacy roles header accepted for inbound interop.</summary>
    public const string LegacyActorRoles = "X-Ontogony-Roles";

    /// <summary>Legacy idempotency header accepted for retry-policy interop.</summary>
    public const string LegacyIdempotencyKey = "Idempotency-Key";
}
