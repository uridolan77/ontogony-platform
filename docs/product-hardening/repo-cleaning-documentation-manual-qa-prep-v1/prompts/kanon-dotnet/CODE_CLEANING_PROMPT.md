# kanon-dotnet — Code Cleaning / Tightening Prompt

```text
We are starting RCQ-CODE-001 for kanon-dotnet.

Repo:
- uridolan77/kanon-dotnet

Goal:
Tighten Kanon topology/decision/provenance paths used by evidence journeys.

Boundary:
- Code hygiene/correctness only.
- No broad refactor.
- No new product feature.
- No production-readiness claim.
- No secrets.

Tasks:
1. Inspect decision-record lookup surfaces.
2. Inspect topology evidence and diagnostics paths.
3. Inspect trace/idempotency header handling.
4. Inspect tests around lookup/not-found/auth/degraded states.
5. Fix small correctness issues only.
6. Do not change topology policy semantics without explicit evidence.

Validation:
Relevant Kanon tests; no policy behavior changes.

Acceptance:
- focused checks pass or pre-existing failures documented
- no secrets
- no broad refactor
- not production readiness

Suggested branch:
chore/rcq-code-001-kanon-dotnet

Suggested commit:
chore(repo): tighten code hygiene before manual QA
```
