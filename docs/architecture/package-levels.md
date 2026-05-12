# Package levels and dependency rules

This document makes the Ontogony.Platform **package graph intentional**: which packages exist, how they group, which references are allowed, and which edges are forbidden.

Machine checks live in [`../../scripts/validate-package-levels.ps1`](../../scripts/validate-package-levels.ps1) and run in CI.

---

## Level 0 — Foundation

**Packages:** `Ontogony.Primitives`, `Ontogony.Hashing`, `Ontogony.Configuration`

**Purpose:** time and IDs, canonical JSON and hashing, startup validation.

**Notes:** `Ontogony.Hashing` references `Ontogony.Contracts` for shared envelope/hash mechanics; the four-level map is a **consumer mental model**, not a strict topological sort. The **allowed reference matrix** below is authoritative.

---

## Level 1 — Service mechanics

**Packages:** `Ontogony.Hosting`, `Ontogony.Observability`, `Ontogony.Errors`, `Ontogony.Http`, `Ontogony.Security`

**Purpose:** ASP.NET defaults, trace/correlation, error shape, resilient outbound HTTP, service identity and actor context.

---

## Level 2 — Event and consistency mechanics

**Packages:** `Ontogony.Contracts`, `Ontogony.Messaging`, `Ontogony.Idempotency`, `Ontogony.Persistence`, `Ontogony.Persistence.Postgres`, `Ontogony.ProtocolIngress`

**Purpose:** envelopes and events, idempotency, outbox and processed-message contracts, PostgreSQL outbox, protocol normalization.

---

## Level 3 — AI runtime mechanics

**Packages:** `Ontogony.AI.Contracts`, `Ontogony.Artifacts`, `Ontogony.Execution`

**Purpose:** LLM request/response telemetry, usage/cost/error records, large payload references, execution journal facts and checkpoints — **without** orchestration or provider policy.

**Aggregate (not a runtime tier):** `Ontogony.Testing` pulls together multiple packages for test fixtures.

---

## Forbidden dependency rules

1. **Lower documentation levels must not pull in ad-hoc “higher” packages** beyond what the allowed matrix encodes. CI compares `ProjectReference` edges to the golden map in `validate-package-levels.ps1`.
2. **`Ontogony.Execution` must not reference `Ontogony.Artifacts` directly.** Execution uses opaque artifact identifiers (for example `PayloadArtifactId`); artifact storage semantics stay in `Ontogony.Artifacts` and composing services.
3. **AI runtime packages must not depend on product repos** (no references to Athanor/Agentor/Conexus codebases). That is a process rule; repo layout enforces absence of those projects.
4. **`Ontogony.AI.Contracts` stays provider-neutral** (no vendor-specific SDK types in public contracts). Additional scanning lives in `scripts/validate-ai-runtime-boundaries.ps1`.
5. **Shipping libraries under `src/` must not reference `Ontogony.Testing`.** Only test projects consume `Ontogony.Testing`.

---

## Package dependency matrix

Rows depend on columns. **✓** means the row package may `ProjectReference` the column package (golden set in `validate-package-levels.ps1`). Column order is alphabetical by package id.

|  | AI | Art | Cfg | Con | Err | Exe | Hash | Hst | Http | Idem | Msg | Obs | Per | Pg | Pri | Ing | Sec | Tst |
|--|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|:---:|
| **Ontogony.AI.Contracts** |  |  |  | ✓ |  |  |  |  |  |  |  |  |  |  |  |  |  |  |
| **Ontogony.Artifacts** |  |  |  | ✓ |  |  | ✓ |  |  |  |  |  |  |  | ✓ |  |  |  |
| **Ontogony.Configuration** |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |
| **Ontogony.Contracts** |  |  |  |  |  |  |  |  |  |  |  |  |  |  | ✓ |  |  |  |
| **Ontogony.Errors** |  |  |  |  |  |  |  |  |  |  |  | ✓ |  |  |  |  |  |  |
| **Ontogony.Execution** |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |
| **Ontogony.Hashing** |  |  |  | ✓ |  |  |  |  |  |  |  |  |  |  |  |  |  |  |
| **Ontogony.Hosting** |  |  |  |  | ✓ |  |  |  |  |  |  | ✓ |  |  |  |  | ✓ |  |
| **Ontogony.Http** |  |  |  |  |  |  |  |  |  |  |  | ✓ |  |  | ✓ |  |  |  |
| **Ontogony.Idempotency** |  |  |  |  |  |  | ✓ |  |  |  |  |  |  |  |  |  |  |  |
| **Ontogony.Messaging** |  |  |  | ✓ |  |  | ✓ |  |  |  |  | ✓ |  |  |  |  |  |  |
| **Ontogony.Observability** |  |  |  | ✓ |  |  |  |  |  |  |  |  |  |  |  |  |  |  |
| **Ontogony.Persistence** |  |  |  |  |  |  |  |  |  |  |  |  |  |  | ✓ |  |  |  |
| **Ontogony.Persistence.Postgres** |  |  |  |  |  |  |  |  |  |  |  |  | ✓ |  | ✓ |  |  |  |
| **Ontogony.Primitives** |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |  |
| **Ontogony.ProtocolIngress** |  |  |  | ✓ |  |  | ✓ |  |  |  |  |  |  |  | ✓ |  |  |  |
| **Ontogony.Security** |  |  |  | ✓ |  |  |  |  |  |  |  |  |  |  | ✓ |  |  |  |
| **Ontogony.Testing** |  |  |  | ✓ | ✓ |  | ✓ |  | ✓ |  | ✓ | ✓ | ✓ |  | ✓ |  | ✓ |  |

**Header abbreviations:** AI = AI.Contracts, Art = Artifacts, Cfg = Configuration, Con = Contracts, Err = Errors, Exe = Execution, Hash = Hashing, Hst = Hosting, Http = Http, Idem = Idempotency, Msg = Messaging, Obs = Observability, Per = Persistence, Pg = Persistence.Postgres, Pri = Primitives, Ing = ProtocolIngress, Sec = Security, Tst = Testing.

The matrix matches `validate-package-levels.ps1`. When you add or change a `ProjectReference`, update **both** the script and this table.

---

## Related

- [`../00_START_HERE.md`](../00_START_HERE.md)
- [`../../README.md`](../../README.md)
- [`../../AGENTS.md`](../../AGENTS.md)
