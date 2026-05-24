# Prompt 01 — Implement RELEASE-READINESS-TRUTH-001

Implement the package with a narrow truth-hardening scope.

Primary goal:

> The System → Release Readiness page must no longer imply real release readiness from a generated artifact alone.

Work in this order:

1. Add or update a view-model/classification helper for generated route rows.
2. Add artifact source/freshness classification.
3. Update page header and posture panel.
4. Rename/reframe summary cards as artifact counts.
5. Add route row release impact, reason, and next action.
6. Add a live-validation section that honestly reports not attached/unavailable unless live data really exists.
7. Add tests.
8. Add/update operator docs.

Constraints:

- Do not invent backend data.
- Do not remove generated artifact functionality.
- Do not broadly refactor System pages.
- Preserve existing styling/component conventions.
- Keep changes reviewable.

After implementation, run relevant tests/typecheck/lint/build and report results.
