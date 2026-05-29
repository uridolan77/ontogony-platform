# Allagma producer alignment

Allagma owns run lifecycle and causal relations from governed execution to downstream semantic/model work.

## Required

- Emit `run_started`, `run_completed`, `run_failed` with `runId`.
- Preserve `traceId` and `correlationId`.
- Capture downstream `semanticPlanId`, `decisionId`, `modelCallId`.
- Emit:
  - `allagma.run_to_kanon.semantic_plan`
  - `allagma.run_to_kanon.decision`
  - `allagma.run_to_conexus.model_call`
- Add tests for envelope fields, edge emission, idempotency, disabled emitter.
- Add/validate `scripts/system/run-allagma-aisthesis-producer-smoke.ps1`.
