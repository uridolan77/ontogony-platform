# Cursor Implementation Prompt

Use this prompt after unpacking the package into the intended repo/location.

```text
You are implementing ONTOGONY-SYSTEM-TEST-HARNESS-001.

Goal:
Turn the starter harness into a runnable automated test suite that replaces at least 50% of repeatable manual regression testing across Ontogony.

Repositories involved:
- uridolan77/ontogony-platform
- uridolan77/conexus-dotnet
- uridolan77/kanon-dotnet
- uridolan77/allagma-dotnet
- uridolan77/metabole-dotnet
- uridolan77/aisthesis-dotnet
- uridolan77/ontogony-frontend
- uridolan77/ontogony-ui

Rules:
1. Do not weaken service boundaries.
2. Do not introduce real external provider or real tool execution into default CI.
3. Use fake/local deterministic providers for default tests.
4. Every cross-service E2E test must write an evidence bundle.
5. Contract tests must fail on undocumented route/schema drift.
6. Auth/negative tests must exist for every protected route.
7. Idempotency tests must check duplicate prevention, not only status code.
8. Restart tests must prove durable state, not just process recovery.
9. UI tests must prove backend capability coverage, not only page render.
10. Keep destructive tests opt-in behind environment flags.

First implementation steps:
1. Calibrate manifests/services.yml and manifests/routes.yml against the current repos.
2. Fill exact route paths, auth headers, and required seed data.
3. Make tests/dotnet compile.
4. Implement readiness tests first.
5. Implement Allagma governed run E2E next.
6. Add evidence bundle writing for every test.
7. Wire run scripts and CI workflow.
8. Produce a first coverage report showing which manual checks can be retired.
```
