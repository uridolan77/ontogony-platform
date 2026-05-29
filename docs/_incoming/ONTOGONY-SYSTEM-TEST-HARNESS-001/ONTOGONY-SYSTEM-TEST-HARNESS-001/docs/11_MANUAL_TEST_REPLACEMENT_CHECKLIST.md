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
| Local stack services start | READINESS-* | yes candidate | after route calibration |
| Allagma governed run completes | E2E-001 | yes candidate | high priority |
| Human gate pause/resume | E2E-003 | yes candidate | high priority |
| Conexus fallback works | E2E-005 | yes candidate | high priority |
| UI page loads for each service | UI-* | partial | needs current frontend route calibration |
