# RP-005 — Real provider manual QA execution evidence

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS**

**Boundary:** Manual local verification only. **No secrets in this document or git.** **No CI real-provider calls.** **Not production readiness.**

## Executive summary

Controlled local real-provider manual QA was executed on the Docker-local stack (`5081`–`5083`, frontend `5175`). One successful end-to-end real-provider path was re-validated in this session:

```text
Fake baseline (Conexus fake)     PASS
Conexus live OpenAI probe      PASS
Allagma real subject run       PASS (providerKey=openai)
Eval + evidence export         PASS
Fake regression (kill switch)  PASS
```

Cross-reference: [`RP_003A_LIVE_REAL_PROVIDER_COMPLETION_EVIDENCE.md`](RP_003A_LIVE_REAL_PROVIDER_COMPLETION_EVIDENCE.md) (first live completion after Docker TLS fix). This RP-005 run re-executes the checklist with fresh sanitized IDs.

## Preflight

| Check | Result |
| --- | --- |
| `CONEXUS_PROVIDER_OPENAI_API_KEY` in local shell only | **PASS** (set; value not recorded) |
| No provider keys in git diff (platform/allagma/frontend/conexus) | **PASS** |
| Budget caps (≤3 calls, ≤256 output tokens, small model) | **PASS** (`gpt-4o-mini`, `max_tokens` 16 in probes) |
| Fake-provider baseline before real run | **PASS** |
| RP-004 frontend visibility in repo | **PASS** (committed; E2E green) |

## Execution

### 1. Fake-provider baseline

```powershell
# Conexus alias -> fake; CONEXUS_REAL_PROVIDER_ENABLED=false
POST http://localhost:5082/v1/chat/completions  # model gpt-4o-mini
```

| Field | Value |
| --- | --- |
| Result | **PASS** |
| `modelCallId` | `chatcmpl-41ff33f732c442dc75f0f335f2b6041c` |

### 2. RP-003A live validation (re-run for RP-005)

```powershell
cd C:\dev\ontogony-platform\docker\local-working-system
.\scripts\run-rp-003a-live-provider-validation.ps1 -SkipRebuild
```

| Phase | Result |
| --- | --- |
| TLS CA inject (Avast root) | **PASS** (required) |
| Conexus live OpenAI probe | **PASS** — `chatcmpl-ce8486e2121340703f0c3b10d5eea81f` |
| Allagma guided flow (overall) | **PASS** |
| Report | `docker/local-working-system/artifacts/rp-003a/live-provider-validation-report.json` |

### 3. Allagma real-provider subject (this session)

| Field | Value |
| --- | --- |
| `runId` | `run_01ca4ffb5f304f0284478face401dd1e` |
| `status` | `Completed` |
| `modelCallId` | `chatcmpl-0HNLLR3A2CTT8-00000002` |
| `planningDecisionId` | `decision_46fb55844cf14c21b44a3b45d32a5603` |
| `evaluationRunId` | `eval_2ea02c52ddc0481e9b66d9bdf655dbdb` |
| Eval `verdict` | `pass` |
| `traceId` (eval) | `4c3e16f5fb32fb209a263a99b7063971` |
| `route_provider_key` | `openai` |
| `route_provider_model` | `gpt-4o-mini` |
| `route_decision_id` | `rd-0HNLLR3A2CTT8-00000002` |
| `route_trace_id` | `78e406fc2e6f7c78be27a09e4b6e03ad` |
| Evidence export schema | `ontogony-allagma-eval-evidence-export-bundle-v1` |

### 4. Fake-provider regression (kill switch)

| Field | Value |
| --- | --- |
| Result | **PASS** |
| `runId` | `run_e66f3e78ee744b639d50e68a934b1de3` |
| `providerKey` | `fake` |

Guided-flow report: `allagma-dotnet/artifacts/rp-003/real-provider-guided-flow-report.json` (`verdict=PASS`).

## Real call accounting

| Metric | Value |
| --- | --- |
| Provider | `openai` (name only) |
| Model | `gpt-4o-mini` |
| Real outbound calls (this session) | **2** (Conexus probe + Allagma subject) |
| Budget max calls | **3** |
| Max output tokens (probe config) | **16** (policy cap **256**) |
| Token counts in eval metrics | Not populated (`route_input_tokens` / `route_output_tokens` empty) |

## Frontend visibility (RP-004)

| Surface | Result |
| --- | --- |
| RP-004 unit tests (`resolveProviderValidationState`) | **PASS** (12) |
| RP-004 E2E (`conexus-observability.spec.ts`) | **PASS** (4) — posture banner + `unauthorized_missing_key` from mock inventory |
| Live eval route metrics (`route_provider_key=openai`) | **PASS** — data path for eval/run banners |
| Live Docker UI rebuild | **Operator note:** rebuild `ontogony-frontend` image if banners not visible in container UI |

## Classified / limitations

| Item | Classification |
| --- | --- |
| Guided-flow report `realProvider.status=classified` | **Script reporting defect** — PowerShell property `selectedProviderKey` missing on probe object; **run `run_01ca4ffb5f304f0284478face401dd1e` completed with `openai`** |
| Conexus execution-run lookup by `modelCallId` | **404** for admin diagnostics path in this session (eval metrics still carry route evidence) |
| Token/cost fields | **Unavailable** in exported eval metrics |

## Acceptance (RP-005)

| Criterion | Result |
| --- | --- |
| Real-provider manual QA executed or failure classified | **PASS** |
| No secrets in docs / diff | **PASS** |
| Frontend visibility checked | **PASS** (automated + live metrics) |
| Fake-provider regression after kill switch | **PASS** |
| No CI real-provider behavior | **PASS** |
| Not production readiness | **Stated** |

## Next step

**`RP-CLOSEOUT-001`** — Real provider validation closeout.

## Required statement

```text
RP-005 executed controlled local real-provider manual QA with sanitized evidence IDs.
Fake provider remains the default after kill switch.
No CI real-provider calls. No secrets in git.
Not production readiness.
```
