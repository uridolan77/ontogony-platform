# KANON-AUTH-LOCAL-001 — Harden and document local service-token auth path while preserving dev headers

**Priority:** P1  
**Repo:** kanon-dotnet  
**Theme:** Security posture

## Problem

Trusted headers are acceptable for local developer convenience but should not be the only proven path.

## Scope

Add local service-token smoke, docs comparing DevelopmentTrustedHeaders vs ServiceToken, and config validation examples.

## Acceptance criteria

Local stack can run Kanon in ServiceToken mode and Allagma/Kanon client tests still pass.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
