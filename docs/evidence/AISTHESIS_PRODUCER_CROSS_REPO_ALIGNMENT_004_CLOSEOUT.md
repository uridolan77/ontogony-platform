# AISTHESIS-PRODUCER-CROSS-REPO-ALIGNMENT-004 closeout

**Package:** `AISTHESIS-PRODUCER-CROSS-REPO-ALIGNMENT-004`  
**Unpacked to:** `docs/_incoming/packages/AISTHESIS-PRODUCER-CROSS-REPO-ALIGNMENT-004/`  
**Date:** 2026-05-28

## Summary

Producer repos were aligned to emit native Aisthesis envelopes, stable native IDs, and required-edge relations matching `AISTHESIS_REQUIRED_EDGE_MATRIX_V1` / fixture `CROSS_SYSTEM_TRACE_REQUIRED_EDGES_V1.evidence.json`.

## Allowed claims

- Producer emitters aligned with the Aisthesis required-edge contract (envelope fields + edge emission where implemented).
- Repo-level library builds and targeted tests pass for changed projects.
- Aisthesis fixture smoke remains **PASS** (`requiredEdges.present: 10`, `requiredEdges.missing: 0`, `reconstructabilityGrade: complete`).
- Live five-service proof harness is ready for a coordinated run.

## Not claimed

- Full live five-service **PASS** from native producer emission (live mode **NOT_RUN** in this session).
- Frontend live backing, production IAM, retention/erasure APIs, or distributed trace export.

## Required-edge ownership (after alignment)

| Edge ID | Owner | Alignment action |
|---|---|---|
| `allagma.run_to_kanon.semantic_plan` | Allagma | Cross-producer `requested` link to Kanon `semantic_plan` evidence ID |
| `allagma.run_to_kanon.decision` | Allagma | Cross-producer `evaluated_by` link to Kanon `semantic_decision` |
| `allagma.run_to_conexus.model_call` | Allagma | Cross-producer `requested` link to Conexus `model_call` |
| `kanon.decision_to_semantic_plan` | Kanon | `derived_from` edge after plan + decision emit |
| `kanon.decision_to_policy` | Kanon | `evaluated` edge; `policy_evaluation` envelope with `policyEvaluationId` |
| `conexus.model_call_to_route_decision` | Conexus | `routed_to` edge on model call completion |
| `conexus.model_call_to_provider_attempt` | Conexus | `providerAttemptId` on model call envelope |
| `metabole.pipeline_to_profile` | Metabole | `transformation_plan` + `produced` edge |
| `metabole.pipeline_to_mapping_candidate` | Metabole | `produced` edge pipeline → mapping candidate |
| `metabole.mapping_candidate_to_artifact` | Metabole | `output_artifact` + `materialized_as` edge |

## Validation

See `AISTHESIS_PRODUCER_ALIGNMENT_004_REPO_RESULTS.md` and `AISTHESIS_PRODUCER_ALIGNMENT_004_LIVE_PROOF_STATUS.md`.

Fixture smoke artifact: `aisthesis-dotnet/artifacts/five-service-aisthesis-live-smoke/20260528T080350Z/summary.json`.
