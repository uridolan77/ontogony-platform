# Master Cursor Prompt — ONTOGONY-RC-CERTIFICATION-CLOSURE-001

You are working across the Ontogony multi-repo workspace.

Expected workspace:

```text
C:\dev\ontogony-platform
C:\dev\allagma-dotnet
C:\dev\kanon-dotnet
C:\dev\conexus-dotnet
C:\dev\metabole-dotnet
C:\dev\aisthesis-dotnet
C:\dev\ontogony-frontend
C:\dev\ontogony-ui
```

## Objective

Close the current RC-certification blockers without expanding scope.

Do not add new product features. Do not rename public APIs unless a drift test proves that the name already exists and generated artifacts are stale. Do not weaken safety gates. Do not claim production readiness.

## Current blockers

1. Runtime port / service identity drift:
   - canonical lock expects Metabole on `5084`;
   - canonical lock expects Aisthesis on `5085`;
   - some live-cert evidence suggests the docker-local stack used Aisthesis `5084` and Metabole `5085`.

2. Aisthesis live cert cannot pass because producer workflows fail:
   - Metabole schema-profile trigger returned `502`;
   - Allagma governed run trigger returned `500`;
   - no `traceId` was produced;
   - `producersObserved` was empty.

3. Allagma full suite has known drift:
   - `phenomenological-projection` route/matrix/OpenAPI drift;
   - phenomenological bridge event vocabulary drift;
   - skill optimization evidence expects `openai` while fake provider returns `fake`;
   - smoke script assertion drift.

4. Kanon package-mode build blocks Allagma:
   - `Kanon.Contracts` pack misses or mispackages `ReplayTarget` types.

5. Conexus hardening wrapper still fails:
   - `ConexusGatewayMetricsTests.RecordCache_instruments_emit_lookup_hit_and_miss`.

## Hard boundaries

- Allagma owns governed execution, not semantic truth.
- Kanon owns semantic authority, policy, canonical facts, gates, provenance.
- Conexus owns model routing, provider details, aliases, quota/cost.
- Metabole owns data-spine profiling, SLOD candidates, lineage, evidence.
- Aisthesis owns evidence ingestion/query/reconstructability certification.
- Ontogony.Platform owns shared mechanics and system contracts.

## Definition of done

The package is not done until all of these are true:

```powershell
# All repos local tests are green or explicitly filtered only for external-provider/operator-owned gates.
dotnet test

# Package mode gates pass for package-consuming repos.
# Five-service live certification passes with service identities verified.
# System cohesion acceptance passes.
# Closeout evidence is written in each touched repo.
```

## Execution rule

Fix the earliest failing gate first. Do not mask failures. If a gate is deferred, it must have owner, blocker, next command, non-claim, and evidence path.
