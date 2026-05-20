# FE-AUTH-001 — Add route-level operator auth guard

**Priority:** P0  
**Repo:** ontogony-frontend  
**Theme:** Operator access

## Problem

The route tree mounts all operator pages under OntogonyShell with no explicit guard.

## Scope

Add ProtectedRoute or shell-level auth boundary, local credential/token settings, unauthorized state, and tests.

## Acceptance criteria

Without configured local token/session, operator routes render an auth-required state; authenticated local settings allow access.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
