# 10 — Implementation Sequence

## Recommended exact order

### 1. `PLAT-EVAL-001`

Add neutral evaluation contracts to Ontogony.Platform. No runtime change.

### 2. `PLAT-TOPO-001`

Add neutral topology vocabulary/contracts. No runtime change.

### 3. `AGM-TOPO-001`

Allagma records task classification and selected topology. Default remains `single_workflow`.

### 4. `AGM-TOPO-002`

Add topology information to run audit bundle and event query responses.

### 5. `KANON-TOPO-001`

Kanon exposes topology-policy evaluation.

### 6. `AGM-TOPO-003`

Allagma calls Kanon topology policy for high-risk or override modes.

### 7. `EVAL-FIX-001` — Baseline comparison correctness + frontend route evidence polish

This should come **before** your current item 7.

Scope:

```text
- Fix BaselineComparisonService scenario mismatch guard.
- Add tests for baseline/subject scenario mismatch.
- Render route provider/model/cost/latency/fallback in frontend model-call card.
- Sort/select latest evaluation deterministically.
- Optionally return structured Conexus 404 body.
```

Why: this fixes the main correctness issue from the review before expanding the eval system.

---

### 8. `CX-ROUTE-EVIDENCE-001` — Route decision records, not just route evidence endpoint

Your current description says:

> Conexus emits route decision records and links them to model calls.

This is **not the same** as what already exists. Current EVAL-RUN-005 gives a safe model-call evidence endpoint. The next Conexus PR should make route decisions first-class.

Scope:

```text
- Add routeDecisionId.
- Persist route decision record per model call.
- Link modelCallId → routeDecisionId.
- Include requested model alias, resolved alias, provider key, provider model.
- Include fallback chain / selected attempt.
- Include pricing/catalog version if available.
- Include selection reason / constraints applied.
- Add GET /conexus/v0/route-decisions/{routeDecisionId}, or enrich existing model-call endpoint.
- Update OpenAPI and tests.
```

Keep the endpoint governance-safe: no raw prompts, completions, secrets, headers, or provider payloads.

---

### 9. `AGM-EVAL-001` — Deterministic eval harness runner

Rename from “Allagma eval harness runs deterministic scenario evals” to something more precise:

```text
AGM-EVAL-001 — Deterministic eval harness runner
```

Scope:

```text
- Add service/script that loads docs/evals cases.
- Runs or replays deterministic scenarios.
- Writes EvaluationRunRecord.
- Writes BaselineComparisonRecord where applicable.
- Produces eval-run JSON and scorecard output.
- Supports dry/local mode only.
- No real external execution.
```

This is the real next step after the current writer/fixtures/comparison. The writer exists; now you need the **runner**.

---

### 10. `SYS-EVAL-001` — Cross-repo eval smoke

Keep this, but make it depend on `AGM-EVAL-001` and `CX-ROUTE-EVIDENCE-001`.

Scope:

```text
- Start/verify Conexus, Kanon, Allagma, frontend.
- Run one eval scenario end-to-end.
- Confirm Allagma run → topology evidence → Kanon decision → Conexus model call → route decision evidence → evaluation record → baseline comparison → frontend visibility.
- Produce cross-repo compatibility/eval smoke report.
```

Output:

```text
docs/evidence/SYS_EVAL_001_CROSS_REPO_EVAL_SMOKE.md
artifacts/sys-eval/eval-smoke-report.json
```

---

### 11. `SYS-OBS-EVAL-001` — Eval metrics and operator docs

Keep this as the observability/docs layer after the smoke.

Scope:

```text
- Add metrics for evaluation recorded/failed.
- Add baseline comparison metrics.
- Add route evidence resolvability metric.
- Add promotion recommendation counts.
- Add operator docs for interpreting PASS/FAIL/INCONCLUSIVE.
- Add docs explaining when not to promote a topology.
```

Recommended metrics:

```text
allagma.evaluation.recorded.count
allagma.evaluation.failed.count
allagma.evaluation.verdict.count
allagma.baseline_comparison.recorded.count
allagma.baseline_comparison.recommendation.count
allagma.route_evidence.resolvable.count
conexus.route_decision.recorded.count
```

## Final sequence

Use this order:

```text
7.  EVAL-FIX-001
8.  CX-ROUTE-EVIDENCE-001
9.  AGM-EVAL-001
10. SYS-EVAL-001
11. SYS-OBS-EVAL-001
```
