# ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-007

**Title:** DEC-RECON-007 — persisted reconstructability report artifacts  
**Status:** closed (2026-05-26 closeout)  
**Slice:** DEC-RECON-007  
**Depends on:** DEC-RECON-004 (live smoke), DEC-RECON-006 (Evidence Spine graph)

## Goal

Turn on-demand reconstructability classification into **persisted, versioned, hash-addressable** reconstructability report artifacts so operators can answer:

```text
What did Kanon classify then?
From which exact decision-event input?
Under which classifier version?
What report did it produce?
```

## Current state

| Slice | Status | Capability |
| --- | --- | --- |
| DEC-RECON-004 | **closed** | Live Allagma `decision-events` → Kanon `classify-batch` → run audit panel |
| DEC-RECON-006 | **closed** | Evidence Spine `allagma.decisionEvent` nodes + `ReconstructabilityReportPanel` |
| DEC-RECON-007 | **closed** | Persisted report artifacts + live smoke |

**Gap:** The stack shows what Kanon says **now**, but cannot prove what Kanon said **then** from the exact input under a recorded classifier version.

## Design principle

```text
Keep classify / classify-batch as live on-demand classification.
Add report-artifacts / report-artifacts/batch as the persisted, auditable path.
```

Do **not** break DEC-RECON-004 or DEC-RECON-006 on-demand paths.

## Package docs

| File | Purpose |
| --- | --- |
| [00_IMPLEMENTATION_PROMPT.md](./00_IMPLEMENTATION_PROMPT.md) | Cross-repo implementation sequence |
| [01_ACCEPTANCE_CHECKLIST.md](./01_ACCEPTANCE_CHECKLIST.md) | Gate checklist |
| [02_ARTIFACT_CONTRACT.md](./02_ARTIFACT_CONTRACT.md) | `ontogony-reconstructability-report-artifact-v1` |
| [03_VERIFICATION_PLAN.md](./03_VERIFICATION_PLAN.md) | Tests, smoke, evidence paths |

## Canonical artifact schema

```text
ontogony-reconstructability-report-artifact-v1
```

## Repo ownership

| Repo | Responsibility |
| --- | --- |
| `kanon-dotnet` | Contracts, hashing, classifier version, persistence, API endpoints, tests |
| `ontogony-frontend` | Progressive enhancement in Evidence Spine + run reconstruction (artifact metadata when available; classify-batch fallback) |
| `ontogony-platform` | Smoke/verification scripts, `dec-recon-007-smoke-report.json`, evidence closeout |

## Out of scope

- Conexus decision-event export (CONEXUS-DECISION-EVENTS-001)
- Hidden chain-of-thought storage or exposure
- Breaking existing reconstructability routes or DEC-RECON-004/006 behavior

## Working log

### 2026-05-26 — intended implementation sequence

1. **Platform (this package)** — intake docs, artifact contract, verification plan *(done)*
2. **Kanon** — artifact contracts, `DecisionEventCanonicalHasher`, classifier version, persistence, `report-artifacts` endpoints, tests *(done 2026-05-26 — see `kanon-dotnet/docs/evidence/ONTOGONY_DECISION_RECONSTRUCTABILITY_SPINE_007_KANON_EVIDENCE.md`)*
3. **Frontend** — optional artifact batch on spine load; show `artifactId` / `decisionEventHash` / `classifierVersion` / `classifiedAtUtc` when present; keep classify-batch fallback *(done 2026-05-26 — see `ontogony-frontend/docs/evidence/ONTOGONY_DECISION_RECONSTRUCTABILITY_SPINE_007_EVIDENCE.md`)*
4. **Closeout** — cross-repo verification, `dec-recon-007-smoke-report.json`, evidence markdown in each repo *(done 2026-05-26)*

### 2026-05-26 — closeout proof

| Gate | Result |
| --- | --- |
| Kanon build + Reconstructability tests (75) | PASS |
| Frontend openapi:check / build / unit | PASS |
| Migration `023` on `kanon_local` | Applied (`reconstructability_report_artifact` table present) |
| Playwright `decision-reconstructability-docker-live.spec.ts` | PASS (4/4) |

**Smoke report:** [`docker/local-working-system/artifacts/dec-recon-007-smoke-report.json`](../../../docker/local-working-system/artifacts/dec-recon-007-smoke-report.json) — `artifactCount=11`, `classificationPath=persisted-artifacts`, `verdict=PASS`.

**Evidence:** `kanon-dotnet/docs/evidence/ONTOGONY_DECISION_RECONSTRUCTABILITY_SPINE_007_KANON_EVIDENCE.md`, `ontogony-frontend/docs/evidence/ONTOGONY_DECISION_RECONSTRUCTABILITY_SPINE_007_EVIDENCE.md`.

**Next track:** CONEXUS-DECISION-EVENTS-001. Optional hygiene: promote this package folder to `docs/_incoming/_consumed/2026-05/` per intake README.

### Notes

- Cursor prompts for slices 2–4 live in [`../ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-006/ODR-007.md`](../ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-006/ODR-007.md) (Prompts 2–4). This package is the canonical intake home for DEC-RECON-007.
- DEC-RECON-004 and DEC-RECON-006 packages were **not** moved; they remain in `_active/` stubs or `_consumed/2026-05/` per existing archive state.
