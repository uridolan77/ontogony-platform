# CONEXUS-STREAM-COST-001 — Close streaming usage/cost observability gaps

**Priority:** P1  
**Repo:** conexus-dotnet  
**Theme:** Telemetry integrity

## Problem

Streaming token usage may be unavailable depending on provider chunks; this should be visible rather than silently incomplete.

## Scope

Log/metric when streaming usage is missing; support final usage chunks where providers emit them; surface cost_unknown reason in model-call telemetry.

## Acceptance criteria

Streaming model calls have either cost values or explicit cost_unknown reason visible in admin/API telemetry.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
