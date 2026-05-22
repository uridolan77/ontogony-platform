# Header Propagation Contract

**Owner:** `Ontogony.Http.OntogonyPropagationHeaderContract`
**Schema:** `schemas/contracts/header-propagation.schema.json`

This contract defines the complete set of HTTP headers that Ontogony runtime services must
propagate on outbound integration calls.  Compliance is enforced by
`Ontogony.Testing.HeaderPropagationConformanceAssertions`.

---

## Required propagation headers

Every outbound integration call must carry these headers when the corresponding context value is
present in `OntogonyCorrelationContext`:

| Header | Source class | Required when |
| --- | --- | --- |
| `traceparent` | `OntogonyEventHeaders.TraceParent` | Always |
| `X-Correlation-ID` | `OntogonyIntegrationHeaders.LegacyCorrelationId` | Correlation context present |
| `X-Ontogony-Actor-Id` | `OntogonyIntegrationHeaders.ActorId` | Actor context present |
| `X-Ontogony-Actor-Type` | `OntogonyIntegrationHeaders.ActorType` | Actor context present |
| `X-Ontogony-Actor-Roles` | `OntogonyIntegrationHeaders.ActorRoles` | Actor context present |
| `X-Ontogony-Idempotency-Key` | `OntogonyIntegrationHeaders.IdempotencyKey` | Idempotent unsafe methods |
| `X-Allagma-Run-Id` | `OntogonyPropagationHeaderContract.AllagmaRunId` | Run spine active |

The `FrozenRequired` list in `OntogonyPropagationHeaderContract` is the machine-readable source of
truth for these seven headers.

---

## Canonical aliases (preferred in new integrations)

| Header | Source class |
| --- | --- |
| `X-Ontogony-Trace-Id` | `OntogonyEventHeaders.TraceId` |
| `X-Ontogony-Correlation-Id` | `OntogonyIntegrationHeaders.CorrelationId` |

---

## Legacy interop aliases (inbound only)

These are accepted on inbound calls for backward compatibility.  Services must not emit them on
outbound calls.

| Header | Source class | Replaced by |
| --- | --- | --- |
| `Idempotency-Key` | `OntogonyIntegrationHeaders.LegacyIdempotencyKey` | `X-Ontogony-Idempotency-Key` |
| `X-Ontogony-Roles` | `OntogonyIntegrationHeaders.LegacyActorRoles` | `X-Ontogony-Actor-Roles` |

---

## Compliance test

```csharp
// In each consumer test suite:
Ontogony.Testing.HeaderPropagationConformanceAssertions.AssertFrozenRequiredHeadersPresent(
    capturedHeaders,
    OntogonyPropagationHeaderContract.FrozenRequired);
```

---

## Scope identifiers (optional, propagated when tenancy context is active)

| Header | Source class |
| --- | --- |
| `X-Ontogony-Tenant-Id` | `OntogonyIntegrationHeaders.TenantId` |
| `X-Ontogony-Workspace-Id` | `OntogonyIntegrationHeaders.WorkspaceId` |

These are not in `FrozenRequired` because many integration endpoints are tenant-neutral.  Services
that are tenancy-aware must propagate them.
