# SYS-TIGHT-008 — Production-readiness separation package

**Repo:** ontogony-platform, all services  
**Type:** planning + safety docs  
**Priority:** P2

## Goal

Prevent alpha tight integration from being mistaken for production readiness.

## Scope

- Separate backlog for auth, secrets, durable artifacts, observability, real tool execution, deployment hardening.
- Link non-claims from all closeouts.

## Acceptance

- Every release closeout includes non-claims.
- Real tool execution remains blocked.
- Service tokens/project keys are explicitly alpha.
