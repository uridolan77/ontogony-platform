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

## REPLAY-RUNTIME-002 (cross-service wiring)

| Route | Owner | Purpose |
| --- | --- | --- |
| `POST /ontology/v0/replay/eligibility` | Kanon | Semantic replay eligibility |
| `POST /admin/v0/replay/eligibility` | Conexus | Model-call / route-decision eligibility |
| `POST /admin/v0/replay/model-calls/{id}/dry-run` | Conexus | Evidence-only model-call replay |
| `POST /admin/v0/replay/route-decisions/{id}/dry-run` | Conexus | Route snapshot reconstruction |
| `GET /admin/v0/replay/model-calls/{id}/evidence` | Conexus | Evidence links for bundles |

Allagma orchestration (`POST /allagma/v0/replay/requests`) appends Kanon/Conexus `ReplayServiceAttempt` rows when replaying terminal runs. See `docs/_incoming/_active/REPLAY-RUNTIME-002/IMPLEMENTATION_NOTES.md`.

### Allagma orchestration scope (REPLAY-RUNTIME-002)

What Allagma **calls automatically** when replaying a terminal run:

| Downstream call | Status |
| --- | --- |
| Kanon `POST /ontology/v0/replay/eligibility` + replay bundle list/prepare | **Wired** |
| Conexus `POST /admin/v0/replay/model-calls/{id}/dry-run` | **Wired** (optional; requires Allagma Conexus admin API key) |
| Conexus `POST /admin/v0/replay/route-decisions/{id}/dry-run` | **Not wired** — Conexus route exists; use admin/frontend direct call |
| Conexus `POST /admin/v0/replay/eligibility` | **Merged via Allagma** — frontend calls Allagma eligibility; Conexus admin direct for posture panel dry-run |
| Merged cross-service eligibility on `POST /allagma/v0/replay/eligibility` | **Wired (REPLAY-RUNTIME-004)** |

Repo-specific detail: `allagma-dotnet/docs/contracts/CROSS_SERVICE_REPLAY.md`.

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
