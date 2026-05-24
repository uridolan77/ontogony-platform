# Replay runtime contract (REPLAY-RUNTIME-001)

**Status:** v1 (`ontogony-replay-runtime-v1`)  
**Package:** `Ontogony.Replay.Contracts`  
**Schemas:** `schemas/contracts/replay-*-v1.schema.json`

## Purpose

Ontogony replay is a family of evidence-grounded operations with different truth claims. This contract defines the **shared vocabulary** used when Allagma orchestrates replay across Kanon and Conexus without implying exact reproducibility where only reconstruction exists.

## Replay modes

| Mode | Meaning |
| --- | --- |
| `exact_replay` | Re-execute with proven identical inputs and environment. Rare; must be explicitly proven. |
| `deterministic_simulation` | Re-run logic against pinned snapshots (fake/local providers, semantic snapshots). |
| `dry_run` | Evaluate routing/policy without provider or tool side effects. |
| `reconstructed` | Rebuild timelines/views from stored events and exports. |
| `evidence_only` | Export manifests, bundles, and fingerprints without execution. |
| `unavailable` | Target cannot be replayed in the requested sense. |

Legacy Allagma run replay `manifest_only` maps to `evidence_only` with `legacyMode = manifest_only`.

## Target kinds

See `ReplayTargetKind` in `Ontogony.Replay.Contracts` and `replay-runtime-v1.schema.json`.

## Core documents

| Document | Schema | Owner |
| --- | --- | --- |
| Target | `replay-target-v1` | Resolver (Allagma/frontend) |
| Eligibility | `replay-eligibility-v1` | Service owning target |
| Request | `replay-request-v1` | Allagma orchestration |
| Result | `replay-result-v1` | Allagma orchestration |
| Delta | `replay-delta-v1` | Allagma orchestration |
| Evidence bundle | `replay-evidence-bundle-v1` | Allagma export |

## Safety defaults

All operator replay flows default to:

- `providerExecutionPolicy = forbid_real_providers`
- `toolExecutionPolicy = forbid_real_tools`
- `redactionPolicy = operator_default`

Real provider or tool execution requires an explicit future trust model; it is **out of scope** for REPLAY-RUNTIME-001.

## Evidence Spine links

Replay artifacts use stable reference kinds:

- `replay.request`
- `replay.result`
- `replay.delta`
- `replay.evidence_bundle`

See [`EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md`](../operators/EVIDENCE_SPINE_IDENTIFIER_TAXONOMY.md).

## Service boundaries

| Concern | Owner |
| --- | --- |
| Shared vocabulary/schemas | Ontogony Platform |
| Replay orchestration record | Allagma |
| Semantic replay bundle | Kanon |
| Model-call/route-decision evidence | Conexus |
| Operator workflow | Ontogony Frontend |
