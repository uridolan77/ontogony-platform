# ALLAGMA-RUNTIME-POSTURE-001 — Expose and consume Allagma runtime posture as operator truth source

**Priority:** P1  
**Repo:** allagma-dotnet and ontogony-frontend  
**Theme:** Runtime posture

## Problem

Allagma now has runtime posture contracts describing persistence mode, model purposes, tool execution posture, evaluation posture, downstream config, and contract versions. This should become the operator-facing source of what Allagma can do right now.

## Scope

Finalize posture endpoint docs, route matrix entry, frontend consumption, tests, and evidence. Ensure no secrets are exposed and that real external tool execution remains visibly disabled.

## Acceptance criteria

Frontend/system dashboard can show Allagma posture from live API: persistence, model aliases, streaming flags, real execution disabled, sandbox posture, downstream configured flags, and service/package version info.

## Source anchors

- `allagma-dotnet/src/Allagma.Contracts/AllagmaRuntimePostureContracts.cs`
- `allagma-dotnet/tests/Allagma.Tests/GetAllagmaRuntimePostureServiceTests.cs`

## Implementation notes

- Keep changes additive unless correcting stale docs.
- Do not make production-readiness claims.
- Do not enable real external tool execution.
- Prefer generated inventories and validator scripts over handwritten assertions.
