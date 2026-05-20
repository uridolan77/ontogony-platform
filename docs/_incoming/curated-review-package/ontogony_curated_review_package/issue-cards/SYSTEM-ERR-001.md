# SYSTEM-ERR-001 — Standardize cross-service error envelope and typed client failures

**Priority:** P0  
**Repo:** ontogony-platform first; consumed by allagma-dotnet, kanon-dotnet, conexus-dotnet, ontogony-frontend  
**Theme:** Cross-service contracts

## Problem

Mixed local error shapes make downstream UX and automated retries fragile.

## Scope

Define shared error envelope with code, message, stage, system, downstreamSystem, traceId, correlationId, retryable, and detail. Map Conexus provider/quota/idempotency failures, Kanon policy/human-gate failures, and Allagma orchestration failures.

## Acceptance criteria

Kanon.Client and Conexus.Client expose deterministic typed failures; Allagma translates downstream failures into the shared envelope; frontend adapters can render a single error model.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
