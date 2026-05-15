# Platform cleanup and tightening — status

**Repository:** `ontogony-platform`  
**Goal:** Keep Ontogony.Platform a **mechanical substrate** only (no Kanon, Allagma, or Conexus product semantics in shipping code or consumer blueprints).  
**Last review:** 2026-05-15 (see `CHANGELOG.md` Unreleased for doc/example edits tied to this pass).

---

## Baseline inspection (summary)

| Area | Finding |
| --- | --- |
| **README / package inventory** | README claims **23** shipping `Ontogony.*` libraries; `src/` contains **23** projects — **aligned**. |
| **`docs/consumer-blueprints/`** | Conexus (active) and Allagma (blueprint) docs present. Allagma readiness pipeline section tightened to **mechanical** steps only; boundary table wording reduced product coupling in one pass. |
| **`docs/packages/`** | Twenty-three package markdown files matching shipping set; `index.md` catalogs them. |
| **`docs/adoption/`** | Mechanical adoption guides; occasional product names as **examples** of consumers (expected). |
| **`docs/architecture/`** | `package-levels.md`, durability boundaries, index — present. |
| **`examples/`** | `AllagmaDotNetSkeleton`, `ConexusDotNetSkeleton`, `ConexusDotNetPackageSmoke`, minimal APIs — compile-only or smoke; **no** MAF or provider SDK references in skeleton csproj. |
| **`src/Ontogony.*`** | No `human_gate`, `tool_intent`, MAF, or provider SDK package references found in a targeted scan; integration mechanics (`OntogonyIntegrationContext`, etc.) are appropriately generic. |
| **`tests/Ontogony.*`** | Architecture tests **intentionally** mention forbidden package fragments (`Anthropic`, `Azure.AI.OpenAI`, …) as **negative** fixtures — not a product dependency. |
| **`scripts/`** | Consumer baseline validators align readiness docs with skeleton csproj; CI runs Conexus + Allagma validators. |
| **`*.lscache`** | Listed in **`.gitignore`**; not a hygiene issue. |
| **Stale execution-framework naming** | No `execution-framework` / `ExecutionFramework` string hits in active `src` / consumer blueprint paths scanned. Historical **Agentor** naming remains in some adoption / start-here docs as **legacy context** (acceptable; not the old framework monolith name). |
| **Docs vs shipped features** | No finding in this pass that `docs/packages/*` promises packages missing from `src/`. |

---

## What was cleaned in this pass

- **`docs/consumer-blueprints/allagma-dotnet-platform-readiness.md`** — Request pipeline is **mechanical** (tracing, logging, actor, idempotency, outbound HTTP, optional AI telemetry, journal, artifacts, persistence). Avoids run IDs, semantic-plan steps, and tool/human-gate flows. `Ontogony.AI.Contracts` row no longer names a specific consumer stack for telemetry. Boundary table rows for human-gate / planning sharpened to “product repos, not Ontogony” without prescribing cross-product semantics.
- **`examples/AllagmaDotNetSkeleton/Program.cs`** — Named HTTP clients `partner-alpha` / `partner-beta`; integration metric operation `SmokeOperation` (replacing product-specific labels).
- **`CHANGELOG.md`** — Unreleased notes for the above.
- **`README.md`** — Documentation map already includes [`docs/backlog/PLATFORM_CLEANUP_TIGHTENING_STATUS.md`](PLATFORM_CLEANUP_TIGHTENING_STATUS.md) (periodic audit entry point).
- **`docs/backlog/PLATFORM_EXTRACTION_CANDIDATES.md`** — New planning doc for safe mechanical extractions (implementation **not** done here except documentation).

---

## What remains product-local (non-negotiable)

- Kanon contracts, decision records, ontology/planning HTTP **DTOs** and clients (`kanon-dotnet`).
- Allagma run lifecycle, tool-intent evaluation flow, MAF adapter (`allagma-dotnet`).
- Conexus gateway routing, provider SDK usage, OpenAI-compatible shapes (`conexus-dotnet`).
- Cross-system event **vocabularies** and identifier-ownership **governance** documents.

---

## What is proposed for extraction

See [`PLATFORM_EXTRACTION_CANDIDATES.md`](PLATFORM_EXTRACTION_CANDIDATES.md). **Highest-confidence tiny candidate:** unify `KanonClientCallOptions` and `ConexusClientCallOptions` into a single `Ontogony.Http` type (same `IntegrationOutboundState` push logic). Additional candidates (generic error reader, durable Postgres ledgers, artifact durability) are **deferred** until duplication and schema stability are proven.

---

## What was explicitly rejected (for platform inclusion)

- Microsoft Agent Framework packages and runtime abstractions.
- Provider SDK packages (OpenAI, Anthropic, Azure OpenAI, Google AI, etc.).
- Human-gate, tool-intent, or semantic-plan **rules or DTOs**.
- Conexus-specific routing or Kanon-specific meaning models in platform source.

---

## Build / test validation

Commands (run from repo root after changes):

```powershell
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln -c Release --no-restore
dotnet test Ontogony.Platform.sln -c Release --no-build
```

Consumer skeleton / baseline scripts (also in CI):

```powershell
./scripts/validate-allagma-consumer-baseline-alignment.ps1
./scripts/validate-conexus-consumer-baseline-alignment.ps1
```

**Results (2026-05-15, local):**

- `dotnet restore Ontogony.Platform.sln` — **succeeded**
- `dotnet build Ontogony.Platform.sln -c Release --no-restore` — **succeeded** (0 warnings / 0 errors)
- `dotnet test Ontogony.Platform.sln -c Release --no-build` — **succeeded** (all test assemblies passed; e.g. 273 tests in `Ontogony.Infrastructure.Tests`, 30 in `Ontogony.Http.Tests`, 23 in `Ontogony.PublicApi.Tests`, plus other projects)
- `./scripts/validate-allagma-consumer-baseline-alignment.ps1` — **succeeded** (17 packages aligned; AllagmaDotNetSkeleton Release build OK)
- `./scripts/validate-conexus-consumer-baseline-alignment.ps1` — **succeeded** (16 packages aligned)

_Note: On Windows, run the `.ps1` validators via `powershell -File …` if `pwsh` is not on PATH._

---

## Next recommended platform PR

1. **Maintenance:** **PLAT-NP-008** / in-memory registration warning coverage gaps, or consumer-driven HTTP resilience doc/test hardening per `docs/packages/Ontogony.Http.md`. (**`IntegrationClientCallOptions`** extraction is complete as of 2026-05-15.)
