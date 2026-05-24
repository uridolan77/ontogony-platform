# Test Strategy — RELEASE-READINESS-TRUTH-001

## Unit tests

Target the classification helpers.

Cases:

```text
fixture_only + ready => demo_only
fixture_only + partial => demo_only/needs_review
unknown + ready => unresolved
unknown + partial => unresolved
live_with_fallback + ready => needs_live_validation/needs_review
live + ready + noLiveValidation => needs_live_validation
live + ready + liveValidation => release_candidate
invalid artifact status => unknown/unresolved
```

## Freshness tests

```text
now - 1 hour => fresh
now - 2 days => aging
now - 10 days => stale
now + 1 day => future
invalid => unknown
missing => unknown
```

Use fake timers or inject `now` if the codebase has a convention for deterministic date tests.

## Component/page tests

Verify:

- header copy
- posture panel
- artifact path/timestamp/freshness
- artifact-labeled summary cards
- fixture-only row warning
- unknown row warning
- partial route reason and next action

## Snapshot caution

Avoid brittle full-page snapshots if the codebase prefers behavior/text queries. If snapshots already exist, update them only after asserting the semantic copy changes.
