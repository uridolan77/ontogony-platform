# Current Repo State Baseline

## Completed

### EVAL-FIX-001

- Baseline comparison now rejects scenario mismatch.
- Frontend model-call evidence card shows provider/model/alias/fallback/cost/latency.
- Latest evaluation selection is deterministic.

### CX-ROUTE-EVIDENCE-001

- Conexus route-decision-linked model-call evidence exists.
- Response includes routeDecisionId, requestId, traceId, provider/model, requested alias, fallback, cost, tokens, latency, completedAtUtc.
- Structured 404 exists.
- Conexus route-decision metric exists.

### AGM-EVAL-001

- Allagma deterministic eval harness exists.
- Fixture-backed replay only.
- Eval summary and scorecard are generated.
- Five replay kinds exist:
  - summarize_player_risk_baseline
  - unregistered_tool_blocked
  - human_gate_required
  - provider_fallback_recorded
  - sandbox_replay_safe

### SYS-EVAL-001

- Cross-repo eval smoke recorded PASS.
- Canonical committed machine report:
  - `docs/evidence/SYS_EVAL_001_CROSS_REPO_EVAL_SMOKE.report.json`
- Local artifacts remain gitignored.

### SYS-OBS-EVAL-001

- Eval metrics documented and wired.
- Operator docs exist.
- Bounded dimensions documented.
- Forbidden raw/high-cardinality dimensions documented.

## Known carried limitations

- Evaluation records and baseline comparisons are not durably persisted yet.
- `artifacts/eval/` and `artifacts/sys-eval/` remain local/gitignored.
- Fixture harness routeDecisionId may remain null by design.
- Live run topology summary may not yet link evaluationRunIds/baselineComparisonId.
- Full system sanity test has not yet been promoted to a durable release gate.
