# Scope and boundary — ONTOGONY-BACKEND-COORDINATION-002

## What this sprint owns

A **coordinated proof layer** across six backend repos:

1. Navigable, non-contradictory documentation with shared index conventions.
2. Machine-readable compatibility manifest aligned with runtime lock.
3. Uniform cross-service error envelope (mechanical, not semantic).
4. End-to-end trace, actor, and idempotency header propagation.
5. Config-driven Conexus model alias discipline for Allagma.
6. Five-service governed E2E smoke with evidence artifacts.
7. Aisthesis reconstructability spine with **live** (not fixture-only) proof.
8. Metabole data-spine hardening with Kanon handoff clarity.

## What this sprint does not own

| Forbidden | Reason |
| --- | --- |
| Semantic policy in Conexus/Allagma | Kanon authority |
| Provider routing in Kanon/Allagma | Conexus authority |
| Workflow orchestration in Kanon | Allagma authority |
| Ontology truth in Metabole | Kanon authority |
| Real tool execution | Trust model not ready |
| Kanon v1 API graduation | Exit gates not met |
| Production IAM / multi-region | Alpha scope |

## Repo responsibilities during sprint

| Repo | Must do | Must not do |
| --- | --- | --- |
| **Platform** | Publish/adopt shared contracts, schemas, conformance kits | Add product semantics |
| **Conexus** | Alias registry manifest, error middleware alignment, propagation on outbound | Add ontology or execution |
| **Kanon** | Error shape alignment, propagation on Client, doc truth | Add provider SDKs |
| **Allagma** | Refresh system matrices, E2E orchestration, alias-only Conexus calls | Pick providers/models directly |
| **Metabole** | Data-spine certification, SLOD↔Kanon handoff docs | Canonize facts locally |
| **Aisthesis** | Live reconstructability PASS, producer contract checks | Own semantic decisions |

## Slice boundary guards

### BACKEND-REPO-DOCS-ORDER-002

- Docs only. No API or behavior changes unless fixing broken links references stale routes.

### SYSTEM-COMPATIBILITY-MATRIX-001

- Refresh matrices and lock; no new features. Bump lock only with evidence.

### SHARED-ERROR-CONTRACT-001

- Mechanical envelope (`code`, `message`, `system`, `traceId`, optional `detail`). Kanon typed validation DTOs remain **by design**.

### CROSS-REPO-IDENTITY-CORRELATION-001

- Headers and middleware only. No new auth product (OIDC production) in this slice.

### ALLAGMA-CONEXUS-MODEL-ALIAS-001

- Configuration and manifest; Conexus still owns routing. Remove test-only hard-coded model IDs where they imply product defaults.

### BACKEND-SYSTEM-E2E-001

- Governed fake/real stack smoke; real tools remain blocked.

### AISTHESIS-RECONSTRUCTABILITY-SPINE-001

- Evidence and certification; may subsume `AISTHESIS-LIVE-FIVE-SERVICE-PASS-009` if still active.

### METABOLE-DATA-SPINE-HARDENING-001

- Pipeline/SLOD spine; no speculative connectors.

## Conflict resolution

If a slice conflicts with an active repo intake package:

1. Prefer **merging** into the slice spec (same goal).
2. Archive the smaller package to `_consumed` with `CONSUMED.md` noting merge.
3. Never leave two active packages claiming the same acceptance gate.

## Promotion rule

Durable contracts promote to:

- `ontogony-platform/docs/contracts/` — mechanical cross-service contracts
- `allagma-dotnet/docs/system/` — runtime matrices
- `<product-repo>/docs/integrations/` — integration alignment docs

Intake package archives when promoted.
