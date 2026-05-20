# CONEXUS-GOV-DRILLDOWN-001 — Add row-level model-call/usage drill-down contracts for the operator console

**Priority:** P1  
**Repo:** conexus-dotnet + ontogony-frontend  
**Theme:** Operator observability

## Problem

Aggregate governance usage exists, but the console needs row-level model-call history, evidence, route decision, usage, and cost drill-downs.

## Scope

Clarify and test admin model-call list/detail, model-call evidence, route-decision detail, and usage/cost query contracts. Align with frontend adapters.

## Acceptance criteria

Frontend Conexus observability page can display recent calls, route, fallback, token/cost, artifact/evidence availability, and trace/run links from live APIs.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
