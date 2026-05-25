# KANON-DEPTH-001 to KANON-DEPTH-006 — Kanon implementation-depth slice

## Objective

Raise `kanon-dotnet` implementation depth from 8.5 to 9.1+ by deepening semantic authority rather than adding generic graph/demo features.

## First required fix — route/client truth correction

Before feature work, resolve route count drift:

- generated manifest should be the source of truth for total routes, client routes, and server-only routes;
- update `CURRENT_STATE.md`, `KNOWN_LIMITATIONS.md`, compatibility docs, and generated fragments to agree;
- add a test that fails if narrative docs contain stale route-count literals.

## KANON-DEPTH-001 — Decision lifecycle depth

Deepen lifecycle semantics beyond classifier/read route:

- transition eligibility matrix;
- invalid transition explanations;
- audit event emitted for transitions;
- lifecycle timeline in decision provenance;
- Postgres coverage.

Acceptance:

- lifecycle GET and POST transition behavior tested;
- invalid transitions return stable errors or typed DTOs;
- lifecycle sidecar is reconstructable from decision record + sidecar events.

## KANON-DEPTH-002 — Domain-pack governance depth

Strengthen accepted vs active lifecycle: authoring submit/validate/promote/load path, migration/rollback evidence, auto-deprecation proof, and impact reports that cite affected semantic artifacts.

Acceptance:

- in-memory and Postgres tests cover promotion chain;
- dev bypass is explicitly labelled and cannot accidentally pass as strict governance.

## KANON-DEPTH-003 — Semantic graph depth

Deepen `/semantic-graph` for Evidence Spine and operator UI:

- canonical fact root fan-out;
- source binding edges;
- semantic plan edges;
- review item detail edges;
- decision/provenance edges;
- graph completeness metadata.

Acceptance:

- graph tests prove required node/edge types for each enriched root;
- no Conexus/Allagma implementation dependency;
- evidence spine handoff tests updated.

## KANON-DEPTH-004 — Safe Kanon.Client coverage expansion

Review server-only policy. Add typed client methods only for safe read/operator routes. Keep mutations/internal emissions server-only where appropriate.

Acceptance:

- `ONTOLOGY_V0_CLIENT_COVERAGE.json` generated and matches policy;
- server-only routes have reasons;
- Allagma conformance consumes new coverage.

## KANON-DEPTH-005 — Semantic quality history and baselines

Deepen semantic quality snapshots with baseline lifecycle, comparison history, decision record linkage, and Postgres semantic smoke coverage.

## KANON-DEPTH-006 — Conexus assistance review loop depth

Keep model assistance non-authoritative, but deepen review loop: accept/reject/convert events, redaction evidence, draft_only invariant, and no auto-approval.

## Validation commands

```powershell
pwsh ./scripts/bootstrap-solution.ps1
dotnet restore ./Kanon.sln
dotnet build ./Kanon.sln -c Release --no-restore
dotnet test ./Kanon.sln -c Release --no-build
pwsh ./scripts/update-route-doc-fragments.ps1
pwsh ./scripts/update-kanon-openapi-baseline.ps1
pwsh ./scripts/update-kanon-compatibility-manifest.ps1
dotnet test ./tests/Kanon.Tests/Kanon.Tests.csproj -c Release --filter "FullyQualifiedName~OntologyV0RouteInventory|FullyQualifiedName~OntologyV0RouteDocFragment|FullyQualifiedName~OpenApi|FullyQualifiedName~KanonCompatibilityManifest|FullyQualifiedName~ForbiddenDependency|FullyQualifiedName~KanonV0ContractFreeze"
```
