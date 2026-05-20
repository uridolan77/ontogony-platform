# KANON-CONEXUS-ASSIST-001 — Prove Conexus assistance disabled/mock/local paths, not real-provider rollout

**Priority:** P1  
**Repo:** kanon-dotnet + allagma-dotnet E2E  
**Theme:** Assistance seam

## Problem

Assistance routes are valuable but should remain advisory and deterministic until the local durable stack is stable.

## Scope

Tests for disabled behavior, role/field gating, redaction, draft_only outcome, idempotency, and decision-record emission with mock/local Conexus.

## Acceptance criteria

System E2E verifies one assistance route produces a redacted draft_only decision record and provenance entry.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
