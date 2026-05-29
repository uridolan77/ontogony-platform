# Cursor prompt — Aisthesis RC certification 006

Apply inside `C:\Dev\aisthesis-dotnet`.

Goal: integrate RC-readiness-005 with the expanded review gaps and make Aisthesis an RC-certification candidate or honest partial.

Steps:

1. Inspect current repo state and detect whether 005 files already exist.
2. Preserve all useful 005 artifacts.
3. Add/upgrade RC certification docs/scripts/contracts from this package.
4. Implement required-edge matrix v2 or add it as a documented contract with tests if code scope is too large.
5. Complete Aisthesis.Client coverage for evaluation routes or mark evaluation routes server-only.
6. Harden direct edge authorization or document blocker.
7. Replace or plan fire-and-forget evaluation with durable evaluation job semantics.
8. Run Release restore/build/test.
9. Run fixture smoke.
10. Run five-service certification in Preflight, Fixture, and LiveOrExplain modes.
11. Produce closeout and lock decision.

Do not claim production readiness.
