# Real provider validation — scorecard

**Date:** 2026-05-19  
**Gate:** `RP-CLOSEOUT-001`  
**Evidence:** [RP_CLOSEOUT_001_REAL_PROVIDER_VALIDATION_EVIDENCE.md](../evidence/RP_CLOSEOUT_001_REAL_PROVIDER_VALIDATION_EVIDENCE.md)  
**Manual QA:** [RP_005_REAL_PROVIDER_MANUAL_QA_EXECUTION_EVIDENCE.md](../evidence/RP_005_REAL_PROVIDER_MANUAL_QA_EXECUTION_EVIDENCE.md)

**This scores real provider validation v1 only. Not production readiness.**

| Dimension | Score | Notes |
| --- | ---: | --- |
| Safety / secret discipline | 9.7/10 | Keys shell-only; no secrets in git; policy + kill switch documented |
| Fake-provider default preservation | 9.8/10 | Regression PASS after `CONEXUS_REAL_PROVIDER_ENABLED=false` |
| Budget / call control | 9.2/10 | ≤3 calls, small model, capped tokens; manual enforcement |
| Conexus provider gate (`RP-002`) | 9.1/10 | Explicit opt-in; classified missing/invalid key and transport errors |
| Allagma guided flow (`RP-003` / `RP-003A`) | 9.0/10 | End-to-end live completion after Docker TLS fix; Kanon unchanged |
| Eval evidence export | 9.0/10 | `ontogony-allagma-eval-evidence-export-bundle-v1` on successful real run |
| Frontend visibility (`RP-004`) | 8.7/10 | E2E + live route metrics PASS; Docker UI may need image rebuild |
| Fake-provider regression | 9.5/10 | Post-real kill switch verified in RP-005 |
| Evidence / redaction quality | 9.3/10 | Sanitized IDs only; no raw prompts or keys |
| Production-readiness separation | 10/10 | Boundaries stated consistently; no prod/cloud claims |
| **Overall real provider validation v1** | **9.2/10** | Package **CLOSED** |

## Implementation item acceptance (summary)

| Item | Verdict | Primary evidence |
| --- | --- | --- |
| RP-000 | PASS | `RP_000_PACKAGE_SETUP_EVIDENCE.md` |
| RP-001 | PASS | `RP_001_SECRET_BUDGET_SAFETY_GATES_EVIDENCE.md` |
| RP-002 | PASS | `RP_002_CONEXUS_REAL_PROVIDER_LOCAL_MODE_EVIDENCE.md` |
| RP-003 | PASS | `RP_003_ALLAGMA_REAL_PROVIDER_GUIDED_FLOW_EVIDENCE.md` (classified transport) |
| RP-003A | PASS | `RP_003A_LIVE_REAL_PROVIDER_COMPLETION_EVIDENCE.md` |
| RP-004 | PASS | `RP_004_FRONTEND_REAL_PROVIDER_OPERATOR_VISIBILITY_EVIDENCE.md` |
| RP-005 | PASS | `RP_005_REAL_PROVIDER_MANUAL_QA_EXECUTION_EVIDENCE.md` |
| RP-CLOSEOUT-001 | PASS | `RP_CLOSEOUT_001_REAL_PROVIDER_VALIDATION_EVIDENCE.md` |

## Score deductions (honest)

| Gap | Impact |
| --- | --- |
| Guided-flow JSON `realProvider.status=classified` despite successful run | Script property bug — operator must read eval route metrics |
| `route_input_tokens` / `route_output_tokens` empty on eval | No token/cost accounting in export for this run |
| Conexus execution-run lookup 404 for model-call id | Admin diagnostics path gap; route evidence on eval still present |
| Live Docker UI banners | May require `docker compose build ontogony-frontend` |

## What the score does not mean

- Not production SLOs, security certification, or multi-region deploy readiness
- Not proof of provider parity across models or vendors
- Not load, latency, or cost governance at enterprise scale
- Not automatic CI coverage of real-provider paths

## Combined posture

```text
First Dockerized local working system (ENV)     CLOSED  ~9.3/10
Post-Docker-local hardening                     CLOSED  ~9.3/10
Product feature hardening v1 (PFH)              CLOSED  ~9.1/10
Repo cleaning + manual QA prep (RCQ)            CLOSED
PRODUCT-MANUAL-QA-002R1 (fake provider)         PASS
Real provider validation v1 (RP-*)            CLOSED  ~9.2/10
Production readiness                            NOT STARTED
```
