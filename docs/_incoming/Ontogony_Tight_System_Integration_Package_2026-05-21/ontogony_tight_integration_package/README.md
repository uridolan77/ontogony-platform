# Ontogony Tight System Integration Package

**Package date:** 2026-05-21  
**Scope:** `ontogony-platform`, `allagma-dotnet`, `kanon-dotnet`, `conexus-dotnet`, with operator/frontend hooks where they are system-gating.

## Executive purpose

This package turns the current Ontogony alpha governed runtime from a validated multi-repo system into a tighter, auditable, operator-ready integration baseline. It deliberately avoids new product ambition inside Kanon or Conexus. The work belongs mostly in Allagma and Ontogony.Platform because Allagma owns runtime orchestration and Platform owns shared contracts/mechanics/evidence registry.

## Current baseline assumed

- `SYSTEM-ALPHA-006` is the current Docker-local governed runtime baseline.
- Allagma owns `docs/system/SYSTEM_COMPATIBILITY_MATRIX.md`, `ontogony-runtime.lock.json`, and the feature connection matrix.
- Kanon v0 is frozen for integration and represented by `KANON_COMPATIBILITY_MANIFEST.json` v1.1.
- Conexus provides durable gateway features, SSE streaming, route decisions, model-call evidence, route-preview, quota status, and provider fallback.
- Real external tool execution remains blocked and must stay blocked unless the real-tool trust model graduates through explicit safety gates.

## Package contents

| File | Purpose |
|---|---|
| `00_EXECUTIVE_VERDICT.md` | Strategic state assessment and target outcome |
| `01_REPO_STATE_REVIEW.md` | Repo-by-repo current-state review |
| `02_TARGET_ARCHITECTURE.md` | Desired tight integration architecture |
| `03_GAP_REGISTER.md` | Prioritized integration gaps |
| `04_PR_SEQUENCE.md` | Proposed PR sequence and ownership |
| `05_ACCEPTANCE_MATRIX.md` | Gate-by-gate acceptance criteria |
| `06_RUNTIME_LOCK_AND_COMPATIBILITY_SPEC.md` | Runtime lock, package, manifest, and moving-main policy |
| `07_E2E_SCENARIO_SPECS.md` | Required local, Docker, restart, and evidence scenarios |
| `08_OPERATOR_EVIDENCE_SPINE_PLAN.md` | Operator/evidence spine integration plan |
| `09_STREAMING_AND_MODEL_PURPOSE_PLAN.md` | Streaming/model-purpose tightening plan |
| `10_IDENTITY_AND_SECURITY_ROADMAP.md` | Production identity, secrets, and tool execution roadmap |
| `11_OBSERVABILITY_AND_SLO_PLAN.md` | System-level observability plan |
| `12_CI_RELEASE_GATES.md` | CI/release gate design |
| `issue-cards/` | Implementation-ready issue cards |
| `matrices/` | Machine-readable matrices for planning |
| `templates/` | Evidence and closeout templates |
| `scripts/validate-package-structure.ps1` | Lightweight package structure validator |

## Recommended first sprint name

`SYS-TIGHT-001 — Ontogony Tight Integration Baseline`

## Core rule

Do not use this package to add random features. Use it to make the existing runtime graph stricter, easier to validate, and easier to operate.
