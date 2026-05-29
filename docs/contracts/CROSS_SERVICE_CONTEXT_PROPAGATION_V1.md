# Cross-service context propagation — v1

**Package:** `CROSS-REPO-IDENTITY-CORRELATION-001`  
**Owner:** `ontogony-platform` (`Ontogony.Http`, `Ontogony.Contracts`, `Ontogony.Security`)  
**Status:** promoted (2026-05-29)

## Purpose

Define which headers propagate across Ontogony HTTP calls so traces, correlation, actors, and idempotency survive Allagma → Kanon → Conexus → Metabole → Aisthesis chains.

**Frozen header set:** [`HEADER_PROPAGATION_CONTRACT.md`](./HEADER_PROPAGATION_CONTRACT.md) (PLATFORM-9-003).  
**Allagma matrix:** [`allagma-dotnet/docs/system/SYSTEM_TRACE_CONTEXT_MATRIX.md`](../../allagma-dotnet/docs/system/SYSTEM_TRACE_CONTEXT_MATRIX.md).

## Required propagation (mutating cross-service calls)

| Header | Constant source | Required |
| --- | --- | --- |
| `traceparent` | `OntogonyEventHeaders.TraceParent` | When W3C context present |
| `X-Ontogony-Trace-Id` | `OntogonyEventHeaders.TraceId` | Yes |
| `X-Ontogony-Correlation-Id` | `OntogonyIntegrationHeaders.CorrelationId` | When business correlation exists |
| `X-Ontogony-Idempotency-Key` | `OntogonyIntegrationHeaders.IdempotencyKey` | Yes on mutating calls with side effects |
| `X-Ontogony-Actor-Id` | `OntogonyIntegrationHeaders.ActorId` | Kanon semantic/policy calls only |
| `X-Ontogony-Actor-Type` | `OntogonyIntegrationHeaders.ActorType` | Kanon semantic/policy calls only |
| `X-Ontogony-Actor-Roles` | `OntogonyIntegrationHeaders.ActorRoles` | Kanon semantic/policy calls only |
| `X-Allagma-Run-Id` | `AllagmaIntegrationHeaders.RunId` | Allagma-orchestrated runtime calls |

## Privacy rule

Actor identity and roles are semantic/policy context for **Kanon** and **Metabole**. Allagma does **not** send actor headers to **Conexus** on model completion (`AllagmaRunContextPropagation.ForConexus`).

## Actor context (alpha / dev modes)

| Mode | Where configured | Behavior |
| --- | --- | --- |
| **Header-trusted (alpha default)** | Kanon `Kanon:Security:ActorContextMode` = `header` | Allagma sends `X-Ontogony-Actor-*` on Kanon/Metabole calls; Kanon trusts inbound headers for policy gates |
| **Service token only** | Allagma `Allagma:Api:Auth:ServiceToken` | Client → Allagma auth; actor headers originate from run start request body |
| **Off / local** | Aisthesis `Aisthesis:Auth:Mode` = `off` | Producer auth disabled; trace headers still recommended on evidence POST |

Production OIDC/JWT federation is **out of scope** for this contract (RC readiness packages).

## Idempotency derivation

Do not blind-forward the root client key. Derive scoped downstream keys (see Allagma matrix). Examples:

```text
allagma:{runId}:plan
allagma:{runId}:model:{purpose}
allagma:{runId}:metabole:inspect:{metaboleRunId}
```

## Client obligations

| Client | Must forward |
| --- | --- |
| Allagma outbound | Trace, correlation, derived idempotency, run id; actor to Kanon/Metabole only |
| `Kanon.Client` | Trace + correlation + idempotency |
| `Conexus.Client` | Trace + correlation + project context |
| `Metabole.Client` | Trace + correlation (via `AddOntogonyIntegrationHttpClient`) |
| Aisthesis producers | Trace on evidence envelopes; separate producer token auth |

## Verification

| Proof | Location |
| --- | --- |
| Platform constants | `Ontogony.Http.OntogonyIntegrationHeaders`, `Ontogony.Contracts.Events.OntogonyEventHeaders` |
| Matrix ↔ constants | `CrossRepoIdentityCorrelation001ConformanceTests` |
| Allagma outbound | `AllagmaOutboundPropagationConformanceTests` |
| Live correlation chain | `scripts/lib/system-cohesion-e2e.ps1` scenario `correlation_chain` |
| Validator | `ontogony-platform/scripts/validate-header-propagation-contract.ps1` |

## Schema

[`docs/schemas/ontogony-context-propagation-v1.schema.json`](../schemas/ontogony-context-propagation-v1.schema.json)

## Related

- [`HEADER_PROPAGATION_CONTRACT.md`](./HEADER_PROPAGATION_CONTRACT.md)
- [`CROSS_SYSTEM_IDENTIFIERS.md`](../../allagma-dotnet/docs/contracts/CROSS_SYSTEM_IDENTIFIERS.md)
- Kanon: `kanon-dotnet/docs/integrations/IDEMPOTENCY_AND_TRACE_HEADERS.md`
