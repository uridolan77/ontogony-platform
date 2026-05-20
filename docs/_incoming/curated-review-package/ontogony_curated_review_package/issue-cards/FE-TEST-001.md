# FE-TEST-001 — Add smoke tests for the highest-value operator pages

**Priority:** P2  
**Repo:** ontogony-frontend  
**Theme:** Regression coverage

## Problem

Critical pages need tests beyond route existence.

## Scope

ConexusChatPage, StartRunWorkbenchPage, HumanGatesPage, RunDetailPage, OperatorSettingsPage, SystemOverviewPage.

## Acceptance criteria

Vitest covers render/loading/error/success state for these pages; optional Playwright smoke covers main navigation.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
