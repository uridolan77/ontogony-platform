# FE-STUBS-001 — Replace thin/stub pages with honest operational states or live wiring

**Priority:** P1  
**Repo:** ontogony-frontend  
**Theme:** Operator trust

## Problem

Routes such as Kanon plans/facts/provenance and Allagma replay are operator-important and should not appear silently incomplete.

## Scope

For each thin page, either wire live data or render a standard NotYetImplemented/ComingSoon panel with route purpose and backend dependency.

## Acceptance criteria

No released route renders a near-empty page without clear status and next action.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
