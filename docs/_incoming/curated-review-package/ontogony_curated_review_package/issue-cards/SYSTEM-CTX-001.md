# SYSTEM-CTX-001 — Standardize end-to-end trace, correlation, actor, and idempotency propagation

**Priority:** P0  
**Repo:** allagma-dotnet, kanon-dotnet, conexus-dotnet, ontogony-frontend  
**Theme:** Traceability

## Problem

The system value depends on linking Allagma events, Kanon decisions, and Conexus telemetry without leaking sensitive payloads.

## Scope

Document and enforce traceparent, X-Correlation-ID, X-Ontogony-Actor-Id, X-Ontogony-Actor-Type, X-Ontogony-Actor-Roles, Idempotency-Key, X-Allagma-Run-Id. Add tests for propagation across Client→Allagma→Kanon→Conexus.

## Acceptance criteria

Trace/correlation IDs appear in Allagma events, Kanon decision records, and Conexus telemetry/model-call records for the same local-stack run.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
