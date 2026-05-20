# FE-API-ADAPTER-001 — Add API/adapter contract tests for Conexus, Kanon, and Allagma clients

**Priority:** P1  
**Repo:** ontogony-frontend  
**Theme:** Contract safety

## Problem

The frontend depends on domain adapters hiding backend DTOs; drift should be caught locally.

## Scope

Fixture response samples from live smoke/evidence; adapter tests for status mapping, error mapping, pagination, trace IDs, cost/evidence links, and human-gate states.

## Acceptance criteria

Generated/handwritten client DTO changes fail tests if UI contracts break.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
