# Ontogony unified documentation structure standard

**Item:** `DOCS-STANDARD-001`  
**Version:** v1  
**Effective:** 2026-05-19  
**Authority repo:** `uridolan77/ontogony-platform` (cross-repo governance)  
**Applies to:** `ontogony-platform`, `allagma-dotnet`, `kanon-dotnet`, `conexus-dotnet`, `ontogony-frontend`, `ontogony-ui`

**Boundary:** Documentation governance only. This standard does **not** constitute production readiness, real provider certification, cloud deployment, or permission to mass-migrate historical archives.

---

## 1. Purpose

The six Ontogony repos accumulated planning packages, evidence, closeouts, and operator guides across different folder habits. Before repo documentation sweeps (`RCQ-DOCS-001` per repo) and full manual guided QA, this standard defines:

1. A **shared target layout** for *current* documentation.
2. **Explicit rules** for evidence, operators, releases, and closeouts.
3. A **manageable reorganization policy** — indexes and pointers first; mass moves last and only when clearly justified.
4. **Historical/archive treatment** so old planning trees stay discoverable without being mistaken for current guidance.

Package source (control copy): [`docs/product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/standards/DOCUMENTATION_STRUCTURE_STANDARD.md`](../product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/standards/DOCUMENTATION_STRUCTURE_STANDARD.md).

Evidence: [`docs/evidence/DOCS_STANDARD_001_UNIFIED_DOCUMENTATION_STRUCTURE_EVIDENCE.md`](../evidence/DOCS_STANDARD_001_UNIFIED_DOCUMENTATION_STRUCTURE_EVIDENCE.md).

---

## 2. Principles

| # | Principle |
| --- | --- |
| P1 | **Every repo has a docs index** at `docs/README.md` (create or refresh; do not leave newcomers guessing). |
| P2 | **Evidence is centralized** under `docs/evidence/` with a predictable filename pattern (see §6). |
| P3 | **Operator-facing guidance** lives under `docs/operators/` (platform may also publish cross-repo operator standards there). |
| P4 | **Release closeouts and scorecards** live under `docs/releases/` (service repos) or platform `docs/releases/` for cross-repo milestones. |
| P5 | **Historical docs stay in place** unless a small, obvious misfile is corrected; add banners and index pointers instead of mass-moving archives. |
| P6 | **Status text must be honest** — distinguish **CLOSED / PASS**, **in progress**, **not started**, and **historical**. |
| P7 | **Never commit secrets** — no API keys, tokens, passwords, raw provider payloads, or unredacted connection strings in docs or evidence. |
| P8 | **Closeouts state the boundary** — Docker-local, product hardening, repo cleaning, and manual QA programs are **not production readiness** unless a separate `PROD-READINESS-*` charter explicitly says otherwise. |

Related: [ONTOGONY_TERMINOLOGY_GLOSSARY.md](./ONTOGONY_TERMINOLOGY_GLOSSARY.md), [repo cleaning principles](../product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/standards/REPO_CLEANING_PRINCIPLES.md).

---

## 3. Universal target structure

Use as the **target** for new and actively maintained docs. **Do not create empty folders** only to comply.

```text
docs/
  README.md                 # Required index (§4)
  architecture/             # System design, boundaries, ADRs (optional adr/ subfolder)
  api/                      # OpenAPI notes, contract matrices, HTTP surface docs
  development/              # Local dev, build, test commands, packaging
  deployment/               # Deploy/readiness notes (not prod charter by itself)
  evidence/                 # Verification records (§6)
  operators/                # Runbooks, glossaries, cross-cutting operator contracts
  releases/                 # Closeouts, scorecards, next-options (§7)
  testing/                  # Test strategy, CI notes, harness docs
  troubleshooting/          # Failure modes, debug playbooks
```

### 3.1 Where common doc types go

| Doc type | Primary location | Notes |
| --- | --- | --- |
| PR / program evidence | `docs/evidence/` | One file per completed item when practical |
| Operator runbook | `docs/operators/` or service `docs/operations/` | Prefer `operators/` for cross-cutting; keep service-specific ops local |
| Release closeout | `docs/releases/` | Name: `<PROGRAM>_CLOSEOUT.md` or existing convention |
| Scorecard / limitations | `docs/releases/` | Pair with closeout |
| Next options / roadmap after closeout | `docs/releases/*_NEXT_OPTIONS.md` | Must state **not production readiness** |
| OpenAPI / contract drift | `docs/api/` or `docs/architecture/` | Align with repo convention; link generated client paths in frontend |
| CI cost / branch protection | `docs/operators/` (platform) | Example: [CI_COST_CONTROL.md](./CI_COST_CONTROL.md) |
| Product-hardening control package | `docs/product-hardening/<package>/` | **Platform only**; other repos link here |
| Environment / compose programs | `docs/environments/` | **Platform** (+ service overlays under each backend repo) |
| Migrations (breaking changes) | `docs/migrations/` | When present |
| ADRs | `docs/adr/` or `docs/architecture/` | Either is acceptable if indexed |

---

## 4. Required docs index (`docs/README.md`)

Each repo **must** maintain `docs/README.md` containing at minimum:

1. **Purpose** of the repo’s documentation.
2. **Current operator entry points** (links to active runbooks, not historical plans).
3. **Development** — how to build/test locally.
4. **Architecture / contracts** — boundaries and integration surfaces.
5. **Evidence** — link to `docs/evidence/` and naming hint.
6. **Releases / closeouts** — active and recently closed programs.
7. **Known limitations** — or link to consolidated limitations doc.
8. **Historical / archived docs** — explicit statement that `_planning/`, `_incoming/`, `legacy/`, phase folders, etc. are not current unless cross-linked.

Use the package template as a starting outline: [`DOCS_README_TEMPLATE.md`](../product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/templates/DOCS_README_TEMPLATE.md).

**Platform canonical index:** [`docs/README.md`](../README.md) (created by `DOCS-STANDARD-001`).

---

## 5. Repo-specific target structures

These extend §3. Existing folders **not listed** may remain; add index pointers rather than renaming trees wholesale.

### 5.1 `ontogony-platform`

```text
docs/
  README.md
  architecture/
  environments/             # Docker-local and environment programs (platform-owned)
  evidence/
  operators/                # Cross-repo governance (this standard, glossary, CI, trace)
  product-hardening/        # Control packages (PFH, RCQ, future)
  releases/                 # Cross-repo milestone closeouts
  testing/
  troubleshooting/
```

**Existing paths (keep; index only):** `planning/`, `alignment/`, `governance/`, `packages/`, `migrations/`, numbered roots (`00_START_HERE.md`, …), `_incoming/`, `_planning/`. Treat `_planning/` and `_incoming/` as **non-canonical intake** — not operator entry points.

**Contributor entry:** [`00_START_HERE.md`](../00_START_HERE.md) remains valid for platform extraction work; `docs/README.md` is the **documentation map**.

### 5.2 `allagma-dotnet` / `kanon-dotnet` / `conexus-dotnet`

```text
docs/
  README.md
  api/
  architecture/
  development/
  deployment/               # conexus-dotnet; allagma/kanon may use operations/ until aligned
  evidence/
  operators/
  releases/
  testing/
  troubleshooting/
```

**Existing paths (keep; index only):** `_planning/`, `_incoming/`, `legacy/`, `integrations/`, `migrations/`, `reviews/`, `security/`, service-specific trees (`evals/`, `domain-packs/`, `providers/`, etc.).

| Repo | Notable existing folders | Index as |
| --- | --- | --- |
| allagma-dotnet | `evals/`, `environments/`, `operations/`, `runbooks/` | Service-specific; link from README |
| kanon-dotnet | `domain-packs/`, `api/` | Current guidance under api/ and domain-packs/ |
| conexus-dotnet | `providers/`, `deployment/`, `testing/` | Already near target |

### 5.3 `ontogony-frontend`

```text
docs/
  README.md
  architecture/
  development/
  evidence/
  operators/
  releases/                 # may be release/ today — align gradually
  testing/
  troubleshooting/
```

**Existing paths (keep; index only):** `openapi/`, `generated/`, `phase-*`, `PLAN_2/`, `_incoming/`. Phase folders are **historical program artifacts** unless a status board says otherwise.

### 5.4 `ontogony-ui`

```text
docs/
  README.md
  architecture/
  components/
  development/
  evidence/
  releases/                 # may be release/ today — align gradually
  testing/
  troubleshooting/
```

**Existing paths (keep; index only):** `design/`, `specs/`, `donor/`, `proposed-src/`, `pr-plan/`, `_incoming/`.

---

## 6. Evidence files

All repos place verification records under **`docs/evidence/`**.

### 6.1 Filename convention

```text
<ITEM>_EVIDENCE.md
```

Examples: `RCQ_000_PACKAGE_SETUP_EVIDENCE.md`, `DOCS_STANDARD_001_UNIFIED_DOCUMENTATION_STRUCTURE_EVIDENCE.md`, `FE_PRODUCT_CLOSEOUT_001_EVIDENCE.md`.

Use uppercase item ids with underscores; match the control prompt or PR id when one exists.

### 6.2 Required sections

Every evidence file **must** include:

| Section | Content |
| --- | --- |
| Title + date | UTC date recorded |
| Verdict | PASS / PARTIAL / FAIL |
| Boundary | Explicit **not production readiness** when applicable |
| Scope | Repos, paths, items |
| Commands | Reproducible command block |
| Results | Table of checks |
| Files changed | List or “docs only” |
| Known limitations | What was not verified |
| Safety | No secrets |

Template: [`EVIDENCE_TEMPLATE.md`](../product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/templates/EVIDENCE_TEMPLATE.md).  
Package standard: [`EVIDENCE_FILE_STANDARD.md`](../product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/standards/EVIDENCE_FILE_STANDARD.md).

### 6.3 Evidence index

Repos with many evidence files should add `docs/evidence/README.md` (allagma already has one). Platform lists key evidence from [`docs/README.md`](../README.md).

---

## 7. Releases, closeouts, and next-options

### 7.1 Closeout documents

Cross-repo milestones on **platform**:

- `docs/releases/<PROGRAM>_CLOSEOUT.md`
- `docs/releases/<PROGRAM>_SCORECARD.md`
- `docs/releases/<PROGRAM>_KNOWN_LIMITATIONS.md` (when needed)

Service repos mirror the same pattern under their own `docs/releases/`.

Each closeout **must** include:

```text
This milestone is NOT production readiness.
```

List prerequisites (closed programs), verdict, and evidence links.

### 7.2 Next-options documents

After a program closes, `*_NEXT_OPTIONS.md` documents optional tracks. It **must**:

1. State current posture (what is CLOSED / NOT STARTED).
2. Link the active next preparation package when chartered (e.g. RCQ).
3. Repeat the production-readiness boundary.

Current examples:

- [POST_DOCKER_LOCAL_HARDENING_NEXT_OPTIONS.md](../releases/POST_DOCKER_LOCAL_HARDENING_NEXT_OPTIONS.md)
- [PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_NEXT_OPTIONS.md](../releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_NEXT_OPTIONS.md)

---

## 8. Historical, archive, and intake treatment

### 8.1 Categories

| Prefix / folder | Treatment |
| --- | --- |
| `docs/_incoming/` | Staging for ZIPs and unpack review; **not** operator guidance. May contain duplicates; do not link from README as current. |
| `docs/_planning/` | Historical or in-flight planning; mark **historical** when superseded. |
| `docs/legacy/` | Historical product names (Agentor, Athanor, etc.); index with banner. |
| `docs/planning/` (no underscore) | May be active or historical — README must say which. |
| Phase folders (`phase-h-*`, `PLAN_2`, compose packages) | **Historical** unless status board says in progress. |
| Numbered platform roots (`00_START_HERE.md`, Sprints) | Contributor history; link from README under “Historical / platform extraction”. |

### 8.2 Banner for superseded docs

When touching a superseded doc, add at the top:

```markdown
> **Historical** — Superseded by `<link>`. Not current operator guidance.
```

Do **not** rewrite body content of old planning packages unless correcting factual errors.

### 8.3 Manageable reorganization (allowed vs avoid)

**Allowed in `RCQ-DOCS-001` sweeps:**

- Create or refresh `docs/README.md`
- Add `docs/evidence/README.md`
- Fix stale status lines (in progress → closed)
- Add pointers from README, product-hardening README, next-options
- Move **a small number** of clearly misplaced *current* docs
- Consolidate duplicate “limitations” sections into one linked doc

**Avoid:**

- Mass-moving `_planning/`, `_incoming/`, or phase trees
- Rewriting historical archives for style
- Changing hundreds of links without spot-checking
- Mixing documentation PRs with runtime or workflow changes
- Deleting evidence or closeouts

---

## 9. Cross-repo linking

| Rule | Example |
| --- | --- |
| Prefer relative links inside a repo | `../evidence/FOO_EVIDENCE.md` |
| Cross-repo references use repo-root paths in prose | `allagma-dotnet/docs/releases/...` |
| Platform hosts cross-repo governance | `docs/operators/`, `docs/product-hardening/` |
| Six-repo layout assumption | Sibling folders under `C:\dev\` (see glossary) |
| Terminology | [ONTOGONY_TERMINOLOGY_GLOSSARY.md](./ONTOGONY_TERMINOLOGY_GLOSSARY.md) — **Allagma** not Agentor in active docs |

---

## 10. Program sequence (repo cleaning package)

Current RCQ sequence after this standard:

```text
RCQ-000   package setup                    DONE
DOCS-STANDARD-001 unified standard         DONE (this document)
RCQ-CODE-001 per-repo code tightening      NOT STARTED
RCQ-DOCS-001 per-repo documentation sweep  NOT STARTED
PRODUCT-MANUAL-QA-001 package              NOT STARTED
PRODUCT-MANUAL-QA-002 execution            NOT STARTED
```

Control package: [`docs/product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/`](../product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/).

---

## 11. Adoption checklist (per repo)

Use during `RCQ-DOCS-001-<repo>`:

- [ ] `docs/README.md` exists and matches §4
- [ ] Evidence discoverable under `docs/evidence/`
- [ ] Active closeouts linked; stale “in progress” removed
- [ ] Historical folders labeled in README
- [ ] Limitations consolidated or linked
- [ ] No secrets in changed files
- [ ] No mass archive moves
- [ ] PR is docs-only (no `src/`, `.github/workflows/`, or provider keys)

Repo prompts: [`prompts/<repo>/DOCUMENTATION_CLEANING_PROMPT.md`](../product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/prompts/PROMPT_INDEX.md).

---

## 12. Required boundary statement

Copy into closeouts, evidence, and operator programs as appropriate:

```text
This work is NOT production readiness. It does not authorize real provider mode by default,
cloud deployment, production identity/TLS/secrets, or unscoped runtime refactors.
```

---

## Changelog

| Date | Change |
| --- | --- |
| 2026-05-19 | v1 published (`DOCS-STANDARD-001`) |
