# Slice 5 — ALLAGMA-CONEXUS-MODEL-ALIAS-001

**Owner:** `allagma-dotnet` + `conexus-dotnet`  
**Depends on:** Slice 4  
**Prompt:** [`../prompts/P05_ALLAGMA_CONEXUS_MODEL_ALIAS_001.md`](../prompts/P05_ALLAGMA_CONEXUS_MODEL_ALIAS_001.md)

## Goal

Allagma requests models **only** via configured **Conexus model aliases** and execution purposes — never provider model IDs.

## Current baseline

- `AllagmaModelPurposeRoute.ConexusModelAlias` exists.
- Some tests still reference `gpt-4o-mini` as model string — replace with aliases.
- `conexus-model-alias-manifest.snapshot.json` exists in Allagma system docs.

## Deliverables

1. Grep-clean `allagma-dotnet/src/` (no `gpt-*` provider IDs)
2. Refreshed alias manifest snapshot + Conexus consumer contract doc
3. `appsettings.Development.json` examples use aliases only
4. Conexus documents alias registry for consumers
5. CI test: purpose resolver output is always alias

## Evidence

- `allagma-dotnet/docs/evidence/ALLAGMA_CONEXUS_MODEL_ALIAS_001_CLOSEOUT.md`
- `conexus-dotnet/docs/evidence/ALLAGMA_CONEXUS_MODEL_ALIAS_001_CLOSEOUT.md`

## Boundary

Conexus still owns routing, fallback, and provider selection. Allagma records `modelCallId` / `routeDecisionId` from Conexus responses only.
