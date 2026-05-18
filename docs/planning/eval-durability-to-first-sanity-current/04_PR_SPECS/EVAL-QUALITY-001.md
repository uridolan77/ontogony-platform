# EVAL-QUALITY-001 — Richer quality scoring and calibrated judge scaffolding

## Repo

- `allagma-dotnet`
- optional docs in `ontogony-platform`

## Goal

Move beyond evidence-presence scoring into richer inspectable scoring.

## Dimensions

```text
task_success
instruction_following
semantic_grounding
policy_correctness
topology_appropriateness
tool_trajectory_correctness
side_effect_safety
route_evidence_quality
cost_efficiency
latency_efficiency
replayability
audit_completeness
operator_explainability
```

## Judge modes

```text
deterministic
rule_based
human_label
llm_judge_assisted
hybrid
```

## Hard rule

LLM judge assistance is disabled by default and advisory only until calibrated.

## Evidence

Add:

```text
docs/evidence/EVAL_QUALITY_001_SCORING_AND_JUDGES_EVIDENCE.md
```
