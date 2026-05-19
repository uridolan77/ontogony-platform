# FE-PRODUCT-003 — Replay evidence workbench (platform index)

**Date:** 2026-05-19  
**Primary implementation:** `ontogony-frontend`  
**Spec:** `docs/product-hardening/eval-alignment-frontend-depth/pr-specs/FE-PRODUCT-003_REPLAY_EVIDENCE_WORKBENCH.md`

## Summary

Frontend replay workbench depth: lookup modes (run/trace/decision), limitation-aware replay operation banner, evidence export, evidence journey cross-links to eval/baseline/dataset/Conexus/Kanon/run surfaces, and cross-service identifier links when correlation resolves.

**Not production readiness.**

## Frontend evidence

`ontogony-frontend/docs/evidence/FE_PRODUCT_003_REPLAY_EVIDENCE_WORKBENCH_EVIDENCE.md`

## Validation commands (frontend repo)

```bash
cd ontogony-frontend
npm run replay:check
npm run test:e2e -- e2e/allagma-replay-evidence.spec.ts
```

## Known limitations

- No live replay trigger until Allagma OpenAPI documents `POST /allagma/v0/runs/{runId}/replay`.
- Replay page has no dedicated fixture query param; mocks drive E2E.
