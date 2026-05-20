# SYSTEM-E2E-001 — Add full local-stack E2E suite for the governed runtime loop

**Priority:** P0  
**Repo:** allagma-dotnet primary; exercises kanon-dotnet and conexus-dotnet  
**Theme:** Cross-repo cohesion

## Problem

The first loop exists, but the durable end-to-end system should be proven as one observable runtime, not just individual services.

## Scope

One script/harness starts Kanon, Conexus, Allagma, and Postgres. Scenarios: completed run, idempotent retry, human gate waiting/approved/denied, Kanon→Conexus assistance, Conexus fallback, restart survival, trace/correlation stitching.

## Acceptance criteria

A single command produces artifacts/system-e2e/<timestamp>/summary.json with route responses, event timeline, decision IDs, model call IDs, trace IDs, and pass/fail status.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
