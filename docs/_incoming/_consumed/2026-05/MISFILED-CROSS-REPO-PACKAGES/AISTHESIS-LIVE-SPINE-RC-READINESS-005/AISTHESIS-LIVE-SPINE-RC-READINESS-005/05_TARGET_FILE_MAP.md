# Target file map

## aisthesis-dotnet — add/update

```text
docs/evidence/AISTHESIS_LIVE_SPINE_RC_READINESS_005_CLOSEOUT.md
docs/evidence/AISTHESIS_LIVE_SPINE_RC_READINESS_005_RELEASE_GATES.md
docs/evidence/AISTHESIS_LIVE_SPINE_RC_READINESS_005_LIVE_PROOF.md
docs/evidence/AISTHESIS_LIVE_SPINE_RC_READINESS_005_LOCK_DECISION.md
docs/evidence/AISTHESIS_LIVE_SPINE_RC_READINESS_005_DEFERRALS.md
docs/operations/AISTHESIS_FIVE_SERVICE_CI_SMOKE_RUNBOOK.md
docs/operations/AISTHESIS_RELEASE_MODE_RUNBOOK.md
docs/contracts/AISTHESIS_LIVE_SPINE_SUMMARY_V2.md
docs/contracts/AISTHESIS_RC_READINESS_GATE_V0.md
scripts/system/run-aisthesis-rc-readiness.ps1
scripts/system/run-five-service-ci-smoke.ps1
```

## aisthesis-dotnet — update only if needed

```text
README.md
docs/status/AISTHESIS_CURRENT_STATE.md
docs/generated/AISTHESIS_ROUTE_INVENTORY.json
docs/generated/openapi.json
```

Only update route inventory/OpenAPI if actual routes change. This package should not require route changes.

## ontogony-platform — add/update

```text
docs/evidence/AISTHESIS_LIVE_SPINE_RC_READINESS_005_PLATFORM_CLOSEOUT.md
docs/evidence/SYSTEM_RC_003_AISTHESIS_LOCK_REVIEW_005.md
docs/evidence/SYSTEM_RC_003_AISTHESIS_LOCK_REVIEW_005_DEFERRALS.md
```

## producer repos

Modify only if live proof or Release gates reveal emitter regressions. Otherwise write validation notes only.
