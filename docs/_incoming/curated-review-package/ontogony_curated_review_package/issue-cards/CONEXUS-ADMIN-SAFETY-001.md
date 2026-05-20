# CONEXUS-ADMIN-SAFETY-001 — Add explicit tests and docs for sensitive admin/diagnostic endpoint exposure

**Priority:** P1  
**Repo:** conexus-dotnet  
**Theme:** Admin safety

## Problem

Admin middleware exists, but routes such as DbViewer and diagnostics should have clear non-production safety proof.

## Scope

Tests proving admin auth/rate limiting covers DbViewer, diagnostics, retention, provider config, and evidence endpoints. Docs explain intended environment exposure.

## Acceptance criteria

Unauthenticated and project-key-only callers cannot reach admin/diagnostic endpoints; production config validation documents expected controls.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
