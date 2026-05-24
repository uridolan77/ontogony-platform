# Acceptance Checklist — RELEASE-READINESS-TRUTH-001

## Page truthfulness

- [ ] The page no longer presents generated route coverage as full release readiness.
- [ ] The page explicitly states that `docs/generated/operator-release-readiness.json` is the source when applicable.
- [ ] The page explicitly states whether live backend validation is present or absent.
- [ ] The page explicitly states the release-candidate posture separately from artifact route counts.
- [ ] `Ready 24 / Partial 8 / Gap 0` or equivalent counts are labeled as artifact counts, not system release readiness.

## Data-source semantics

- [ ] `fixture_only` rows are not counted as release-ready.
- [ ] `unknown` data-source rows are treated as unresolved/gap/needs review.
- [ ] `live_with_fallback` rows disclose fallback use and do not silently imply release readiness.
- [ ] `live` rows are only release-candidate eligible if the page has live validation evidence.

## Route row quality

- [ ] Every route row shows data source.
- [ ] Every route row shows artifact status.
- [ ] Every route row shows release impact or equivalent operator-facing classification.
- [ ] Every partial/unresolved row shows a reason.
- [ ] Every partial/unresolved row shows a next action.
- [ ] `/system/release-readiness` is not shown as release-ready if its data source is fixture/demo/generated only.

## Artifact freshness

- [ ] Generated timestamp is displayed when available.
- [ ] Missing/invalid timestamp is handled.
- [ ] Stale artifact is visibly warned.
- [ ] Future-dated artifact is visibly warned.
- [ ] Regeneration command is shown where appropriate.

## Copy and UX

- [ ] The phrase “RC posture is mechanical, not semantic” is replaced or supplemented by clearer operator wording.
- [ ] The page title/subtitle distinguish route coverage from release readiness.
- [ ] Demo/fixture/generated-only surfaces use warning/neutral styling, not green success styling.
- [ ] No copy implies production readiness.

## Tests

- [ ] View-model/status helper tests cover key impossible/misleading combinations.
- [ ] Page/component tests cover summary cards and route rows.
- [ ] Fixture artifact tests cover stale, fresh, invalid, and future timestamps.
- [ ] Existing tests pass or unrelated failures are documented.

## Documentation

- [ ] Operator docs explain the generated artifact and its limits.
- [ ] Backend/live readiness follow-ups are documented separately.
- [ ] No backend route is invented in frontend code.
