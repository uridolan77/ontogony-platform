# AISTHESIS-LIVE-SPINE-RC-CERTIFICATION-006 — Cursor unpack prompt

You are working in the Ontogony system.

This package integrates two inputs:

1. The uploaded package `AISTHESIS-LIVE-SPINE-RC-READINESS-005`, which focused on RC-readiness gates, ReleaseMode discipline, LES-001/LES-002 evidence, CI smoke, lock decision, IAM/retention/OTel gates, and frontend handoff.
2. The follow-up architectural review of `aisthesis-dotnet`, which identified the remaining gaps needed to turn Aisthesis into the real five-service evidence certification point: true live five-service certification, required-edge matrix v2, durable evaluation/Krisis, client coverage completion, producer edge auth hardening, retention/erasure implementation path, OTel trace export, and frontend validation.

Recommended unpack target:

```text
C:\Dev\ontogony-platform\docs\_incoming\packages\AISTHESIS-LIVE-SPINE-RC-CERTIFICATION-006```

Primary implementation repo:

```text
C:\Dev\aisthesis-dotnet
```

Related repos:

```text
C:\Dev\allagma-dotnet
C:\Dev\kanon-dotnet
C:\Dev\conexus-dotnet
C:\Dev\metabole-dotnet
C:\Dev\ontogony-frontend
C:\Dev\ontogony-platform
```

## Goal

Take Aisthesis from **strong alpha / pre-RC evidence spine** to a **live five-service RC-certification candidate**.

This package supersedes `AISTHESIS-LIVE-SPINE-RC-READINESS-005` but preserves its truth discipline. It expands the package where 005 intentionally stopped:

- real live five-service certification, not only fixture/NOT_RUN harnesses;
- required-edge matrix v2 beyond the 10-rule minimum;
- evaluation/Krisis durability instead of fire-and-forget evaluation;
- complete client coverage or explicit server-only classification;
- hardened producer and edge authorization;
- retention/erasure implementation plan with tombstone/audit semantics;
- OpenTelemetry distributed trace export path;
- frontend live evidence-spine contract validation;
- platform-level lock review and system RC evidence.

## Non-negotiable boundary

Aisthesis owns:

```text
evidence envelopes
evidence edges
trace timelines
trace graphs
trace bundles
lookup/export
reconstructability scoring
evaluation/Krisis reports over evidence completeness
```

Aisthesis does **not** own:

```text
Kanon semantic truth
Conexus model routing/provider selection
Allagma workflow execution/tool authority
Metabole profiling/mapping/transformation semantics
frontend rendering
business/product decisions
```

## Execution order

1. Read `01_PACKAGE_MANIFEST.md`.
2. Read `02_INTEGRATED_BASELINE.md`.
3. Read `03_SOURCE_PACKAGE_INTEGRATION_MAP.md` to understand how RC-readiness-005 is absorbed.
4. Read `04_SCOPE_AND_BOUNDARY.md`.
5. Read `05_ACCEPTANCE_MATRIX.md`.
6. Read `06_TARGET_FILE_MAP.md`.
7. Execute `07_IMPLEMENTATION_PLAN.md` slice by slice.
8. Use `22_CLOSEOUT_TEMPLATE.md` for final closeout.

## Closure rule

Do not mark this package **RC-certification candidate** unless:

- Aisthesis Release restore/build/test passes;
- Aisthesis fixture smoke passes;
- LES-001 remains complete or its replacement live trace is complete;
- LES-002 is complete or accepted partial with exact rationale;
- a live five-service certification script exists and produces honest PASS/NOT_RUN/FAIL summaries;
- required-edge matrix v2 exists and is tested by fixtures;
- evaluation route client coverage is complete or explicitly server-only;
- edge authorization hardening is implemented or explicitly deferred with a blocker label;
- lock decision doc states evidence, blockers, deferrals, and recommendation.

If live orchestration remains unavailable, close as **RC-certification partial**, not RC-ready.
