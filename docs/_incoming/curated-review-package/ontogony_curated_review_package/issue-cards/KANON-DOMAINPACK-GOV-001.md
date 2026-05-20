# KANON-DOMAINPACK-GOV-001 — Clarify domain-pack lifecycle promotion states and blocking rules

**Priority:** P2  
**Repo:** kanon-dotnet  
**Theme:** Governance

## Problem

Domain packs are central artifacts; lifecycle governance should be explicit enough for operators and CI.

## Scope

Document and enforce dev/reviewed/accepted/active/deprecated semantics, promotion blockers, signature/hash requirements, and rollback/deprecation behavior.

## Acceptance criteria

Domain-pack lifecycle docs and tests cover promote, active lookup, deprecate, blocked promotion, hash mismatch, and signature failure.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
