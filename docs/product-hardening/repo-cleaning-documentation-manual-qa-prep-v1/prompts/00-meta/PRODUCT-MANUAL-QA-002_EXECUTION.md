# PRODUCT-MANUAL-QA-002 — Manual QA Execution

```text
We are starting PRODUCT-MANUAL-QA-002 — execute full manual guided product verification.

Repo:
- uridolan77/ontogony-platform

Goal:
Run and record the full manual guided QA checklist from a fresh Docker-local stack.

Process:
1. Pull main in all six repos.
2. Rebuild Docker-local stack.
3. Start stack.
4. Run seed/bootstrap.
5. Run guided main flow.
6. Execute every checklist.
7. Record pass/fail/partial per route.
8. Record defects as follow-up prompts.
9. Produce results doc.

Add:
- docs/evidence/PRODUCT_MANUAL_QA_002_EXECUTION_EVIDENCE.md
- docs/product-hardening/manual-guided-qa/results/YYYY-MM-DD_FULL_MANUAL_QA_RESULTS.md

Boundary:
Verification only. Not production readiness. No secrets.

Suggested commit:
docs(product): record full manual guided QA results
```
