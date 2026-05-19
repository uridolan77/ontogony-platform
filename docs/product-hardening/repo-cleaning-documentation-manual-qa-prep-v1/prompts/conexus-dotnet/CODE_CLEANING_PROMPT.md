# conexus-dotnet — Code Cleaning / Tightening Prompt

```text
We are starting RCQ-CODE-001 for conexus-dotnet.

Repo:
- uridolan77/conexus-dotnet

Goal:
Tighten Conexus bootstrap/readiness/model-call evidence paths before manual QA.

Boundary:
- Code hygiene/correctness only.
- No broad refactor.
- No new product feature.
- No production-readiness claim.
- No secrets.

Tasks:
1. Inspect liveness/readiness mapping.
2. Inspect dev bootstrap and fake provider alias behavior.
3. Inspect route/model-call evidence read paths.
4. Check redaction/logging for provider keys and connection strings.
5. Preserve strict /ready semantics.
6. Do not enable real provider mode.

Validation:
Targeted readiness/bootstrap/evidence tests; no provider safety bypass.

Acceptance:
- focused checks pass or pre-existing failures documented
- no secrets
- no broad refactor
- not production readiness

Suggested branch:
chore/rcq-code-001-conexus-dotnet

Suggested commit:
chore(repo): tighten code hygiene before manual QA
```
