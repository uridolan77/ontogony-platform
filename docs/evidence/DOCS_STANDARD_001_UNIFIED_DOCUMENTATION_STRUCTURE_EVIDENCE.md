# DOCS-STANDARD-001 — Unified documentation structure evidence

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS** — governance published; platform index added; no mass reorg

**Boundary:** Documentation governance only. **Not production readiness.** No real provider mode, cloud deployment, or runtime/workflow changes.

## Scope

| Field | Value |
| --- | --- |
| Item | `DOCS-STANDARD-001` |
| Authority repo | `uridolan77/ontogony-platform` |
| Applies to | Six Ontogony repos (standard defines all; adoption per repo in `RCQ-DOCS-001`) |
| Control prompt | `docs/product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/prompts/00-meta/DOCS-STANDARD-001_UNIFIED_DOCUMENTATION_STANDARD.md` |

## Deliverables

| Deliverable | Path | Status |
| --- | --- | --- |
| Canonical standard | `docs/operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md` | **created** (~15 KB) |
| Platform docs index | `docs/README.md` | **created** |
| Evidence (this file) | `docs/evidence/DOCS_STANDARD_001_UNIFIED_DOCUMENTATION_STRUCTURE_EVIDENCE.md` | **created** |
| Product-hardening pointer | `docs/product-hardening/README.md` | **updated** |
| Post-Docker next-options pointer | `docs/releases/POST_DOCKER_LOCAL_HARDENING_NEXT_OPTIONS.md` | **updated** |
| PFH next-options pointer | `docs/releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_NEXT_OPTIONS.md` | **updated** |
| RCQ package README | `docs/product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/README.md` | **updated** |
| RCQ-000 evidence cross-link | `docs/evidence/RCQ_000_PACKAGE_SETUP_EVIDENCE.md` | **updated** |

## Commands

```powershell
# Standard file present and non-trivial
Test-Path 'docs\operators\ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md'
(Get-Content 'docs\operators\ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md' -Raw).Length -ge 8000

# Platform docs index
Test-Path 'docs\README.md'

# Six-repo docs/README.md audit (only platform required in this item)
@('ontogony-platform','allagma-dotnet','kanon-dotnet','conexus-dotnet','ontogony-frontend','ontogony-ui') | ForEach-Object {
  $p = if ($_ -eq 'ontogony-platform') { 'docs\README.md' } else { "C:\dev\$_\docs\README.md" }
  [PSCustomObject]@{ Repo = $_; DocsReadme = (Test-Path $p) }
}

# Secrets scan (policy words only expected)
Select-String -Path 'docs\operators\ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md','docs\README.md' `
  -Pattern 'sk-[a-zA-Z0-9]{20,}|AKIA[0-9A-Z]{16}|BEGIN (RSA |OPENSSH )?PRIVATE' -Quiet

# Runtime / workflow guard
git diff --name-only HEAD -- 'src/' '.github/' 'docker/' '*.cs' '*.csproj' '*.sln' 'package.json'
```

## Results

| Check | Result |
| --- | --- |
| Standard defines universal structure | **PASS** — §3 universal target + doc-type table |
| Standard defines repo-specific structure (6 repos) | **PASS** — §5.1–5.4 |
| Historical / archive treatment explicit | **PASS** — §8 (`_incoming`, `_planning`, `legacy`, phase folders, banners) |
| Evidence file rules documented | **PASS** — §6 + link to package `EVIDENCE_FILE_STANDARD.md` |
| Manageable reorganization rule | **PASS** — §8.3 allowed vs avoid |
| Closeout / next-options boundary | **PASS** — §7 |
| Platform `docs/README.md` created | **PASS** |
| Sister repo `docs/README.md` | **DONE** — 5/5 (2026-05-19; see RCQ closeout evidence) |
| Mass doc reorg performed | **none** |
| Runtime source changes | **none** |
| Workflow changes | **none** |
| Secrets committed | **none** |
| Code cleanup started | **no** |
| Manual QA started | **no** |

## Standard content summary

The published standard (`ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md`) includes:

1. Purpose, boundary, and cross-links to RCQ package + terminology glossary.
2. Eight principles (index, evidence, operators, releases, historical in place, status honesty, no secrets, closeout boundary).
3. Universal folder purposes and doc-type placement table.
4. Repo-specific targets for all six repos with **existing folder mapping** (no mandatory renames).
5. Evidence naming (`<ITEM>_EVIDENCE.md`) and required sections.
6. Release closeout and next-options requirements.
7. Historical/archive/intake treatment and superseded-doc banner text.
8. Cross-repo linking rules.
9. RCQ program sequence with `DOCS-STANDARD-001` marked DONE.
10. Per-repo adoption checklist for `RCQ-DOCS-001`.

## Sister repo docs layout snapshot (read-only audit)

Recorded for `RCQ-DOCS-001` planning — **not modified** in this item.

| Repo | `docs/README.md` | Notable existing top-level dirs |
| --- | --- | --- |
| ontogony-platform | **yes** (this item) | `environments/`, `product-hardening/`, `releases/`, `operators/`, `planning/`, `_incoming/`, … |
| allagma-dotnet | no | `evidence/`, `operators/`, `releases/`, `evals/`, `_planning/`, `legacy/` |
| kanon-dotnet | no | `evidence/`, `api/`, `domain-packs/`, `_planning/`, `legacy/` |
| conexus-dotnet | no | `evidence/`, `deployment/`, `testing/`, `providers/`, `_incoming_packages/` |
| ontogony-frontend | no | `operators/`, `evidence/`, `openapi/`, phase folders |
| ontogony-ui | no | `components/`, `development/`, `release/`, `testing/`, `_incoming/` |

## Files changed

- `docs/operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md` (new)
- `docs/README.md` (new)
- `docs/evidence/DOCS_STANDARD_001_UNIFIED_DOCUMENTATION_STRUCTURE_EVIDENCE.md` (new)
- `docs/product-hardening/README.md`
- `docs/product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/README.md`
- `docs/releases/POST_DOCKER_LOCAL_HARDENING_NEXT_OPTIONS.md`
- `docs/releases/PRODUCT_FEATURE_HARDENING_EVAL_ALIGNMENT_FRONTEND_DEPTH_NEXT_OPTIONS.md`
- `docs/evidence/RCQ_000_PACKAGE_SETUP_EVIDENCE.md`

## Known limitations

- Sister-repo `docs/README.md` indexes created in `RCQ-DOCS-001` (2026-05-19) — see [`RCQ_DOCS_FINAL_001_REPO_CLEANING_CLOSEOUT_EVIDENCE.md`](./RCQ_DOCS_FINAL_001_REPO_CLEANING_CLOSEOUT_EVIDENCE.md).
- No link validation pass across hundreds of historical planning paths.
- Package control copy under `repo-cleaning-documentation-manual-qa-prep-v1/standards/` is unchanged (platform publishes canonical operator copy).
- No ZIP checksum pinned.

## Safety

- No secrets
- Not production readiness

## Next step

**`RCQ-DOCS-001`** on `ontogony-platform` completed — see [`RCQ_DOCS_001_ONTOGONY_PLATFORM_EVIDENCE.md`](./RCQ_DOCS_001_ONTOGONY_PLATFORM_EVIDENCE.md).

**`RCQ-CODE-001`** — per-repo code tightening (`prompts/<repo>/CODE_CLEANING_PROMPT.md`), starting with backend services per [`01_REPO_CLEANING_SEQUENCE.md`](../product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/sequences/01_REPO_CLEANING_SEQUENCE.md).
