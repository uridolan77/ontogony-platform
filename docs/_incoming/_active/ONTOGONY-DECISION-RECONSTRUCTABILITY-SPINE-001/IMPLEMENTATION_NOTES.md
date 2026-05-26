# ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001 — active (platform)

**Status:** in_progress  
**Tracked in:** [`../MANIFEST.md`](../MANIFEST.md)

## Layout

This package uses a **nested Cursor export** layout:

```text
ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001/          ← you are here (working notes)
  ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001/      ← inner package (prompts, contracts, tasks)
```

Start from the inner folder: [`./ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001/README.md`](./ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001/README.md) or [`00_UNPACK_PROMPT.md`](./ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001/00_UNPACK_PROMPT.md).

Repo tasks: [`./ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001/10_prompts/CURSOR_TASKS_BY_REPO.md`](./ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001/10_prompts/CURSOR_TASKS_BY_REPO.md).

## Platform slice (Task 5)

After Kanon / Allagma / Conexus / frontend slices land:

1. Promote protocol docs to `docs/contracts/` (DecisionEvent, ReconstructabilityReport, F/P/S/O semantics).
2. Optional: `scripts/validate-decision-reconstructability-local.ps1` if aligned with repo conventions.
3. **Move this entire folder** to `docs/_incoming/_consumed/<YYYY-MM>/` per [`../README.md`](../README.md).

## Working log

### 2026-05-26 — Spine connected (2A/2B + frontend)

- Allagma: `GET /allagma/v0/runs/{runId}/decision-events`
- Kanon: `POST /ontology/v0/reconstructability/classify-batch`
- Frontend: panel + OpenAPI-generated DTOs + `@ontogony/ui` report panel

### 2026-05-26 — DEC-RECON-004 started

Cross-repo docker-local smoke: see [`../ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-004/README.md`](../ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-004/README.md).

### 2026-05-26 — DEC-RECON-005 golden fixtures

Five canonical JSON fixtures under `ontogony-platform/fixtures/decision-reconstructability/` with Kanon classifier tests. See [`../ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-005/README.md`](../ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-005/README.md).
 