# FE-ERRBOUND-001 — Add route-level error boundaries and API failure states

**Priority:** P0  
**Repo:** ontogony-frontend  
**Theme:** UX resilience

## Problem

Live API pages should fail per-page, not crash or silently fall back.

## Scope

Shared ErrorBoundary, ProductLiveQueryState usage where missing, retry actions, redaction-safe error rendering.

## Acceptance criteria

Each domain has a page-level error state for failed backend calls; tests cover at least one failure per domain.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
