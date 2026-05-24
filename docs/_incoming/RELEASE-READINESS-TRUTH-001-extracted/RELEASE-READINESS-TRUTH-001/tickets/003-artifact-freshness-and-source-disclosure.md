# Ticket 003 — Artifact freshness and source disclosure

## Goal

Expose where the readiness artifact came from, when it was generated, and whether it is stale.

## Problem

The current page shows a generated timestamp but does not interpret staleness or clearly separate build/generated artifact time from live service health time.

## Implementation

1. Parse generated timestamp from artifact.
2. Add freshness classification:

```text
fresh   <= 24 hours
aging   > 24 hours and <= 7 days
stale   > 7 days
future  timestamp is after now by more than a small tolerance
unknown invalid/missing timestamp
```

3. Display:

```text
Artifact: docs/generated/operator-release-readiness.json
Generated: <timestamp>
Freshness: <freshness>
Regenerate: npm run readiness:sync
```

4. Warn on stale/future/unknown.

## Acceptance

- [ ] Fresh artifact shows neutral/success informational state.
- [ ] Stale artifact shows warning.
- [ ] Future timestamp shows warning.
- [ ] Missing/invalid timestamp does not crash.
- [ ] Tests cover all freshness classes.
