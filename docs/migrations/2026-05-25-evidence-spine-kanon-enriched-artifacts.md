# Evidence Spine — Kanon enriched semantic artifact IDs (KANON-SEMANTIC-DEPTH-003)

**Date:** 2026-05-25  
**Consumers:** `ontogony-frontend` (resolver), `ontogony-platform` (taxonomy/matrix), `kanon-dotnet` (handoff index)

## Summary

Adds Evidence Spine resolver roots for Kanon semantic substrate artifacts beyond decision-centric lookup:

- `canonicalFactId`
- `semanticPlanId`
- `semanticQualitySnapshotId`
- `operatorReviewItemId`
- `ontologyVersionId` (graph anchor)
- `sourceBindingId` (documented in platform matrix; frontend already resolved)

No new Kanon HTTP routes. Uses existing `/ontology/v0` detail and list routes.

## Platform

- `docs/system/system-evidence-spine-resolution.matrix.json` — identifier rows + `kanonHandoffId` links
- `docs/operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md` — operator taxonomy table
- `docs/operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md` — additional supported roots list

## Kanon

- `docs/generated/KANON_EVIDENCE_SPINE_ENTRYPOINTS.json` — handoff entrypoints for enriched artifacts
- `docs/operators/KANON_EVIDENCE_SPINE_HANDOFF.md` — operator retrieval sections

## Frontend

- `src/evidence-spine/*` — parser, shallow/deep resolver, page links, lookup bar kinds
- `src/kanon/api/kanonSemanticQualityClient.ts` — snapshot detail GET helper

## Verification

```bash
# Platform contract gate (from ontogony-platform)
./scripts/validate-system-evidence-spine-contract.ps1

# Kanon handoff drift
dotnet test tests/Kanon.Tests/Kanon.Tests.csproj -c Release --filter KanonEvidenceSpineHandoff

# Frontend
cd ontogony-frontend && npm test -- src/evidence-spine/
```
