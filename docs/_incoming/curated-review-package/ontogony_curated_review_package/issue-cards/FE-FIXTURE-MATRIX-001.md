# FE-FIXTURE-MATRIX-001 — Create page-by-page live/fallback/fixture matrix and remove misleading demo paths

**Priority:** P1  
**Repo:** ontogony-frontend  
**Theme:** Live vs demo clarity

## Problem

System and Allagma fixtures are marked fallback/demo, while Kanon semantic fixtures still supply mock operator content. Operators need to know what is live.

## Scope

Audit src/*/api, src/*/adapters, pages, and fixtures. Document primary live API, fallback behavior, and demo-only components per route.

## Acceptance criteria

Every route in route-workflow-catalog has dataSource=live|live_with_fallback|fixture_only|not_implemented and the UI labels non-live content clearly.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
