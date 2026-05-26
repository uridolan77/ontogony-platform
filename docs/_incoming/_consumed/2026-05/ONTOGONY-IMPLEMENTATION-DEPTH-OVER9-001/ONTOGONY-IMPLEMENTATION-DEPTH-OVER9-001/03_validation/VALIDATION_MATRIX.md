# Validation matrix — Implementation Depth > 9

## Repo-local validation

| Repo | Required validation |
| --- | --- |
| `ontogony-platform` | full build/test; compatibility gate; package inventory; package levels; real-tools block |
| `conexus-dotnet` | default CI-equivalent tests; doc freshness; persistence smoke where configured; provider capability tests |
| `kanon-dotnet` | full tests; route/doc/openapi/manifest/client coverage filters; Postgres semantic smoke |
| `allagma-dotnet` | default tests; cross-repo architecture; runtime lock; feature matrix; system cohesion; MAF strict smoke |

## Cross-repo validation

```powershell
# From allagma-dotnet
./scripts/architecture-conformance/run-cross-repo-conformance.ps1
./scripts/validate-runtime-lock.ps1
./scripts/validate-feature-connection-matrix.ps1 -DevRoot C:/dev
./scripts/system/run-system-cohesion-acceptance.ps1
./scripts/run-system-cohesion-smoke.ps1 -UseExistingServices -IncludeKanonAssistance -IncludeConexusFallback
./scripts/smoke/run-governed-maf-e2e.ps1 -Strict
```

## Evidence required before closeout

- Repo-local `docs/evidence/*DEPTH*` artifacts.
- Final score closeout in Allagma: `docs/reviews/IMPLEMENTATION_DEPTH_OVER9_CLOSEOUT_REPORT.md`.
