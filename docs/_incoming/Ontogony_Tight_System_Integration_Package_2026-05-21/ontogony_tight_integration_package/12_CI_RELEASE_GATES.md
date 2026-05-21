# 12 — CI and release gates

## Release modes

| Mode | Purpose | Required gates |
|---|---|---|
| PR normal | quick confidence | repo build/test, scoped checks |
| PR full | integration-sensitive PR | package mode, cross-repo conformance, route parity |
| Release candidate | lock promotion | all P0 gates, evidence paths, post-lock delta classification |
| Production candidate | future | prod auth/secrets/observability/tool safety gates |

## Release candidate gate sequence

1. Checkout locked commits for all runtime repos.
2. Validate runtime lock structure.
3. Build/test each repo in its default mode.
4. Run package-mode Allagma build using pinned packages.
5. Run cross-repo architecture conformance.
6. Run Kanon manifest conformance.
7. Run Conexus contract tests needed by Allagma and operator.
8. Start Docker/local stack.
9. Run system cohesion smoke with assistance + fallback + streaming evidence.
10. Run restart survival.
11. Run operator audit Playwright smoke.
12. Update evidence indexes.
13. Cut closeout doc with non-claims.

## Required closeout doc sections

Every release closeout must include:

- baseline name,
- locked commits,
- package versions,
- evidence artifacts,
- pass/fail gate table,
- open quarantines,
- explicit non-claims,
- next-stage recommendations,
- real tool execution status.

## CI anti-patterns

- Do not rely on docs-only claims for runtime pass.
- Do not require expensive jobs for docs-only PRs unless they touch lock/evidence contract docs.
- Do not let skipped jobs count as release evidence.
- Do not cut a runtime lock from unclassified moving-main deltas.
- Do not accept frontend route parity drift when backend route contracts changed.
