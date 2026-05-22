# Phase 1 consumer compatibility (Ontogony.Platform)

**Package line:** `0.3.0-alpha.1`  
**Scope:** Full System Phase 1 — four-repo governed runtime baseline (Platform, Conexus, Kanon, Allagma).  
**Rule:** Platform supplies **mechanics only**. Product meaning stays in the consumer repos.

```text
Ontogony.Platform = shared mechanics
Conexus.NET       = model gateway
Kanon.NET         = semantic authority
Allagma.NET       = governed execution
```

Historical name **Agentor** is replaced by **Allagma**; platform docs reference Agentor only as legacy header/donor context.

## Consumer summary

| Consumer | Role | Platform consumption mode | Platform-side proof |
| --- | --- | --- | --- |
| **Conexus.NET** | LLM/model gateway | Sibling `ProjectReference` (default) or `UseOntogonyPackages=true` | [`validate-conexus-consumer-baseline-alignment.ps1`](../../scripts/validate-conexus-consumer-baseline-alignment.ps1); [`CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md`](../consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md); `examples/ConexusDotNetPackageSmoke` |
| **Allagma.NET** | Governed execution | Sibling or package mode (see Allagma repo) | [`validate-allagma-consumer-baseline-alignment.ps1`](../../scripts/validate-allagma-consumer-baseline-alignment.ps1); [`ALLAGMA_ONTOGONY_PACKAGE_MODE_CONTRACT.md`](../consumer-blueprints/ALLAGMA_ONTOGONY_PACKAGE_MODE_CONTRACT.md); `examples/AllagmaDotNetSkeleton` |
| **Kanon.NET** | Semantic authority | Sibling `ProjectReference` (default) or `UseOntogonyPackages=true` | Kanon CI package-mode job; package set in Kanon `eng/Ontogony.References.props`; platform doc guard [`kanon-ontogony-package-union.txt`](./kanon-ontogony-package-union.txt) + [`validate-kanon-ontogony-package-union.ps1`](../../scripts/validate-kanon-ontogony-package-union.ps1) (optional drift check when `../kanon-dotnet` or `KANON_SIBLING_ROOT` is present) |

## How each consumer should consume Platform

### Conexus.NET

- **Readiness:** [`conexus-dotnet-platform-readiness.md`](../consumer-blueprints/conexus-dotnet-platform-readiness.md)
- **Package mode:** set `UseOntogonyPackages=true`, pin `OntogonyPackageVersion` to the platform line, restore from a feed that does not collide with a sibling `../ontogony-platform` checkout in package-mode CI.
- **Do not** expect Ontogony to own routing, pricing, provider adapters, or gateway quota policy.

### Allagma.NET

- **Readiness:** [`allagma-dotnet-platform-readiness.md`](../consumer-blueprints/allagma-dotnet-platform-readiness.md)
- **Mechanical spine:** tracing, logging, redaction, errors, HTTP to Kanon/Conexus, security/actor, idempotency, execution journal, artifacts — **not** run/plan/tool/human-gate semantics.
- **Do not** reference Kanon or Conexus **implementation** assemblies from Platform; call over HTTP only.

### Kanon.NET

- **Package set (by project):** defined in `kanon-dotnet` → `eng/Ontogony.References.props`. The sorted **union** of all `Ontogony.*` references is mirrored in [`kanon-ontogony-package-union.txt`](./kanon-ontogony-package-union.txt) and validated in platform CI by [`validate-kanon-ontogony-package-union.ps1`](../../scripts/validate-kanon-ontogony-package-union.ps1). When Kanon changes references, update the union file in the same PR (or follow-up) that touches `Ontogony.References.props`.
- **Package mode:** `dotnet restore/build/test` with `-p:UseOntogonyPackages=true` and aligned `OntogonyPackageVersion` (see Kanon `docs/reviews/KANON_PHASE_3_CLOSEOUT_REPORT.md`).
- **Do not** expect Ontogony to own ontology, canonization, semantic query plans, or action-policy meaning.

## Shared mechanical contracts (all Phase 1 consumers)

| Concern | Platform packages | Consumer doc |
| --- | --- | --- |
| Trace + correlation headers | `Ontogony.Observability` | [`header-compatibility-matrix.md`](../contracts/header-compatibility-matrix.md) |
| API error JSON (`code`, `message`, `traceId`, …) | `Ontogony.Errors` | [`error-correlation-mechanics.md`](../examples/error-correlation-mechanics.md) |
| Outbound HTTP + header propagation | `Ontogony.Http`, `Ontogony.Security` | [`service-to-service-integration.md`](../adoption/service-to-service-integration.md) |
| Idempotency keys | `Ontogony.Idempotency` | package READMEs under `docs/packages/` |

## Upgrade and breaking-change policy (alpha)

1. Read [`CHANGELOG.md`](../../CHANGELOG.md) and [`docs/migrations/`](../migrations/) before bumping `OntogonyPackageVersion` or syncing sibling platform HEAD.
2. Run the consumer repo’s full test suite (including package-mode CI equivalent when applicable).
3. Platform PRs that change public API must follow [`PUBLIC_API_COMPATIBILITY.md`](../planning/robustness/PUBLIC_API_COMPATIBILITY.md) and [`public-api-review.md`](../public-api-review.md).
4. Coordinate **cross-repo** lock updates (`ontogony-runtime.lock.json` in Allagma) when Phase 1 baseline heads move — that is a system PR, not a silent package bump.

## Non-goals

- No Kanon/Conexus/Allagma domain types or policies in Platform.
- No new Platform packages for Phase 1 governance (docs + existing validation scripts only).
- No compile-only Kanon skeleton in Platform (Kanon proves build compatibility in `kanon-dotnet` CI; Platform holds the Ontogony package **union** doc guard only).
