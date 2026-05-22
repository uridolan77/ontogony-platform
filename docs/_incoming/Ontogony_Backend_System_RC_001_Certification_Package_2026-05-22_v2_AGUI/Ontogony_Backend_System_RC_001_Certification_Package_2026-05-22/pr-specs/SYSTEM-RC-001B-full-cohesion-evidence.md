# SYSTEM-RC-001B — Full Cohesion Evidence

## Owner

`allagma-dotnet`

## Problem

The system has multiple smoke scripts and evidence paths, but above-9 certification requires one full cohesion run that exercises the meaningful cross-repo edges, including Kanon assistance, Conexus fallback, and streaming evidence.

## Goal

Produce a full sibling-source cohesion evidence run for the pinned runtime lock.

## Required command

With Kanon, Conexus, and Allagma running on default ports:

```powershell
cd C:\dev\allagma-dotnet
pwsh ./scripts/run-system-cohesion-smoke.ps1 -UseExistingServices `
  -IncludeKanonAssistance -IncludeConexusFallback -IncludeStreamingEvidence
```

## Required artifacts

```text
artifacts/system-e2e/<timestamp>/summary.json
docs/evidence/SYSTEM_RC_001B_FULL_COHESION_EVIDENCE.md
```

## Minimum scenarios

The summary must prove:

```text
correlation_chain
completed_run
idempotent_run_start
human_gate_waiting
human_gate_approved
human_gate_denied
kanon_conexus_assistance
conexus_fallback
streaming_evidence
restart_survival_reference_or_link
```

If restart survival is not executed inside this same script, link the current restart-survival artifact and mark it as a required sibling gate, not optional.

## Acceptance criteria

- Every scenario has PASS/FAIL status.
- No scenario is “not run” unless explicitly excluded with a justified reason.
- Evidence summary path is recorded in `RELEASE_EVIDENCE_INDEX.md`.
- Failed scenarios are not converted to docs-only follow-ups.
- Streaming evidence is included and not optional.

## Non-goals

- No new feature behavior unless required to fix a failing certification gate.
- No production claims.
- No real tool execution.
