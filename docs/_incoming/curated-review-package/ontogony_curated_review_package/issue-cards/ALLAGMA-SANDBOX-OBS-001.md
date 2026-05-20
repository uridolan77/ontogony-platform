# ALLAGMA-SANDBOX-OBS-001 — Make sandbox/simulated execution status explicit in capabilities, events, and docs

**Priority:** P1  
**Repo:** allagma-dotnet  
**Theme:** Operator clarity

## Problem

Operators must distinguish dry-run/simulation/local marker execution from real external effects.

## Scope

Expose execution mode in /capabilities, run events, audit bundle, and frontend-adapter contracts.

## Acceptance criteria

Every run that touches tool execution records mode=symbolic|dry_run|local_sandbox|real_external, with real_external unavailable until the trust model passes.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
