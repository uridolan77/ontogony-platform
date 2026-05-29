# Cursor unpack prompt — ONTOGONY-BACKEND-COORDINATION-002

You are implementing the **Ontogony Backend Coordination Sprint** — eight ordered slices that close post-cleanup cohesion gaps across the six backend repositories.

## Operating mode

Be conservative, evidence-driven, and boundary-aware. This is a **cohesion sprint**, not a feature free-for-all.

Before editing anything:

1. Inspect sibling checkouts under the dev root (default `C:\dev`):
   - `ontogony-platform`
   - `conexus-dotnet`
   - `kanon-dotnet`
   - `allagma-dotnet`
   - `metabole-dotnet`
   - `aisthesis-dotnet`
2. Read the cleanup baseline:
   - `allagma-dotnet/docs/system/BACKEND_REPO_CLEANUP_ORGANIZATION_001_SUMMARY.md`
   - `allagma-dotnet/docs/system/BACKEND_REPO_BOUNDARY_MATRIX.md`
   - Each repo's `docs/status/*_CLEANUP_ORGANIZATION_STATUS.md`
3. Verify what already exists — **do not duplicate** matrices, contracts, or validators from `SYSTEM-COH-001`, `ALLAGMA-COHESION-001`, or producer-alignment-004. Extend, refresh, and close gaps only.

## Hard boundaries (non-negotiable)

```text
Platform  owns mechanics only — not product semantics
Conexus   owns model access — not ontology or execution
Kanon     owns semantic authority — not routing or workflows
Allagma   owns governed execution — not meaning or provider selection
Metabole  owns data spine — not canonical truth
Aisthesis owns evidence/reconstructability — not runtime ownership
```

- Do **not** enable real external tool execution.
- Do **not** add provider SDKs to Kanon or Allagma core.
- Do **not** move semantic policy into Conexus or Allagma.
- Do **not** hard-code provider model IDs in Allagma — use Conexus aliases/purposes.

## Package location

```text
ontogony-platform/docs/_incoming/_active/ONTOGONY-BACKEND-COORDINATION-002/
```

Copy repo-specific prompts from `repo-prompts/` into target repos when executing a slice locally.

## Read first (in order)

1. [`01_PACKAGE_MANIFEST.md`](./01_PACKAGE_MANIFEST.md)
2. [`02_CURRENT_STATE_BASELINE.md`](./02_CURRENT_STATE_BASELINE.md)
3. [`03_SCOPE_AND_BOUNDARY.md`](./03_SCOPE_AND_BOUNDARY.md)
4. [`04_MASTER_IMPLEMENTATION_SEQUENCE.md`](./04_MASTER_IMPLEMENTATION_SEQUENCE.md)
5. [`05_ACCEPTANCE_MATRIX.md`](./05_ACCEPTANCE_MATRIX.md)
6. The slice folder under `slices/<SLICE-ID>/`
7. The matching prompt under `prompts/P0N_*.md`

## Implementation order (strict)

| Phase | Slice | Gate before next |
| ---: | --- | --- |
| 1 | BACKEND-REPO-DOCS-ORDER-002 | Docs index + status banners consistent; link validator green or deferrals filed |
| 2 | SYSTEM-COMPATIBILITY-MATRIX-001 | Runtime lock + compatibility matrix refreshed; conformance tests pass |
| 3 | SHARED-ERROR-CONTRACT-001 | Cross-service error envelope adopted in all six backends |
| 4 | CROSS-REPO-IDENTITY-CORRELATION-001 | Trace/actor/idempotency headers propagated E2E in smoke |
| 5 | ALLAGMA-CONEXUS-MODEL-ALIAS-001 | No hard-coded provider models; alias manifest authoritative |
| 6 | BACKEND-SYSTEM-E2E-001 | Five-service governed smoke PASS with evidence JSON |
| 7 | AISTHESIS-RECONSTRUCTABILITY-SPINE-001 | Live reconstructability spine PASS (not fixture-only) |
| 8 | METABOLE-DATA-SPINE-HARDENING-001 | SLOD/data-spine certification matrix PASS |

Phases 7 and 8 may run in parallel after phase 6.

## Per-slice workflow

For each slice:

1. Run preflight: `./scripts/validate-backend-coordination-002-preflight.ps1 -DevRoot C:\dev -Slice <SLICE-ID>`
2. Execute slice `SPEC.md` acceptance gates.
3. Run validation commands from `07_CROSS_REPO_VALIDATION_PLAN.md`.
4. Record evidence under the owning repo's `docs/evidence/<SLICE>_*.md`.
5. Update slice README status; do not advance if gates fail without `DEFERRED_WITH_REASON`.

## Definition of done (parent package)

ONTOGONY-BACKEND-COORDINATION-002 closes when:

1. All eight slices are `PASS` or `DEFERRED_WITH_REASON` in [`05_ACCEPTANCE_MATRIX.md`](./05_ACCEPTANCE_MATRIX.md).
2. Cross-repo summary updated in `allagma-dotnet/docs/system/BACKEND_REPO_BOUNDARY_MATRIX.md`.
3. Runtime lock bumped only when package versions or locked commits change.
4. Closeout completed per [`10_CLOSEOUT_TEMPLATE.md`](./10_CLOSEOUT_TEMPLATE.md).
5. Package moved to `docs/_incoming/_consumed/<YYYY-MM>/ONTOGONY-BACKEND-COORDINATION-002/` with `CONSUMED.md`.

## Final report format

When finished, provide:

1. Slices completed vs deferred (with reasons).
2. Validation commands run per repo.
3. Test results (pass/fail/skip categories).
4. Files created/updated per repo.
5. Remaining boundary risks.
6. Recommended follow-on packages.
