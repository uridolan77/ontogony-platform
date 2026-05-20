# CONEXUS-RETENTION-001 — Strengthen retention policy evidence and maintenance run visibility

**Priority:** P2  
**Repo:** conexus-dotnet  
**Theme:** Data lifecycle

## Problem

Retention cleanup is hosted in the API, but operators need evidence of what policies apply and what was deleted or retained.

## Scope

Expose retention policy readback, last run status, counts, and dry-run mode. Add docs and tests.

## Acceptance criteria

Admin can query retention config and last cleanup summary; local test proves expired records are handled as configured.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
