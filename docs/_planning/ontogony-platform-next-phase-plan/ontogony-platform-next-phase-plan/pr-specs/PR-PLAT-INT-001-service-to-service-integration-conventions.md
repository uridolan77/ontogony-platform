# PR-PLAT-INT-001 — Service-to-Service Integration Conventions

## Purpose

Provide reusable mechanics for HTTP calls between Ontogony services.

## Proposed scope

Add to `Ontogony.Http` or adjacent service-mechanics package:

```csharp
public static class OntogonyIntegrationHeaders
{
    public const string IdempotencyKey = "X-Ontogony-Idempotency-Key";
    public const string ActorId = "X-Ontogony-Actor-Id";
    public const string ActorType = "X-Ontogony-Actor-Type";
    public const string ActorRoles = "X-Ontogony-Actor-Roles";
    public const string TenantId = "X-Ontogony-Tenant-Id";
    public const string WorkspaceId = "X-Ontogony-Workspace-Id";
    public const string CorrelationId = "X-Ontogony-Correlation-Id";
}
```

Add typed registration helper:

```csharp
services.AddOntogonyIntegrationHttpClient<TClient, TImplementation>(
    name: "kanon",
    configureClient: ...);
```

## Must compose

```text
correlation propagation
actor propagation
optional idempotency header injection
integration error mapping
integration metrics hook
```

## Non-goals

```text
No KanonClient.
No AgentorClient.
No ConexusClient.
No service discovery framework.
No product retry-safety decision logic.
```

## Tests

- typed client registration works;
- correlation headers propagate;
- actor headers propagate when actor context exists;
- idempotency header can be supplied;
- no forbidden package edges.

## Acceptance

Product repos can build typed clients using platform mechanics without importing each other's implementation assemblies.
