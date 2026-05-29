# AISTHESIS-LIVE-SPINE-RC-READINESS-005 — Cursor unpack prompt

You are working in the Ontogony system.

Package: `AISTHESIS-LIVE-SPINE-RC-READINESS-005`

Recommended unpack target:

```text
C:\Dev\ontogony-platform\docs\_incoming\packages\AISTHESIS-LIVE-SPINE-RC-READINESS-005\
```

Primary implementation repo:

```text
C:\Dev\aisthesis-dotnet
```

Related repos for validation only unless explicitly required by the package:

```text
C:\Dev\allagma-dotnet
C:\Dev\kanon-dotnet
C:\Dev\conexus-dotnet
C:\Dev\metabole-dotnet
C:\Dev\ontogony-frontend
C:\Dev\ontogony-platform
```

## Goal

Take Aisthesis from **strong alpha / pre-RC** to **RC-readiness candidate**.

This package must not add broad new product surface. It hardens the existing live evidence spine:

- full clean Release gates;
- CI-compatible five-service smoke;
- ReleaseMode validation;
- live-spine evidence contract v2;
- LES-001 / LES-002 replay and scoring discipline;
- LES-002 partial-grade closure or accepted justification;
- frontend contract readiness;
- production IAM gate definition;
- retention/erasure operational gate definition;
- OpenTelemetry distributed-trace export gate definition;
- Aisthesis `lockRequired` decision record.

## Baseline

Aisthesis 003A is closed for fixture/harness scope.

Producer alignment 004 has live proof through LES-001:

- producers observed: allagma, kanon, conexus, metabole;
- reconstructability v2: complete;
- score: 0.95;
- blocking findings: 0.

LES-002 exists and passes with zero blockers, but reconstructability is partial at 0.82. This package must either improve LES-002 to complete or document why the partial grade is accepted for the current RC boundary.

## Non-negotiable boundary

Aisthesis owns evidence envelopes, evidence edges, trace timelines, trace bundles, lookup/export, and reconstructability scoring.

Aisthesis does **not** own:

- Kanon semantic truth;
- Conexus model routing/provider selection;
- Allagma workflow execution;
- Metabole profiling/mapping/transformation semantics;
- frontend rendering.

## Execution order

1. Read `01_PACKAGE_MANIFEST.md`.
2. Read `02_CURRENT_STATE_BASELINE.md`.
3. Read `03_SCOPE_AND_BOUNDARY.md`.
4. Read `04_ACCEPTANCE_MATRIX.md`.
5. Read `05_TARGET_FILE_MAP.md`.
6. Execute `06_IMPLEMENTATION_PLAN.md`.
7. Keep all claims synchronized with `18_CLOSEOUT_TEMPLATE.md`.

## Required final deliverables

In `aisthesis-dotnet`:

```text
docs/evidence/AISTHESIS_LIVE_SPINE_RC_READINESS_005_CLOSEOUT.md
docs/evidence/AISTHESIS_LIVE_SPINE_RC_READINESS_005_RELEASE_GATES.md
docs/evidence/AISTHESIS_LIVE_SPINE_RC_READINESS_005_LIVE_PROOF.md
docs/evidence/AISTHESIS_LIVE_SPINE_RC_READINESS_005_LOCK_DECISION.md
docs/operations/AISTHESIS_FIVE_SERVICE_CI_SMOKE_RUNBOOK.md
docs/operations/AISTHESIS_RELEASE_MODE_RUNBOOK.md
docs/contracts/AISTHESIS_LIVE_SPINE_SUMMARY_V2.md
scripts/system/run-aisthesis-rc-readiness.ps1
scripts/system/run-five-service-ci-smoke.ps1
```

In `ontogony-platform`:

```text
docs/evidence/AISTHESIS_LIVE_SPINE_RC_READINESS_005_PLATFORM_CLOSEOUT.md
docs/evidence/SYSTEM_RC_003_AISTHESIS_LOCK_REVIEW_005.md
```

Only modify producer repos if the live proof reveals real emitter regressions.

## Truth rule

Do not mark the package closed unless:

- all Aisthesis tests pass;
- fixture smoke passes;
- LES-001 live path remains complete;
- LES-002 is complete or explicitly justified;
- CI smoke harness exists;
- ReleaseMode gate exists;
- `lockRequired` decision is documented with evidence.

If some gates remain deferred, close as **RC-readiness partial**, not **RC-ready**.
