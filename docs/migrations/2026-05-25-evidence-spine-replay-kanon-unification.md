# Evidence Spine replay + Kanon lifecycle unification

**Date:** 2026-05-25  
**Package:** EVIDENCE-SPINE-REPLAY-KANON-001  
**Consumers:** `ontogony-frontend`, `ontogony-platform`, `kanon-dotnet`, `allagma-dotnet` (optional ref)

## Summary

Unifies the cross-service Evidence Spine investigative graph for:

- Allagma replay orchestration roots (`replayId`, bundle, delta)
- Kanon decision lifecycle projection on decision nodes
- Enriched semantic artifact nodes and explicit edge taxonomy
- Agent Interaction bridges (`replayId`, spine, replay workbench)

## Platform

- `docs/system/system-evidence-spine-resolution.matrix.json` — `allagmaReplayId`, `replayBundleId` rows; `supplementalRequiredIdentifierKinds`
- `docs/system/EVIDENCE_SPINE_GRAPH_TAXONOMY.md` — canonical node/edge enums
- `scripts/validate-system-evidence-spine-contract.ps1` — supplemental replay gate

## Kanon

- `docs/generated/KANON_EVIDENCE_SPINE_ENTRYPOINTS.json` — `decision_lifecycle` handoff
- No new HTTP routes

## Frontend

- `src/evidence-spine/*` — parser aliases, replay graph edges, lifecycle on decisions, workbench sections
- `src/agent-interaction/*` — `replayId` lookup and cross-links

## Follow-up (001A — graph semantic polish)

- Request → result edge uses `replay_recorded_result` (not `replay_produced_bundle`).
- `evidenceSpineRootRef()` aligns `graph.root` for `replay-delta:` / `replay-evidence-bundle:` inputs.
- `replayDeltaId` added to platform supplemental required identifier kinds.

## Verification

```powershell
cd ontogony-platform
./scripts/validate-system-evidence-spine-contract.ps1

cd kanon-dotnet
dotnet test tests/Kanon.Tests/Kanon.Tests.csproj --filter KanonEvidenceSpineHandoff

cd ontogony-frontend
npm test -- src/evidence-spine
npm test -- src/agent-interaction
npm run typecheck
```
