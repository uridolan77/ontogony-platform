# 06 — Repo touchpoints

## `ontogony-platform`

Primary owner for:

- Sprint plan and acceptance matrix reconciliation.
- `CURRENT_SYSTEM_BASELINE.md`.
- Runtime baseline index and cross-repo evidence links.
- Local working system compose docs.
- Stale package archive policy.

Likely files:

```text
docs/_incoming/curated-review-package/ontogony_curated_review_package/02_SPRINT_PLAN.md
docs/_incoming/curated-review-package/ontogony_curated_review_package/03_ACCEPTANCE_MATRIX.md
docs/evidence/SYSTEM_SPRINT4_STATUS_RECON_001_EVIDENCE.md
docs/system/CURRENT_SYSTEM_BASELINE.md
docs/archive/README.md
```

## `allagma-dotnet`

Primary owner for:

- Runtime lock.
- System E2E/restart/observability evidence.
- Allagma evidence and streaming validation.
- Runtime closeout.

Likely files:

```text
docs/system/ontogony-runtime.lock.json
docs/evidence/SYSTEM_ALPHA_004_PREP_EVIDENCE.md
docs/evidence/SYSTEM_ALPHA_004_CLOSEOUT.md
docs/evidence/ALLAGMA_EVIDENCE_001_EVIDENCE.md
docs/evidence/ALLAGMA_STREAM_001_EVIDENCE.md
```

## `conexus-dotnet`

Primary owner for:

- Retention visibility and durable maintenance history.
- Admin route inventory.
- Idempotency docs clarification.

Likely files:

```text
docs/evidence/CONEXUS_RETENTION_001_EVIDENCE.md
docs/operations/RETENTION_OPERATOR_VISIBILITY.md
docs/evidence/CONEXUS_RETENTION_002_EVIDENCE.md
docs/security/ADMIN_ROUTE_INVENTORY.md
tests/...AdminRouteInventory...
```

## `kanon-dotnet`

Primary owner for:

- Kanon-Conexus assistance evidence.
- Domain-pack lifecycle governance.
- Route module inventory/shared filters.

Likely files:

```text
docs/evidence/KANON_CONEXUS_ASSIST_001_EVIDENCE.md
docs/reviews/KANON_DOMAINPACK_GOV_001_VALIDATION_REPORT.md
docs/api/ROUTE_MODULE_INVENTORY.md
src/Kanon.Api/Endpoints/*
tests/Kanon.Tests/*RouteInventory*
```

## `ontogony-frontend`

Primary owner for:

- Docker-live operator smoke.
- Evidence spine live browser check.
- UI/fallback/live route truth surfacing.

Likely files:

```text
e2e/fe-live-smoke-docker-local.spec.ts
e2e/evidence-spine-docker-live.spec.ts
docs/evidence/FE_LIVE_SMOKE_002_EVIDENCE.md
package.json
```

## `ontogony-ui`

Primary owner for:

- Bundle/subpath report.
- Visual/a11y matrix.
- Public API/root barrel governance.

Likely files:

```text
docs/evidence/UI_BUNDLE_001_EVIDENCE.md
docs/evidence/UI_VISUAL_001_EVIDENCE.md
docs/reports/SUBPATH_BUNDLE_IMPACT.md
docs/reports/VISUAL_A11Y_COVERAGE.md
```
