# 05 — Eval detail and export checklist

Primary route: `/allagma/evaluations/{subjectEvaluationRunId}`

## Detail rendering checks

- [ ] Route resolves for guided `subjectEvaluationRunId`
- [ ] Header identifiers match guided report
- [ ] Core evaluation profile fields render
- [ ] Evidence journey sections render without placeholder crashes
- [ ] Known-limitation text appears where expected (not hidden)

## Fixture behavior checks (if used)

- [ ] `?evalFixture=` renders fixture banner
- [ ] Fixture details are clearly marked as non-live evidence
- [ ] Unknown fixture ID falls back to live-mode behavior (as documented)

## Evidence export checks

- [ ] Export panel/button is visible
- [ ] Export action produces downloadable artifact or explicit error state
- [ ] Exported content is scoped to this evaluation run
- [ ] Redaction behavior is preserved (no secret leakage)
- [ ] Export failure path shows explicit message, not silent failure

## Integrity checks

- [ ] Evaluation detail values align with dashboard row for same run
- [ ] Links from evaluation detail to run/baseline/replay targets are valid
- [ ] No invalid navigation loops or dead-end links

## Evidence

- [ ] Screenshot: eval detail main card
- [ ] Screenshot: export panel before export
- [ ] Artifact captured: one eval export file (or fail screenshot with reason)
- [ ] Note: redaction/secret check result
