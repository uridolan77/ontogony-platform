# KANON-POSTGRES-LOCAL-001 — Make PostgreSQL the proven local acceptance path

**Priority:** P0  
**Repo:** kanon-dotnet  
**Theme:** Durable local path

## Problem

Kanon defaults to in-memory persistence and development-trusted headers; durable local behavior must be proven before broader cross-repo claims.

## Scope

Docker/Postgres smoke covering ontology lifecycle, source bindings, semantic plan compile, human gate, decision/provenance, replay bundle, and domain pack flows.

## Acceptance criteria

A local script runs migrations and verifies the major workflows persist across process restart.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
