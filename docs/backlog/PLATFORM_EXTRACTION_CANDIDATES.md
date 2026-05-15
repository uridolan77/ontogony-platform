# Platform extraction candidates (mechanical only)

**Update 2026-05-15:** §1 (**`Ontogony.Http` / `IntegrationClientCallOptions`**) is **implemented** in platform + Kanon/Conexus consumers; downstream extraction decision record: [`allagma-dotnet` `docs/backlog/PLATFORM_EXTRACTION_DECISION_RECORD.md`](../../../allagma-dotnet/docs/backlog/PLATFORM_EXTRACTION_DECISION_RECORD.md).

This document records **repeated mechanics** observed across consumer repos that **could** move into Ontogony.Platform. Open items beyond §1 remain planning-only unless another decision record adopts them.

**Rule:** extractions must not import Kanon meaning, Allagma run or tool-intent protocols, Conexus routing DTOs, Microsoft Agent Framework, or provider SDKs.

---

## 1. Per-integration `HttpClient` call options → `OntogonyIntegrationContext` — **DONE**

Canonical type: **`IntegrationClientCallOptions`** in `Ontogony.Http`, with **`PushScope()`** → **`OntogonyIntegrationContext`**. Removed duplicate **`KanonClientCallOptions`** / **`ConexusClientCallOptions`** records.

### ~~Current duplicate locations~~ (historical)

| Location | Type |
| --- | --- |
| `kanon-dotnet/src/Kanon.Client/KanonClientCallOptions.cs` | `record` with `PushIntegrationScope()` → `IntegrationOutboundState` |
| `conexus-dotnet/src/Conexus.Client/ConexusClientCallOptions.cs` | Identical shape and implementation |

### Why it is mechanical

The type is only idempotency key plus actor/tenant/workspace snapshots pushed into `Ontogony.Http`’s async-local integration scope before an HTTP call. No domain verbs, plans, or provider enums.

### Why it is not product semantics

Product repos keep their HTTP paths, DTOs, and business rules. This record is a header-propagation helper already aligned with `IntegrationHeadersDelegatingHandler`.

### Proposed package

`Ontogony.Http` (same assembly as `OntogonyIntegrationContext` / `IntegrationOutboundState`).

### Proposed API surface (illustrative)

- **Name:** `IntegrationClientCallOptions` (or `OutboundIntegrationCallOptions`) — a `sealed record` mirroring today’s optional parameters.
- **Method:** `public IDisposable PushScope()` delegating to `OntogonyIntegrationContext.Push(new IntegrationOutboundState(...))`.
- **Migration:** deprecate `KanonClientCallOptions` / `ConexusClientCallOptions` in favor of the platform type, or keep thin type aliases in product clients for a release if needed.

### Migration plan per product repo — **completed** (2026-05-15)

| Repo | Status |
| --- | --- |
| **ontogony-platform** | Shipped **`IntegrationClientCallOptions`** + **`Ontogony.Http.Tests`** + public API snapshot update. |
| **kanon-dotnet** | **`KanonClient`** uses platform type; **`GlobalUsings`** for `Ontogony.Http`; tests updated. |
| **conexus-dotnet** | **`Conexus.Client`** uses platform type; **`GlobalUsings`**; tests updated. |
| **allagma-dotnet** | **`Allagma.Infrastructure`** call sites + tests use **`IntegrationClientCallOptions`**. |

### Tests required

- Unit tests in `Ontogony.Http` verifying `PushScope()` sets the same header-relevant state as today’s product records.
- Existing `Kanon.Tests` / Conexus tests adjusted or duplicated minimally to prove behavior unchanged.

### Risks

- **Naming / binary compatibility:** public API change in Kanon.Client and Conexus.Client SemVer surfaces.
- **Wrong abstraction level:** if future clients need unrelated per-call options, avoid bloating the shared record — keep it strictly “outbound integration identity + idempotency.”

**Sizing:** Small, localized change — acceptable as a single focused platform PR **if** consumers upgrade in lockstep or via deprecation window.

---

## 2. Generic JSON API error envelope reader

### Current duplicate locations

_Not fully inventoried in this pass._ Candidate pattern: HTTP APIs returning `Ontogony.Errors`-shaped problem JSON; consumers parsing `ProblemDetails` or custom error contracts.

### Why it could be mechanical

A single `TryReadProblemDetailsAsync(HttpResponseMessage)` (or stream reader) that maps RFC7807-style payloads to stable platform error types avoids copy-paste across gateways.

### Why defer

Requires confirmation of **at least two** product repos using the **same** envelope and parser flow without product-specific error codes leaking into the parser.

### Proposed package

`Ontogony.Http` or `Ontogony.Errors` (prefer `Errors` if the type models domain-agnostic failure surfaces only).

### Proposed API surface

TBD after duplication scan (method signatures, optional `JsonSerializerOptions`, max body size).

### Migration plan

TBD per consumer once shapes align.

### Tests required

Golden JSON fixtures; regression for missing/invalid bodies.

### Risks

Coupling all consumers to one opinionated parse path; security (response body size, DoS).

**Status:** **Defer** until duplication is proven.

---

## 3. Postgres idempotency ledger / execution journal providers

### Current duplicate locations

Platform already ships `Ontogony.Persistence`, `Ontogony.Persistence.Postgres`, `Ontogony.Idempotency`, `Ontogony.Execution` with in-memory references. Durable Postgres **application** schemas for idempotency ledgers and execution journals may exist or emerge in product repos.

### Why it could be mechanical

Generic “append-only / upsert ledger” storage behind `IIdempotencyLedger` / `IExecutionJournal` interfaces is mechanics if SQL avoids product-specific columns.

### Why defer (per tightening pass)

Per guardrail: **do not** extract until **at least two** product repos have **concrete, stable** durable implementations to compare. Otherwise the platform risks encoding one product’s schema as “generic.”

### Proposed package

`Ontogony.Persistence.Postgres` (extend) or new `Ontogony.Persistence.Postgres.*` satellite if surface area grows.

### Risks

Schema drift, migration ownership, multi-tenant vs single-tenant assumptions.

**Status:** **Defer.**

---

## 4. Durable artifact store provider

### Current duplicate locations

Platform: `Ontogony.Artifacts` + in-memory store. Product durability (blob storage, encryption policy) remains product-local until checkpoint/replay payloads stabilize.

### Why defer

No second shared implementation with aligned lifetime and security model was confirmed in this pass.

**Status:** **Defer** until real checkpoint/replay payloads and two consumers align.

---

## 5. Execution journal recorder helper

### Current duplicate locations

_Not enumerated._ If multiple repos repeat “open scope → append lines → complete with correlation id,” a small helper could live in `Ontogony.Execution`.

### Why defer

Risk of smuggling Allagma run semantics into the helper. Revisit when a **mechanical** pattern is identical across repos (no step kinds, no tool vocabulary).

**Status:** **Defer.**

---

## 6. Service identity outbound signing handler

### Current state

Platform already exposes service identity and HTTP integration handlers; product-specific signing policy should stay minimal.

### Status

**Mostly platform already.** Re-open only if duplicate handler stacks appear in two repos beyond thin options wiring.

---

## 7. Architecture test helper improvements

### Current state

`Ontogony.Testing.Architecture` provides `ArchitectureScanTargets` and `ArchitectureReferenceAssertions`. Consumer skeletons compile-smoke these APIs.

### Possible extraction

Shared MSBuild glob patterns, standard forbidden-fragment lists per **mechanical** bans (provider SDKs, MAF), or adapters for `CentralPackageVersions` layout.

### Risks

Lists must remain **mechanical** (package names), not product DTO or namespace bans that drift into semantics.

**Status:** **Optional small follow-ups** as consumers report friction; no new surface in this cleanup PR.

---

## Explicitly rejected for platform (in this review)

| Topic | Reason |
| --- | --- |
| Kanon decision records, semantic plan contracts | Meaning authority — stays in Kanon. |
| Allagma run events, tool-intent protocol | Governed execution — stays in Allagma. |
| Conexus OpenAI-compatible DTOs, provider routing | Gateway product — stays in Conexus. |
| Microsoft Agent Framework adapters | Allagma-local adapter per boundary rules. |
| Cross-system identifier ownership / conformance rules | Documentation and governance live in product or org docs, not generic mechanics. |
