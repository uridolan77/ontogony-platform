# Ticket 002 — Readiness view model and status classification

## Goal

Centralize the mapping from raw generated artifact rows to truthful operator-facing release-readiness state.

## Problem

Raw row fields such as `dataSource=fixture_only` and `readiness=ready` can be technically true for page rendering but false for release posture.

## Implementation

Add or update a helper/module that maps raw artifact rows into a route view model.

Required classifications:

```text
artifactStatus: ready | partial | gap | unknown
releaseImpact: release_candidate | needs_live_validation | demo_only | unresolved | not_release_ready
isBlocking: boolean
reason: string
nextAction: string
```

Map rules:

```text
fixture_only + ready      => demo_only
unknown + any             => unresolved
live_with_fallback + ready => needs_live_validation or needs_review
live + ready              => release_candidate only if live validation exists
partial                   => needs_review with reason
```

Use repo naming/style conventions.

## Acceptance

- [ ] Helper has unit tests.
- [ ] Fixture-only ready row is not release-ready.
- [ ] Unknown source row is unresolved.
- [ ] Live-with-fallback row exposes fallback status.
- [ ] Component uses the helper instead of ad-hoc conditionals.
