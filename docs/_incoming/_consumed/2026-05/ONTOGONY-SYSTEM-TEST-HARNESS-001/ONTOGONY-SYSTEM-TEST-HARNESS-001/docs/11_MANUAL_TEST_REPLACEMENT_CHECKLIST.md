# Manual Test Replacement Checklist

Use this to decide whether a manual check can be retired.

A manual regression check can be replaced by automation only when:

- [ ] The automated test covers the same user/system behavior.
- [ ] The assertion checks outcomes, not only status code.
- [ ] The test fails for the actual regression humans are worried about.
- [ ] The test is deterministic enough for local and CI use.
- [ ] The evidence bundle is readable by a developer/operator.
- [ ] The test has clear owner and severity.
- [ ] The test is included in a standard command/workflow.
- [ ] The manual tester agrees that the repeated check is now redundant.

## Manual checks that should remain manual

- Exploratory UX review.
- Product/semantic judgment.
- New feature acceptance before behavior stabilizes.
- Philosophical/modeling coherence of Aisthesis/Kanon semantics.
- Visual polish until visual regression tooling is established.
- Security review for new trust boundaries.

## Replacement log

| Manual check | Automated test ID | Replaced? | Notes |
|---|---|---:|---|
| Local stack services start | READINESS-* | **yes** | hard fail on `/health` and `/ready` for 5081–5083 |
| Allagma governed run completes | E2E-001 | **yes** | milestone events + Kanon/Conexus refs |
| Idempotent run replay/conflict | E2E-002, E2E-002b | **yes** | |
| Human gate pause/resume | E2E-003a, E2E-003b | **yes** | requires gaming-core + PaymentsOperator |
| Kanon assistance draft_only | E2E-004 | **yes** | fails if assistance disabled (503) |
| Conexus fallback works | E2E-005 | **yes** | fake providers only |
| UI page loads for each service | UI-* | partial | needs current frontend route calibration |

See [generated/MANUAL_REGRESSION_RETIREMENT_REPORT.md](generated/MANUAL_REGRESSION_RETIREMENT_REPORT.md) for full retirement matrix.
