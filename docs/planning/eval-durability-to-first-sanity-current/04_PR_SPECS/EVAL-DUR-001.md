# EVAL-DUR-001 — Durable eval/baseline persistence

## Repo

- `allagma-dotnet`

## Goal

Move evaluation runs and baseline comparisons from in-memory-only to durable persistence.

## Tables

### allagma_evaluation_runs

Suggested columns:

```text
evaluation_run_id text primary key
subject_run_id text not null
trace_id text null
scenario_id text null
evaluation_profile_id text null
verdict text null
quality_score numeric null
source_kind text null
writer_version text null
contract_version text null
started_at_utc timestamptz null
completed_at_utc timestamptz null
record_json jsonb not null
created_at_utc timestamptz not null
```

Indexes:

```text
subject_run_id
scenario_id
evaluation_profile_id
source_kind
completed_at_utc
```

### allagma_baseline_comparisons

Suggested columns:

```text
comparison_id text primary key
subject_run_id text not null
baseline_run_id text null
scenario_id text null
baseline_mode text null
subject_mode text null
promotion_recommendation text null
policy_equivalent boolean null
side_effect_safe boolean null
quality_delta numeric null
cost_delta_usd numeric null
latency_delta_ms bigint null
record_json jsonb not null
created_at_utc timestamptz not null
```

Indexes:

```text
subject_run_id
baseline_run_id
scenario_id
promotion_recommendation
created_at_utc
```

## Repository behavior

Implement Postgres versions of:

```text
IEvaluationRunRepository
IBaselineComparisonRepository
```

Required behavior:

- idempotent save by primary ID
- list by subject run ordered newest first
- get by ID
- JSON round-trip preserving Ontogony.Evaluation.Contracts shape
- no raw prompts/secrets/marker payloads inserted
- in-memory repos remain for tests/local harness

## Tests

- save/get evaluation run
- list by subject run
- save/get baseline comparison
- JSON roundtrip with nested scores/metrics/cases/artifacts
- source metadata preserved
- deterministic ordering
- duplicate save behavior defined and tested
- Postgres integration test if current infrastructure supports it

## Evidence

Add:

```text
docs/evidence/EVAL_DUR_001_DURABLE_EVAL_PERSISTENCE.md
```
