# Package levels and dependency rules

This document makes the Ontogony.Platform **package graph intentional**: which packages exist, how they group, which references are allowed, and which edges are forbidden.

Machine checks live in [`../../scripts/validate-package-levels.ps1`](../../scripts/validate-package-levels.ps1) and run in CI.

**Authoritative edges:** the **allowed `ProjectReference` matrix** below matches the golden map in `validate-package-levels.ps1`. Documentation **levels** are a consumer mental model (grouping related concerns); they are **not** a strict topological sort — for example `Ontogony.Hashing` references `Ontogony.Contracts` even though both sit in “foundation / shared representation” tiers.

---

## Level 0 — Pure foundation

**Packages:** `Ontogony.Primitives`, `Ontogony.Configuration`

**Purpose:** time and IDs; startup and options validation helpers.

---

## Level 0.5 — Shared representation

**Packages:** `Ontogony.Contracts`, `Ontogony.Hashing`

**Purpose:** protocol-neutral envelopes, headers, and protocol constants (`Ontogony.Contracts`); deterministic canonical JSON and payload hashing (`Ontogony.Hashing`, which references `Ontogony.Contracts` for envelope/hash mechanics).

Many higher-level packages depend on one or both of these; that is why they are modeled as **shared representation**, not “event tier” packages.

---

## Level 1 — Service mechanics

**Packages:** `Ontogony.Hosting`, `Ontogony.Observability`, `Ontogony.Errors`, `Ontogony.Http`, `Ontogony.Security`, `Ontogony.Logging`, `Ontogony.Redaction`, `Ontogony.Secrets`, optional `Ontogony.Secrets.AzureKeyVault`

**Purpose:** ASP.NET defaults, trace/correlation, error shape, resilient outbound HTTP, service identity and actor context, structured logging fields and scopes, deterministic redaction, and secret-reference mechanics. Cloud vault resolution is an optional adapter (`Ontogony.Secrets.AzureKeyVault` → `Ontogony.Secrets` only).

`Ontogony.Security` references `Ontogony.Http` for outbound actor/tenant propagation (`CurrentActorOutboundPropagator`, integration header helpers) — not for product routing semantics.

---

## Level 2 — Event, consistency, persistence mechanics

**Packages:** `Ontogony.Messaging`, `Ontogony.Idempotency`, `Ontogony.Persistence`, `Ontogony.Persistence.Postgres`, `Ontogony.ProtocolIngress`, `Ontogony.Quotas`

**Purpose:** in-process publish/dispatch, idempotency ledger, outbox and processed-message contracts, PostgreSQL outbox provider, protocol normalization into envelopes, and mechanical quota windows/decisions (no product plan tiers).

---

## Level 3 — AI runtime mechanics

**Packages:** `Ontogony.AI.Contracts`, `Ontogony.Artifacts`, `Ontogony.Evaluation.Contracts`, `Ontogony.Execution`, `Ontogony.Replay.Contracts`, `Ontogony.Topology.Contracts`

**Purpose:** LLM request/response telemetry, usage/cost/error records, large payload references, neutral evaluation run/score/baseline DTOs, neutral task classification and topology selection DTOs, execution journal facts and checkpoints, and replay bundle contracts — **without** orchestration, provider policy, eval harnesses, topology planners, or a replay engine.

---

## Cross-cutting gates (CI / dev tooling)

**Package:** `Ontogony.SystemCompatibility`

**Purpose:** Cross-repo mechanical compatibility validators (runtime lock, route/client drift, error envelopes, header propagation, six-repo gate). References `Ontogony.Contracts`, `Ontogony.Errors`, `Ontogony.Hashing`, and `Ontogony.Http` only. Consumed by `Ontogony.SystemCompatibility.Tests` and gate scripts — not a shipping runtime dependency for product services.

**Golden `ProjectReference` set:** `Contracts`, `Errors`, `Hashing`, `Http` (see `validate-package-levels.ps1`).

---

## Aggregate (not a runtime tier)

**Package:** `Ontogony.Testing` — pulls together multiple packages for test fixtures and conformance kits. Shipping libraries must not reference it.

---

## Forbidden dependency rules

1. **No edges outside the matrix.** CI compares every Ontogony `ProjectReference` under `src/` to the golden map in `validate-package-levels.ps1`. When you add or change an edge, update **both** the script and the matrix in this document.
2. **`Ontogony.Execution` must not reference `Ontogony.Artifacts` directly.** Execution uses opaque artifact identifiers (for example `PayloadArtifactId`); artifact storage semantics stay in `Ontogony.Artifacts` and composing services.
3. **AI runtime packages must not depend on product repos** (no references to Athanor/Agentor/Conexus codebases). That is a process rule; repo layout enforces absence of those projects.
4. **`Ontogony.AI.Contracts` stays provider-neutral** (no vendor-specific SDK types in public contracts). Additional scanning lives in `scripts/validate-ai-runtime-boundaries.ps1`.
5. **Shipping libraries under `src/` must not reference `Ontogony.Testing`.** Only test projects consume `Ontogony.Testing`.

---

## Package dependency matrix

Rows depend on columns. **✓** means the row package may `ProjectReference` the column package (golden set in `validate-package-levels.ps1`). Column order is alphabetical by package id.

|  | AI | Art | Cfg | Con | Err | Eva | Exe | Hash | Hst | Http | Idem | Log | Msg | Obs | Per | Pg | Pri | Ing | Quo | Rdc | Rpl | Scr | Sec | Top | Tst |
|--|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|
| **Ontogony.AI.Contracts** |   |   |   | ✓ |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |
| **Ontogony.Artifacts** |   |   |   | ✓ |   |   | ✓ |   |   |   |   |   |   |   |   | ✓ |   |   |   |   |   |   |   |   |
| **Ontogony.Configuration** |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |
| **Ontogony.Contracts** |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   | ✓ |   |   |   |   |   |   |   |   |
| **Ontogony.Errors** |   |   |   |   |   |   |   |   |   |   |   |   | ✓ |   |   |   |   |   |   |   |   |   |   |   |   |
| **Ontogony.Evaluation.Contracts** |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |
| **Ontogony.Execution** |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |
| **Ontogony.Hashing** |   |   |   | ✓ |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |
| **Ontogony.Hosting** |   |   |   |   | ✓ |   |   |   |   |   |   |   | ✓ |   |   |   |   |   |   |   |   | ✓ |   |   |
| **Ontogony.Http** |   |   |   |   |   |   |   |   |   |   |   |   | ✓ |   |   | ✓ |   |   |   |   |   |   |   |   |
| **Ontogony.Idempotency** |   |   |   |   |   |   | ✓ |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |
| **Ontogony.Logging** |   |   |   |   |   |   |   |   |   |   |   |   | ✓ |   |   |   |   |   | ✓ |   |   |   |   |   |
| **Ontogony.Messaging** |   |   |   | ✓ |   |   | ✓ |   |   |   |   |   | ✓ |   |   |   |   |   |   |   |   |   |   |   |
| **Ontogony.Observability** |   |   |   | ✓ |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |
| **Ontogony.Persistence** |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   | ✓ |   |   |   |   |   |   |   |   |
| **Ontogony.Persistence.Postgres** |   |   |   |   |   |   |   |   |   |   |   |   |   | ✓ |   | ✓ |   |   |   |   |   |   |   |   |
| **Ontogony.Primitives** |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |
| **Ontogony.ProtocolIngress** |   |   |   | ✓ |   |   | ✓ |   |   |   |   |   |   |   |   | ✓ |   |   |   |   |   |   |   |   |
| **Ontogony.Quotas** |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   | ✓ |   |   |   |   |   |   |   |   |
| **Ontogony.Redaction** |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |
| **Ontogony.Replay.Contracts** |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |
| **Ontogony.Secrets** |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   | ✓ |   |   |   |   |   |
| **Ontogony.Secrets.AzureKeyVault** |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   | ✓ |   |   |   |   |
| **Ontogony.Security** |   |   |   | ✓ |   |   |   |   | ✓ |   |   |   |   |   |   | ✓ |   |   |   |   |   |   |   |   |
| **Ontogony.SystemCompatibility** |   |   |   | ✓ | ✓ |   |   | ✓ |   | ✓ |   |   |   |   |   |   |   |   |   |   |   |   |   |   |
| **Ontogony.Topology.Contracts** |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |   |
| **Ontogony.Testing** |   |   |   | ✓ | ✓ |   | ✓ |   | ✓ |   |   | ✓ | ✓ | ✓ |   | ✓ |   |   |   |   |   | ✓ |   |   |

**Header abbreviations:** AI = AI.Contracts, Art = Artifacts, Cfg = Configuration, Con = Contracts, Err = Errors, Eva = Evaluation.Contracts, Exe = Execution, Hash = Hashing, Hst = Hosting, Http = Http, Idem = Idempotency, Log = Logging, Msg = Messaging, Obs = Observability, Per = Persistence, Pg = Persistence.Postgres, Pri = Primitives, Ing = ProtocolIngress, Quo = Quotas, Rdc = Redaction, Rpl = Replay.Contracts, Scr = Secrets, Sec = Security, Top = Topology.Contracts, Tst = Testing.

The matrix matches `validate-package-levels.ps1`. When you add or change a `ProjectReference`, update **both** the script and this table.

---

## Related

- [`../governance/PACKAGE_LEVEL_GOVERNANCE.md`](../governance/PACKAGE_LEVEL_GOVERNANCE.md) — change workflow, forbidden edges, CI evidence
- [`../CURRENT_STATE.md`](../CURRENT_STATE.md)
- [`../../README.md`](../../README.md)
- [`../../AGENTS.md`](../../AGENTS.md)
