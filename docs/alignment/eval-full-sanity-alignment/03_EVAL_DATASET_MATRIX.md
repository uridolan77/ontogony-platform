# Eval dataset matrix — scenario-dataset-v0

**Package:** `allagma-dotnet/docs/evals/datasets/scenario-dataset-v0/`  
**Manifest schema:** `allagma-eval-scenario-dataset-v1`  
**Default profile:** `eval-profile-v0`

## Dataset manifest

| Field | Value |
| --- | --- |
| `datasetId` | `scenario-dataset-v0` |
| `version` | `0.1.0` |
| `fixture` | `true` |
| Default CI suite size | 5 (`suite.replayKindOrder`) |
| Total cases on disk | 8 |
| Expected overlays | 2 |
| Baseline fixtures | 1 |

## Default CI suite (`suite.replayKindOrder`)

| Order | replay_kind (case file) | Case purpose |
| --- | --- | --- |
| 1 | `summarize_player_risk_baseline` | `case-summarize-player-risk-baseline.json` |
| 2 | `unregistered_tool_blocked` | `case-unregistered-tool-blocked.json` |
| 3 | `human_gate_required` | `case-human-gate-first.json` |
| 4 | `provider_fallback_recorded` | `case-provider-fallback-recorded.json` |
| 5 | `sandbox_replay_safe` | `case-sandbox-replay-safe.json` |

## Additional cases (not in default suite)

| Case file | Typical use |
| --- | --- |
| `case-single-workflow-summary.json` | Baseline run mode `single_workflow` |
| `case-topology-override-centralized.json` | Subject topology override |
| `case-high-risk-deny-decentralized.json` | Deny path coverage |

## Expected overlays

| File | Pairs with |
| --- | --- |
| `expected-single-workflow-summary.json` | Single-workflow baseline expectations |
| `expected-topology-override-centralized.json` | Centralized orchestrator override |

## Baseline comparison fixture

| File | Schema | Sanity relevance |
| --- | --- | --- |
| `baselines/baseline-comparison-single-vs-centralized.sample.json` | `allagma-baseline-comparison-v1` | Full sanity step 7 — baseline vs subject comparison |

## Frontend fixture alignment

| Frontend fixture | Backend dataset reference |
| --- | --- |
| `?dashboardFixture=ci-suite` | Mirrors CI suite scenario ids and verdict mix for operator review |
| `cmp-fixture-ci-suite` | Sample baseline comparison deep link in fixture mode |

## Validation and loading

```powershell
# From allagma-dotnet
./scripts/validate-eval-dataset.ps1
./scripts/run-eval-ci-suite.ps1
```

Application loader: `EvaluationScenarioDatasetLoader.Load(<dataset-root>)`.

## CI artifact fields

`eval-ci-summary.json` includes `datasetId` and `datasetVersion` when EVAL-DATA-001 wiring is active.

## Full sanity scenario mapping

| Sanity step | Dataset / case |
| --- | --- |
| Baseline run `single_workflow` | `case-single-workflow-summary.json` + expected overlay |
| Subject run `centralized_orchestrator` override | `case-topology-override-centralized.json` |
| Baseline comparison | `baseline-comparison-single-vs-centralized.sample.json` pattern |
