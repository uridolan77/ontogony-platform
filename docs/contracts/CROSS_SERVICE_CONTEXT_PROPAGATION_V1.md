# Cross-service context propagation v1

**Package:** SYSTEM-COH-001 · **Owner:** `ontogony-platform` (neutral contract)

## Purpose

Define product-neutral context fields for Ontogony runtime services. Product repos implement propagation; Allagma owns run-scoped derivation and privacy rules on outbound Conexus calls.

**Detailed propagation contract:** [`HEADER_PROPAGATION_CONTRACT.md`](HEADER_PROPAGATION_CONTRACT.md) (frozen header set, conformance assertions).

**Allagma matrix:** [`allagma-dotnet/docs/system/SYSTEM_TRACE_CONTEXT_MATRIX.md`](../../allagma-dotnet/docs/system/SYSTEM_TRACE_CONTEXT_MATRIX.md).

## Headers / fields

| Context | Header / constant | Required? | Notes |
| --- | --- | ---: | --- |
| W3C trace | `traceparent` (`OntogonyEventHeaders.TraceParent`) | recommended | Preserve when supplied |
| Ontogony trace | `X-Ontogony-Trace-Id` (`OntogonyEventHeaders.TraceId`) | recommended | Explicit trace id |
| Correlation | `X-Ontogony-Correlation-Id` (`OntogonyIntegrationHeaders.CorrelationId`) | recommended | Legacy `X-Correlation-ID` inbound only |
| Idempotency | `X-Ontogony-Idempotency-Key` / `Idempotency-Key` | per mutation | Derive scoped downstream keys; do not blind-forward root key |
| Actor id | `X-Ontogony-Actor-Id` | semantic/policy calls | Do not send to Conexus model calls unless explicitly approved |
| Actor type | `X-Ontogony-Actor-Type` | semantic/policy calls | e.g. `human`, `service`, `agent` |
| Actor roles | `X-Ontogony-Actor-Roles` (legacy `X-Ontogony-Roles` inbound) | semantic/policy calls | Comma-separated |
| Runtime run id | `X-Allagma-Run-Id` (`AllagmaIntegrationHeaders.RunId`) | runtime calls | Allagma-owned execution context |

## Privacy rule

Actor identity and roles are semantic/policy context for **Kanon**. Allagma does not send actor headers to **Conexus** on model completion (`ForConexus`).

## Idempotency derivation (examples)

```text
allagma:{runId}:plan
allagma:{runId}:tool:{toolIntentId}
allagma:{runId}:model:{purpose}
allagma:{runId}:human-gate-poll:{humanGateId}:{kanonDecisionId}:{nonce}
kanon:{decisionId}:assistance:{assistanceType}
```

## Acceptance

| Proof | Location |
| --- | --- |
| Platform header constants | `Ontogony.Http.OntogonyIntegrationHeaders`, `Ontogony.Contracts.Events.OntogonyEventHeaders` |
| Allagma outbound conformance | `allagma-dotnet/tests/Allagma.Tests/AllagmaOutboundPropagationConformanceTests.cs` |
| Live correlation chain | `allagma-dotnet/scripts/lib/system-cohesion-e2e.ps1` scenario `correlation_chain` |
| SYSTEM-COH-001 validator | `allagma-dotnet/scripts/validate-system-coh-001.ps1` |
