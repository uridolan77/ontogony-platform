# RP-005 — Real Provider Manual QA Execution Prompt

```text
We are starting RP-005 — Real provider manual QA execution.

Repo:
- uridolan77/ontogony-platform

Prerequisites:
- RP-001 done
- RP-002 done
- RP-003 done
- RP-004 done or explicitly deferred
- fake-provider Docker-local baseline passing

Goal:
Execute controlled local real-provider QA and record sanitized results.

Boundary:
- Manual local verification only.
- No production readiness.
- No cloud deployment.
- No CI.
- No secrets in results.
- No customer data.
- No large-cost run.
- No external sandbox/tool execution.

Execution:
1. Pull latest main in relevant repos.
2. Confirm fake-provider Docker-local smoke passes.
3. Set provider secret outside git.
4. Confirm budget caps and manual consent.
5. Run Conexus real-provider smoke.
6. Run Allagma real-provider guided flow.
7. Inspect Conexus evidence, Allagma run/eval/export, trace/correlation, and frontend visibility.
8. Record call/token/cost if available.
9. Disable real mode.
10. Rerun fake-provider smoke.

Deliver:
- docs/evidence/RP_005_REAL_PROVIDER_MANUAL_QA_EXECUTION_EVIDENCE.md
- docs/product-hardening/real-provider-validation-package-v1/results/YYYY-MM-DD_REAL_PROVIDER_VALIDATION_RESULTS.md

Acceptance:
- real-provider validation executed or external failure classified
- no secrets
- fake-provider regression passes
- not production readiness

Suggested commit:
docs(product): record real provider validation results
```
