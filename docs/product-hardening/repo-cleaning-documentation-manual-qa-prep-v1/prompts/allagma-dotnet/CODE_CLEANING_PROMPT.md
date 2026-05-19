# allagma-dotnet — Code Cleaning / Tightening Prompt

```text
We are starting RCQ-CODE-001 for allagma-dotnet.

Repo:
- uridolan77/allagma-dotnet

Goal:
Tighten Allagma eval/run/replay/export code while preserving current contracts.

Boundary:
- Code hygiene/correctness only.
- No broad refactor.
- No new product feature.
- No production-readiness claim.
- No secrets.

Tasks:
1. Inspect eval list, baseline comparison, dataset, quality metadata, and evidence export services.
2. Inspect redaction helpers and OpenAPI provenance scripts.
3. Check in-memory vs Postgres query parity.
4. Check cursor, filter, not-found, validation tests.
5. Add small edge-case tests only where needed.
6. Do not add replay trigger mutation.

Validation:
Focused Allagma tests; OpenAPI snapshot/provenance if touched; no secrets.

Acceptance:
- focused checks pass or pre-existing failures documented
- no secrets
- no broad refactor
- not production readiness

Suggested branch:
chore/rcq-code-001-allagma-dotnet

Suggested commit:
chore(repo): tighten code hygiene before manual QA
```
