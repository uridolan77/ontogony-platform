# Intake report — ontogony-system-release-gates-package-2026-05-22

**Intake date:** 2026-05-22  
**Source ZIP:** `docs/_incoming/ontogony-system-release-gates-package-2026-05-22.zip`  
**Unpack root:** `docs/_incoming/unpacked/ontogony-system-release-gates-package-2026-05-22/`  
**Inner package root:** `ontogony-system-release-gates-package-2026-05-22/` (one extra directory level from `Expand-Archive`)

**Scope IDs:** `SYSTEM-ALPHA-007`, `SYSTEM-ALPHA-008`, `PLATFORM-REL-001`  
**Repos:** `ontogony-platform`, `allagma-dotnet`, `kanon-dotnet`, `conexus-dotnet`

---

## 1. Package contents summary

Cross-repo **release-gate planning package** (docs + JSON schemas + PowerShell/YAML stubs). It formalizes a four-repo release train:

```text
runtime lock → locked checkout → repo gates → package-mode proof → system cohesion → evidence bundle
```

| Area | Files | Role |
|------|-------|------|
| Planning | `00_EXECUTIVE_BRIEF.md` … `07_SOURCE_MAP.md`, `PACKAGE_MANIFEST.json` | Baseline, architecture, PR slices, acceptance, failure policy |
| PR specs | `pr-specs/SYSTEM_ALPHA_007_*.md`, `SYSTEM_ALPHA_008_*.md`, `PLATFORM_REL_001_*.md` | Per-slice deliverables and acceptance |
| Contracts | `contracts/*.md` (4) | Canonical policy/contract prose |
| Schemas | `schemas/*.json` (3) | Evidence bundle shapes (also duplicated under `repo-patches/`) |
| Operator | `operator/*.md` (4) | Runbooks + troubleshooting |
| Implementation | `implementation/*.md` (3) | Implementer prompt, reviewer checklist, branch plan |
| **Repo patches** | `repo-patches/{ontogony-platform,allagma-dotnet,kanon-dotnet,conexus-dotnet}/` | Copy targets for first implementation |

**Manifest note:** `PACKAGE_MANIFEST.json` claims `fileCount: 74`; the unpacked tree has **~44 files** under the inner folder (likely counts planned + duplicate schema copies).

### Unpacked file tree (abbreviated)

```text
ontogony-system-release-gates-package-2026-05-22/
├── 00_EXECUTIVE_BRIEF.md … 07_SOURCE_MAP.md, PACKAGE_MANIFEST.json, README.md
├── contracts/          (4 markdown contracts)
├── schemas/          (3 JSON schemas — canonical copies)
├── operator/         (4 runbooks)
├── implementation/   (3 guides)
├── pr-specs/         (3 PR specs)
└── repo-patches/
    ├── ontogony-platform/
    │   ├── .github/workflows/runtime-release-gate.yml
    │   ├── docs/operators/, docs/releases/runtime-release-evidence/
    │   ├── schemas/runtime-release-evidence-bundle.schema.json
    │   └── scripts/release/run-locked-runtime-release-gate.ps1
    │       validate-runtime-release-evidence.ps1
    ├── allagma-dotnet/
    │   ├── .github/workflows/{package-mode-release-train,system-cohesion-scheduled}.yml
    │   ├── docs/operators/, docs/system/SYSTEM_COHESION_SCENARIO_CONTRACT.md
    │   ├── schemas/{package-mode,system-cohesion}-*.schema.json
    │   └── scripts/release/{run-package-mode-release-train,run-scheduled-system-cohesion}.ps1
    ├── kanon-dotnet/docs/evidence/SYSTEM_ALPHA_007_*_STUB.md
    └── conexus-dotnet/docs/evidence/SYSTEM_ALPHA_008_*_STUB.md
```

---

## 2. Intended repo targets

| Content | Primary repo | Target paths (from package) |
|---------|--------------|----------------------------|
| **SYSTEM-ALPHA-007A** — evidence contract, schema, validator | `ontogony-platform` | `docs/releases/runtime-release-evidence/`, `schemas/`, `scripts/release/`, `docs/operators/` |
| **SYSTEM-ALPHA-007B** — locked release orchestrator + workflow | `ontogony-platform` | `scripts/release/run-locked-runtime-release-gate.ps1`, `.github/workflows/runtime-release-gate.yml` |
| **SYSTEM-ALPHA-007C** — closeout evidence | `ontogony-platform` | `docs/evidence/`, `docs/releases/` (not in `repo-patches`; implement per pr-spec) |
| **PLATFORM-REL-001** — package-mode train | `allagma-dotnet` (impl), `ontogony-platform` (contract docs) | `scripts/release/`, `schemas/`, `.github/workflows/`, `docs/operators/`, `docs/evidence/` |
| **SYSTEM-ALPHA-008** — scheduled cohesion | `allagma-dotnet` (orchestration), `conexus-dotnet` (capacity sub-gate) | `scripts/release/`, `schemas/`, `.github/workflows/`, `docs/system/`, `docs/operators/` |
| Kanon contract gate evidence | `kanon-dotnet` | Stub only → real closeout under `docs/evidence/` |
| Conexus capacity evidence | `conexus-dotnet` | Stub only → delegates to existing `scripts/capacity/run-capacity-baseline.ps1` |

**Canonical planning docs** (`contracts/`, `pr-specs/`, `operator/`, `implementation/`) stay in **platform** under `docs/_planning/` or `docs/releases/` after reconciliation — not copied blindly from `repo-patches/`.

---

## 3. Proposed implementation order

Matches package `README.md`, `05_IMPLEMENTATION_ORDER.md`, and `03_PR_SEQUENCE.md`:

| Step | Slice | Repo | Rationale |
|------|-------|------|-----------|
| 1 | **SYSTEM-ALPHA-007A** | `ontogony-platform` | Evidence schema + validator first (fail-closed contract before orchestration) |
| 2 | **PLATFORM-REL-001A** | `allagma-dotnet` (+ platform contract doc) | Elevates existing `allagma-package-mode` CI into explicit release train |
| 3 | **SYSTEM-ALPHA-007B** | `ontogony-platform` | Orchestrator consumes 007A validator + Allagma package/cohesion outputs |
| 4 | **SYSTEM-ALPHA-008A** | `allagma-dotnet` | Scheduled/manual cohesion; calls existing smoke + Conexus capacity |
| 5 | **SYSTEM-ALPHA-007C** | `ontogony-platform` | First recorded PASS/FAIL bundle + closeout evidence |

**Branch plan (from package):** `platform/system-alpha-007-release-gate` → `allagma/platform-rel-001-package-train` → `allagma/system-alpha-008-scheduled-cohesion` → platform orchestrator → closeout.

---

## 4. Files that can be copied as-is (with path mapping)

After review in a real PR, these are the lowest-friction copies:

| Package path | Destination |
|--------------|-------------|
| `repo-patches/ontogony-platform/schemas/runtime-release-evidence-bundle.schema.json` | `ontogony-platform/schemas/` |
| `repo-patches/ontogony-platform/docs/releases/runtime-release-evidence/RUNTIME_RELEASE_EVIDENCE_CONTRACT.md` | same (or merge with `contracts/RUNTIME_RELEASE_EVIDENCE_CONTRACT.md`) |
| `repo-patches/ontogony-platform/scripts/release/validate-runtime-release-evidence.ps1` | `ontogony-platform/scripts/release/` |
| `repo-patches/allagma-dotnet/schemas/*.schema.json` | `allagma-dotnet/schemas/` |
| `contracts/*.md` | `ontogony-platform/docs/releases/` or `docs/governance/` (dedupe with repo-patches copies) |
| `operator/*.md` | Per-repo `docs/operators/` (platform vs allagma variants already split in `repo-patches`) |
| `pr-specs/*.md` | `ontogony-platform/docs/_planning/` (or existing planning path) |
| Kanon/Conexus `docs/evidence/*_STUB.md` | Starting placeholders only |

---

## 5. Files that need adaptation

| Item | Gap |
|------|-----|
| `run-locked-runtime-release-gate.ps1` | **No clone/fetch** — assumes four repos already under `WorkspaceRoot`; inline `repoGates` always `PASS`; `capacityBaseline` / `restartSurvival` / `streamingSmoke` left `null` |
| `run-package-mode-release-train.ps1` | Wraps `pack-cross-repo-packages.ps1` — verify param names vs live script; add missing `validate-package-mode-release-summary.ps1` (in pr-spec, **not** in package) |
| `run-scheduled-system-cohesion.ps1` | Must wire to `run-system-cohesion-smoke.ps1`, `restart-e2e-first-real-system.ps1`, streaming smoke, Conexus capacity env |
| `system-cohesion-scheduled.yml` / `runtime-release-gate.yml` | Cross-repo `actions/checkout` uses `${{ github.repository_owner }}/…` — confirm org/repo names and tokens |
| `03_PR_SEQUENCE.md` lists `write-runtime-release-evidence.ps1` | **Not shipped** — bundle write is inlined in orchestrator stub |
| `PLATFORM_REL_001` pr-spec | `validate-package-mode-release-summary.ps1`, `package-mode-release-train.yml` partially specified; workflow exists in patches but validator does not |
| Contract paths | pr-spec wants `ontogony-platform/docs/releases/package-mode/PACKAGE_MODE_RELEASE_CONTRACT.md`; package has `contracts/PACKAGE_MODE_RELEASE_CONTRACT.md` |
| Runtime lock SHAs | Lock baseline `SYSTEM-ALPHA-006` pins commits from 2026-05-20 era — release run may fail until lock refreshed post-implementation |
| Duplicate schemas | Same JSON in `schemas/` and `repo-patches/*/schemas/` — pick one canonical location per repo |

---

## 6. Prerequisite verification (current repo state)

| Assumed file | Status |
|--------------|--------|
| `allagma-dotnet/docs/system/ontogony-runtime.lock.json` | **Exists** — baseline `SYSTEM-ALPHA-006`, four `lockedCommits`, `packageVersions` |
| `allagma-dotnet/docs/system/SYSTEM_COMPATIBILITY_MATRIX.md` | **Exists** — documents lock, package-mode, cohesion |
| `allagma-dotnet/.github/workflows/ci.yml` | **Exists** — job `allagma-package-mode` (PLATFORM-REL baseline) |
| `allagma-dotnet/scripts/pack-cross-repo-packages.ps1` | **Exists** |
| `conexus-dotnet/docs/testing/CAPACITY_BASELINE.md` | **Exists** |
| `conexus-dotnet/docs/reviews/CONEXUS_NEXT_009_CAPACITY_AND_RESILIENCE_BASELINE.md` | **Exists** |
| `conexus-dotnet/scripts/capacity/run-capacity-baseline.ps1` | **Exists** |
| `conexus-dotnet/.github/workflows/capacity-baseline.yml` | **Exists** |
| `allagma-dotnet/scripts/run-system-cohesion-smoke.ps1` | **Exists** |
| `allagma-dotnet/scripts/architecture-conformance/run-cross-repo-conformance.ps1` | **Exists** |
| `kanon-dotnet/scripts/bootstrap-solution.ps1` | **Exists** |
| `kanon-dotnet/docs/generated/KANON_COMPATIBILITY_MANIFEST.json` | **Exists** |
| `ontogony-platform/scripts/pack-all.ps1` | **Exists** |
| `ontogony-platform/scripts/release/**` | **Does not exist** (greenfield) |
| `allagma-dotnet/scripts/release/**` | **Does not exist** (greenfield) |

**Conclusion:** Package baseline accurately reflects **existing** Allagma lock, package-mode CI, cohesion scripts, and Conexus capacity machinery. Greenfield work is release orchestration, evidence aggregation, schemas, and workflows — not foundational subsystem invention.

---

## 7. Risks / conflicts

1. **Stub vs production scripts** — Orchestrator marks repo gates `PASS` without parsing test output; first real run will need honest verdict propagation.
2. **Stale lock commits** — `lockedCommits` may not match current `main` on all four repos; `Locked` mode will fail until lock bump or intentional pin.
3. **Missing deliverables** — `write-runtime-release-evidence.ps1`, `validate-package-mode-release-summary.ps1` referenced in docs but absent from `repo-patches/`.
4. **CI duplication** — New workflows overlap `allagma-package-mode` and `capacity-baseline.yml`; risk of drift unless reusable workflow or shared scripts.
5. **Cross-repo Actions** — Scheduled cohesion checks out sibling repos at `main`, not lock SHAs, unless `Locked` input is wired end-to-end.
6. **Secrets / artifacts** — Failure policy forbids secrets in bundles; log redaction and artifact retention must match existing Ontogony conventions.
7. **Manifest file count** — `fileCount: 74` does not match unpacked count; treat manifest as advisory.
8. **No frontend** — Explicit non-goal; aligned with current workspace scope.

---

## 8. Exact next PR recommendation

**Open PR 1: `SYSTEM-ALPHA-007A` in `ontogony-platform`**

**Title (suggested):** `feat(release): add runtime release evidence contract and validator (SYSTEM-ALPHA-007A)`

**Include from package:**

- `repo-patches/ontogony-platform/docs/releases/runtime-release-evidence/RUNTIME_RELEASE_EVIDENCE_CONTRACT.md`
- `repo-patches/ontogony-platform/schemas/runtime-release-evidence-bundle.schema.json`
- `repo-patches/ontogony-platform/scripts/release/validate-runtime-release-evidence.ps1`
- `operator/RUNBOOK_FULL_LOCKED_RUNTIME_RELEASE_GATE.md` → `docs/operators/`
- Promote `contracts/RUNTIME_RELEASE_EVIDENCE_CONTRACT.md` + `contracts/LOCKED_RUNTIME_DRIFT_POLICY.md` into `docs/releases/` (dedupe)
- Add minimal fixture JSON under `schemas/fixtures/` and a CI step that runs validator against pass/fail fixtures

**Do not include in PR 1:** orchestrator, GitHub workflow, Allagma scripts, or lock bump.

**Acceptance for PR 1:** Validator fails on missing sections, rejects non-`Locked` mode with `-ReleaseMode`, rejects secret patterns, verifies four repo commit fields — per `03_PR_SEQUENCE.md` § SYSTEM-ALPHA-007A.

---

## 9. Intake acceptance

| Criterion | Status |
|-----------|--------|
| ZIP unpacked under `docs/_incoming/unpacked/` | Done |
| No implementation outside unpack dir | Done (only this report added) |
| Intake report at unpack root | This file |

**Next human step:** Review this report, then implement **SYSTEM-ALPHA-007A** in `ontogony-platform` before touching Allagma workflows.
