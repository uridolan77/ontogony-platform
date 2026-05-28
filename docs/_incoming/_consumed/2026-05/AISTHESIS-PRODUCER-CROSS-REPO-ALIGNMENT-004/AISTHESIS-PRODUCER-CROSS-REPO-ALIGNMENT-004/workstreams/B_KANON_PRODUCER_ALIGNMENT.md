# Kanon producer alignment

Kanon owns semantic plans, semantic decisions, and policy evaluation.

## Required

- Emit semantic plan evidence with `semanticPlanId`.
- Emit semantic decision evidence with `decisionId` and `semanticPlanId`.
- Emit policy evaluation evidence with `policyEvaluationId`.
- Emit:
  - `kanon.decision_to_semantic_plan`
  - `kanon.decision_to_policy`
- Emit grade-critical authorization edge to Conexus model call when model usage is authorized.
- Add tests for plan/decision/policy envelopes and edges.
