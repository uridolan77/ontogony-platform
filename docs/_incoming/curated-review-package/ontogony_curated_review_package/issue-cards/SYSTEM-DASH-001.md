# SYSTEM-DASH-001 — Create starter dashboard and SLO pack for the three-node runtime

**Priority:** P1  
**Repo:** ontogony-platform or allagma-dotnet docs; dashboards may span all repos  
**Theme:** Operations

## Problem

Metrics and evidence are present in pieces, but operators need a single health/readiness/failure view.

## Scope

Dashboards for service readiness, request rates, downstream failures, idempotency conflicts, human-gate waiting count, Conexus fallback count, model-call latency/cost, and restart/replay health.

## Acceptance criteria

A local Grafana/importable dashboard or documented panel pack can be loaded against the local stack.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
