# ONTOGONY-SYSTEM-TEST-HARNESS-001

Comprehensive starter package for building a real automated test harness around the Ontogony multi-repo system.

The purpose is not to add a few smoke tests. The purpose is to create a **system test discipline** that can replace at least **50% of repeatable manual human regression testing** by covering service readiness, API contracts, business flows, cross-service orchestration, idempotency, persistence, restart safety, error contracts, observability, UI coverage, and negative/security paths.

## Included

```text
ONTOGONY-SYSTEM-TEST-HARNESS-001/
  docs/                       Strategy, matrices, quality gates, service-plan template
  manifests/                  Compatibility, routes, service registry, generated test catalog
  tests/dotnet/               xUnit-based API/system test skeleton
  tests/ui-playwright/        Playwright UI coverage skeleton
  load/k6/                    k6 smoke/load/resilience scripts
  scripts/                    PowerShell + bash runners
  test-data/                  Request payloads and fixtures
  evidence/                   Output folder for generated evidence bundles
  .github/workflows/          CI workflow starter
```

## Intended placement

Recommended options:

1. Create a separate repository: `ontogony-system-test-harness`.
2. Or place under `allagma-dotnet/tests/system-harness` while the system E2E suite is still Allagma-led.
3. Or keep it as a local overlay under `C:\Dev\ONTOGONY-SYSTEM-TEST-HARNESS-001` and run against sibling repos.

Recommended sibling layout:

```text
C:\Dev\
  ontogony-platform\
  conexus-dotnet\
  kanon-dotnet\
  allagma-dotnet\
  metabole-dotnet\
  aisthesis-dotnet\
  ontogony-frontend\
  ontogony-ui\
  ONTOGONY-SYSTEM-TEST-HARNESS-001\
```

## First usage

```powershell
cd C:\Dev\ONTOGONY-SYSTEM-TEST-HARNESS-001
copy .env.example .env
pwsh ./scripts/run-readiness.ps1
pwsh ./scripts/run-system-e2e.ps1
```

Linux/macOS:

```bash
cd ~/Dev/ONTOGONY-SYSTEM-TEST-HARNESS-001
cp .env.example .env
bash ./scripts/run-readiness.sh
bash ./scripts/run-system-e2e.sh
```

## What this harness should eventually automate

- All service `/health` and `/ready` checks.
- Auth and authorization behavior for each protected route.
- OpenAPI / route drift checks.
- Error envelope normalization checks.
- Allagma governed run lifecycle.
- Allagma → Kanon semantic planning/action evaluation/human gates.
- Allagma → Conexus model completion and fallback.
- Kanon → Conexus assistance flows with redaction and `draft_only` enforcement.
- Metabole state/evolution/data-spine flows.
- Aisthesis phenomenological memory flows and Aisthesis ↔ Kanon alignment.
- Cross-service trace/correlation propagation.
- Idempotency/retry/replay semantics.
- Persistence, restart, and migration survival.
- UI service coverage and console path coverage.
- Load, soak, and resilience smoke tests.

## Important implementation rule

The test harness should be **ruthless about boundaries**:

- Conexus owns model gateway behavior.
- Kanon owns semantic authority and policy.
- Allagma owns governed execution.
- Metabole owns transformation/evolution/data spine behavior.
- Aisthesis owns phenomenological memory/experience traces.
- Ontogony.Platform owns shared mechanics.
- Frontend repos must prove API coverage, not invent backend semantics.

## Current maturity

This is a starter package. It is intentionally broad and opinionated. The next step is to calibrate it service-by-service against actual current route names, OpenAPI specs, auth settings, and seed data.
