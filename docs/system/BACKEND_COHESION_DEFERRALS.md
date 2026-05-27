# Backend cohesion deferrals registry

**Manifest:** [`backend-cohesion/ONTOGONY_BACKEND_COHESION_MANIFEST.v0.json`](./backend-cohesion/ONTOGONY_BACKEND_COHESION_MANIFEST.v0.json)

Explicit deferrals prevent silent scope creep. Runtime validation packages must not mark these **complete** until the listed next gate closes.

| Id | Title | Status | Owner repo(s) | Next gate |
| --- | --- | --- | --- | --- |
| `deferral-live-allagma-candor-chat-e2e` | Live Allagma → Candor Chat consumer E2E | open | Allagma, frontend | Sandbox consumer activation with real consumer |
| `deferral-allagma-skill-release-openapi-sync` | Allagma skill-release OpenAPI snapshot sync | open | Allagma | ALLAGMA-BACKEND-COHESION-VALIDATION-001 slice 001H |
| `deferral-production-deployment-controls` | Production deployment controls | open | Platform | RELEASE-READINESS-TRUTH program |
| `deferral-kanon-rollback-governance-evaluation` | Kanon rollback governance evaluation | open | Kanon | KANON-BACKEND-COHESION-VALIDATION-001 |
| `deferral-kanon-binding-governance-evaluation` | Kanon binding governance evaluation | open | Kanon | KANON-BACKEND-COHESION-VALIDATION-001 |
| `deferral-conexus-real-provider-coverage` | Conexus real provider coverage beyond fake/local | open | Conexus | CONEXUS-BACKEND-COHESION-VALIDATION-001 |
| `deferral-metabole-role-evolution-integration` | Metabole role/evolution integration validation | open | Metabole | SLOD profiling or Metabole cohesion validation |
| `deferral-frontend-global-typecheck-unrelated` | Frontend global typecheck unrelated errors | open | Frontend | Frontend hygiene sweep |
| `deferral-known-unrelated-backend-full-suite-failures` | Known unrelated backend full-suite failures | open | All runtimes | Per-repo scorecard triage |

## Production deployment

`productionDeployment.status` in the manifest is **`not_complete`** and must remain so until a separate release-readiness program explicitly promotes it. Backend cohesion truth does not authorize production cutover.

## Using deferrals in validation

- Scorecards in runtime repos should reference deferral **ids** from this registry, not ad-hoc strings.
- Closing a deferral requires evidence in the owning repo and a manifest `updatedUtc` bump if status changes.
- New deferrals require a schema-valid manifest entry and a row in this document.
