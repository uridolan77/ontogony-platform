# QA Checklist — RELEASE-READINESS-TRUTH-001

## Manual QA

Open `/system/release-readiness` with the current generated artifact.

Verify:

- [ ] First screen clearly says this is generated/artifact-based unless live validation exists.
- [ ] Release-candidate posture is separate from route artifact counts.
- [ ] Summary cards do not imply that the whole system is release-ready.
- [ ] Generated timestamp and artifact path/command are visible.
- [ ] `fixture_only` routes are not green release-ready rows.
- [ ] `unknown` source rows are not treated as harmless partials.
- [ ] Partial rows show reasons and next actions.
- [ ] The operator can tell what to do next.

## Scenario QA

Use or create test fixtures for these cases:

### Fresh generated artifact

- [ ] Shows `fresh` or equivalent.
- [ ] Still does not claim live release readiness without live validation.

### Stale generated artifact

- [ ] Shows warning.
- [ ] Recommends `npm run readiness:sync`.

### Future generated artifact

- [ ] Shows warning.
- [ ] Does not treat future timestamp as fresh.

### Missing generated timestamp

- [ ] Shows unknown freshness.
- [ ] Does not crash.

### Fixture-only ready route

- [ ] Shows demo/generated-only impact.
- [ ] Does not count as release-ready.

### Unknown source route

- [ ] Shows unresolved.
- [ ] Offers classification next action.

### Live-with-fallback ready route

- [ ] Shows fallback disclosure.
- [ ] Does not silently imply release-ready.

## Regression QA

- [ ] Existing route rows still render.
- [ ] Existing generated artifact still loads.
- [ ] Existing navigation to `/system/release-readiness` works.
- [ ] No unrelated System pages regress visually.
- [ ] Typecheck/build/lint pass where configured.
