# Real Provider Validation Results — 2026-05-19

**Status:** **PASS**  
**Boundary:** Local real-provider validation only. **Not production readiness.**

## Configuration

| Field | Value |
| --- | --- |
| Provider | `openai` |
| Model | `gpt-4o-mini` |
| Max calls (session budget) | 3 |
| Max output tokens (policy) | 256 |
| Actual probe `max_tokens` | 16 |
| Real calls made (this session) | 2 |
| Secret source | local shell env only (not recorded) |
| CI involved | **no** |
| Stack | Docker-local `5081` Kanon, `5082` Conexus, `5083` Allagma, `5175` frontend |

## Evidence IDs (sanitized)

| ID | Value |
| --- | --- |
| `runId` (real subject) | `run_01ca4ffb5f304f0284478face401dd1e` |
| `evaluationRunId` | `eval_2ea02c52ddc0481e9b66d9bdf655dbdb` |
| `traceId` | `4c3e16f5fb32fb209a263a99b7063971` |
| `route_trace_id` | `78e406fc2e6f7c78be27a09e4b6e03ad` |
| `planningDecisionId` | `decision_46fb55844cf14c21b44a3b45d32a5603` |
| `routeDecisionId` | `rd-0HNLLR3A2CTT8-00000002` |
| `modelCallId` (Allagma run) | `chatcmpl-0HNLLR3A2CTT8-00000002` |
| `modelCallId` (Conexus live probe) | `chatcmpl-ce8486e2121340703f0c3b10d5eea81f` |
| `runId` (fake baseline) | `run_1ad0b28cc004449e9cf63774c0fc7637` |
| `runId` (fake regression) | `run_e66f3e78ee744b639d50e68a934b1de3` |

## Redaction

- No API keys, bearer tokens, or connection strings recorded.
- No raw sensitive prompts or completions recorded.
- Reports and evidence JSON exclude secrets by design.

## Phase results

| Phase | Result |
| --- | --- |
| Fake-provider baseline | **PASS** |
| Conexus real-provider smoke | **PASS** |
| Allagma guided real flow | **PASS** |
| Eval evidence export | **PASS** |
| Frontend visibility (RP-004) | **PASS** (E2E + live route metrics) |
| Kill switch + fake regression | **PASS** |

## Fake-provider regression

After `CONEXUS_REAL_PROVIDER_ENABLED=false` and fake bootstrap:

- Regression run: `run_e66f3e78ee744b639d50e68a934b1de3`
- Provider: `fake`
- **PASS**

## What is fair to say

```text
Real-provider local validation has been proven end-to-end:
Conexus → OpenAI → Allagma run → eval export → fake regression.
```

## What is not claimed

```text
production ready
cloud ready
secure for real traffic
provider benchmarked
cost-governed beyond local/manual caps
```

## Limitations

1. Guided-flow JSON marks `realProvider` as `classified` due to a script property bug; the Allagma run itself **completed** with `route_provider_key=openai`.
2. Token/cost totals were not present on eval route metrics in this run.
3. Conexus admin execution-run lookup returned 404 for the model-call id used as request id (route evidence still on eval).
4. Docker `ontogony-frontend` container may need `docker compose build ontogony-frontend` for live UI banners; source RP-004 tests pass.

## Artifacts

| Artifact | Path |
| --- | --- |
| RP-003A live report | `ontogony-platform/docker/local-working-system/artifacts/rp-003a/live-provider-validation-report.json` |
| RP-003 guided report | `allagma-dotnet/artifacts/rp-003/real-provider-guided-flow-report.json` |
| RP-005 evidence | `ontogony-platform/docs/evidence/RP_005_REAL_PROVIDER_MANUAL_QA_EXECUTION_EVIDENCE.md` |

## Next

**`RP-CLOSEOUT-001`** — package closeout (scorecard, limitations, next options).
