# ALLAGMA-TOOL-TRUST-001 — Keep real tool execution blocked until the real-tool trust model is complete

**Priority:** P0  
**Repo:** allagma-dotnet  
**Theme:** Safety / execution boundaries

## Problem

Allagma has simulated/local sandbox execution surfaces; external side effects need a stronger trust boundary before activation.

## Scope

Add docs/security/REAL_TOOL_EXECUTION_TRUST_MODEL.md, docs/architecture/REAL_TOOL_EXECUTION_CONTRACT.md, docs/contracts/REAL_TOOL_EXECUTION_EVENTS.md, docs/runbooks/REAL_TOOL_EXECUTION_OPERATOR_RUNBOOK.md.

## Acceptance criteria

Design covers deny-by-default registry, per-tool permissions, secret scope, outbound/filesystem allowlists, human gate policy, durable side-effect ledger, no-reexecution replay rule, timeout/cancellation, and evidence export.

## Implementation notes

Keep the PR small. Prefer additive contracts, tests, and docs before behavior expansion. Do not enable real external tool execution as a side effect of any cohesion or frontend work.
