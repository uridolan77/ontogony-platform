# Eval-Based Cross-Repo Development Package

**Program:** Ontogony eval-based runtime hardening  
**Date:** 2026-05-18  
**Repos in scope:**

- `uridolan77/ontogony-platform`
- `uridolan77/kanon-dotnet`
- `uridolan77/conexus-dotnet`
- `uridolan77/allagma-dotnet`

## Purpose

This package converts the “build optimal agents that actually work” lesson into an implementation-ready cross-repo plan.

The goal is **not** to add agent swarms. The goal is to make the Ontogony stack:

1. classify task structure before execution,
2. default to the simplest reliable topology,
3. authorize topology choices through Kanon,
4. record route/model decisions through Conexus,
5. evaluate outcomes against baselines,
6. expose machine-readable evidence for operators and future UI surfaces.

## Core principle

```text
Default to single-workflow execution.
Escalate topology only when task structure, policy, and eval evidence justify it.
```

## Package layout

```text
.
├── 00_EXECUTIVE_BRIEF.md
├── 01_CURRENT_STATE_REVIEW.md
├── 02_TARGET_ARCHITECTURE.md
├── 03_CROSS_REPO_PHASE_ROADMAP.md
├── 04_ACCEPTANCE_MATRIX.md
├── 05_DEPENDENCY_AND_BOUNDARY_RULES.md
├── 06_EVAL_METRICS_AND_SCORING_MODEL.md
├── 07_EVENT_AND_TRACE_MODEL.md
├── 08_SECURITY_AND_SAFETY_GATES.md
├── 09_OPERATOR_RUNBOOK.md
├── 10_IMPLEMENTATION_SEQUENCE.md
├── pr-specs/
├── prompts/
├── schemas/
├── examples/
├── templates/
└── scripts/
```

## Execution policy

Implement in thin, testable PRs. Do not combine contracts, runtime behavior, persistence, and UI into one large PR.

Every PR must provide:

- build/test proof,
- contract tests,
- evidence artifacts,
- updated docs,
- boundary/conformance guards where relevant.

## Non-goals

- No autonomous multi-agent swarm.
- No real external tool execution enablement.
- No provider routing semantics in Kanon.
- No semantic authority in Conexus.
- No product meaning in Ontogony.Platform.
- No Allagma-specific workflow semantics in Ontogony.Platform.
