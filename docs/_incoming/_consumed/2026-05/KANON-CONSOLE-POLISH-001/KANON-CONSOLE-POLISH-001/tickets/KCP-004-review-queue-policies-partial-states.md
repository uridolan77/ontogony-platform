# KCP-004 — Review Queue and Policies partial-state reasons

## Problem

Kanon Review Queue and Policies appear as live/partial in readiness output, but partial states need explicit reasons in the page UI.

## Scope

- Add reason-bearing empty/partial/error states.
- Distinguish empty data from missing route/client/auth/config.
- Add tests.

## Reason codes

Use or map to these concepts:

```text
no_items
not_configured
insufficient_role
backend_route_missing
generated_client_missing
fixture_only
live_fallback
not_in_scope
unknown
```

## Acceptance

- No vague `Partial` status appears without explanation.
- Empty state tells operator what to do next.
- Auth/role issue is not rendered as empty data.
- Backend/client gap is not rendered as empty data.
