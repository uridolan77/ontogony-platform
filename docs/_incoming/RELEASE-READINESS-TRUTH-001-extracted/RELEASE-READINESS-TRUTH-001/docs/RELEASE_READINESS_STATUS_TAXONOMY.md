# Release Readiness Status Taxonomy

## Data source

```text
live
  Data comes from a backend/client call at runtime.

live_with_fallback
  Backend/client data was attempted, but fallback/demo/static values filled some part of the view.

generated_only
  Data comes from a generated artifact, not runtime backend calls.

fixture_only
  Data is static/demo/fixture data and must not support release claims.

unknown
  Data source is not classified. Treat as unresolved.
```

## Artifact status

```text
ready
  The generated artifact says the route/page is ready according to artifact rules.

partial
  The generated artifact says the route/page is incomplete or partly backed.

gap
  The generated artifact says the route/page has a missing implementation/readiness gap.

unknown
  Artifact row status is absent or unrecognized.
```

## Release impact

```text
release_candidate
  Eligible to count toward release posture, only when backed by live validation or explicit accepted release evidence.

needs_live_validation
  Looks good in artifact but cannot be release-ready until live validation exists.

needs_review
  Partial/fallback row that requires operator/developer review.

demo_only
  Fixture/demo/generated-only state. Useful for UI coverage, not release posture.

unresolved
  Unknown/missing classification or contradictory row.

not_release_ready
  Known blocker.
```

## Artifact freshness

```text
fresh
  Generated within 24 hours.

aging
  Generated more than 24 hours but not more than 7 days ago.

stale
  Generated more than 7 days ago.

future
  Generated timestamp is in the future.

unknown
  Missing or invalid timestamp.
```

## Recommended badge severity

```text
release_candidate: success or neutral-success
needs_live_validation: warning/info
needs_review: warning
fixture_only/demo_only: neutral/warning, never success
unresolved: warning/error depending on route importance
not_release_ready: error
```
