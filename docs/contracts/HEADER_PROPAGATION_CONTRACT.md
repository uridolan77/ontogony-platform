# Header propagation contract (PLATFORM-9-003)

Platform-owned **mechanical enforcement** for cross-service HTTP header propagation. Product semantics (who may call whom, idempotency derivation rules) stay in consumer repos; this contract freezes **names** and proves **outbound wiring**.

## Frozen required headers

| Header | Constant / source | Notes |
| --- | --- | --- |
| `traceparent` | `OntogonyEventHeaders.TraceParent` | W3C trace context when `CorrelationState.TraceParent` is set |
| `X-Correlation-ID` | `OntogonyIntegrationHeaders.LegacyCorrelationId` | Legacy correlation alias (also emits `X-Ontogony-Correlation-Id`) |
| `X-Ontogony-Actor-Id` | `OntogonyIntegrationHeaders.ActorId` | Actor identity |
| `X-Ontogony-Actor-Type` | `OntogonyIntegrationHeaders.ActorType` | Actor classifier |
| `X-Ontogony-Actor-Roles` | `OntogonyIntegrationHeaders.ActorRoles` | Comma-separated roles |
| `X-Ontogony-Idempotency-Key` | `OntogonyIntegrationHeaders.IdempotencyKey` | Canonical idempotency for unsafe-method retries |
| `X-Allagma-Run-Id` | `OntogonyPropagationHeaderContract.AllagmaRunId` | Allagma run spine via `IntegrationOutboundState.AdditionalHeaders` |

Matrix: [`docs/system/propagation-header.matrix.json`](../system/propagation-header.matrix.json)

## Canonical aliases (also emitted when context is present)

| Header | When |
| --- | --- |
| `X-Ontogony-Trace-Id` | `OntogonyCorrelationContext` / inbound trace middleware |
| `X-Ontogony-Correlation-Id` | Same operation id as legacy `X-Correlation-ID` |

## Legacy interop (inbound + retry detection)

| Header | Notes |
| --- | --- |
| `Idempotency-Key` | Accepted on inbound; `IntegrationHeaderPropagation.HasIdempotencyKey` treats canonical + legacy |
| `X-Ontogony-Roles` | Legacy roles alias when roles are propagated |

## Outbound implementation

- Handler: `Ontogony.Http.IntegrationHeadersDelegatingHandler`
- Apply logic: `IntegrationHeaderPropagation` (internal)
- Per-call scope: `IntegrationClientCallOptions` / `OntogonyIntegrationContext`

## Proving propagation in service tests

Reference `Ontogony.Testing` and call:

```csharp
await HeaderPropagationConformanceAssertions.AssertIntegrationHandlerPropagatesScenarioAsync(
    new PropagationHeaderScenario(
        TraceParent: "00-4bf92f3577b34da6a3ce929d0e0e4736-00f067aa0ba902b7-01",
        CorrelationId: "corr-op-42",
        ActorId: "actor-1",
        ActorType: "service",
        ActorRoles: "operator,admin",
        IdempotencyKey: "idem-42",
        AllagmaRunId: "run-abc",
        TraceId: "trace-abc"));
```

Or assert a captured downstream request:

```csharp
HeaderPropagationConformanceAssertions.AssertFrozenHeadersOnRequest(capturedRequest, scenario);
```

## Integration docs (sibling repos)

| Service | Doc |
| --- | --- |
| Allagma | `allagma-dotnet/docs/system/SYSTEM_TRACE_CONTEXT_MATRIX.md` |
| Kanon | `kanon-dotnet/docs/integrations/IDEMPOTENCY_AND_TRACE_HEADERS.md` |
| Conexus | `conexus-dotnet/docs/architecture/BOUNDARIES.md` |

## Run

Included in the system compatibility gate:

```powershell
pwsh ./scripts/run-system-compatibility-gate.ps1 -DevRoot C:\dev
```

Standalone structural validation:

```powershell
pwsh ./scripts/validate-header-propagation-contract.ps1 -DevRoot C:\dev
```

## Checks (mechanical)

| Check id | What it proves |
| --- | --- |
| `propagation-headers` | Gate + operator trace docs list frozen headers |
| `propagation-header-matrix` | Matrix + contract doc align with `OntogonyPropagationHeaderContract` |
| `propagation-header-constants` | Gate header list matches frozen contract |
| `propagation-header-sibling-docs` | Allagma/Kanon/Conexus integration docs reference frozen headers |
| `propagation-header-testing` | `Ontogony.Testing` conformance helpers exist |

## Consumer conformance tests (done)

| Repo | ID | Test project |
| --- | --- | --- |
| `allagma-dotnet` | ALLAGMA-PROP-001 | `AllagmaOutboundPropagationConformanceTests` |
| `kanon-dotnet` | KANON-PROP-001 | `KanonConexusAssistancePropagationConformanceTests` |
| `conexus-dotnet` | CONEXUS-PROP-001 | `ConexusOutboundPropagationConformanceTests` |

Index: [`docs/planning/PHASE_TIGHT_CLOSEOUT_2026-05-22.md`](../planning/PHASE_TIGHT_CLOSEOUT_2026-05-22.md).

## Non-goals

- No live Client→Allagma→Kanon→Conexus smoke (see Allagma `scripts/system/run-system-cohesion-acceptance.ps1` and `TRACE-CONTRACT-001`).
- No product idempotency derivation rules (Allagma `AllagmaIdempotencyDerivation`).
