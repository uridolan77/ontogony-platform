# RCQ-CODE-001 — ontogony-platform code hygiene evidence

**Recorded at (UTC):** 2026-05-19  
**Verdict:** **PASS — no code changes required**

**Boundary:** Platform script/test/schema hygiene verification only. **Not production readiness.** No runtime feature work. No workflow changes.

## Scope

`ontogony-platform` hosts shared mechanics (libraries, Docker-local operator scripts, cross-repo schemas, evidence validators). RCQ-CODE-001 on platform is **verification and evidence**, not a product-code sweep — sister repos received the executable hygiene changes.

## Files inspected

| Area | Path | Finding |
| --- | --- | --- |
| Docker-local scripts | `docker/local-working-system/scripts/*.ps1` (17 files) | `Set-StrictMode -Version Latest` on entry scripts; deterministic `$LASTEXITCODE` throws; shared `_docker-local-env.ps1` |
| Product-hardening schemas | `docs/product-hardening/eval-alignment-frontend-depth/schemas/` | JSON Schema + minimal valid fixture present |
| Schema tests | `tests/Ontogony.Infrastructure.Tests/SchemaFixtureValidationTests.cs` | Envelope + invalid fixtures |
| Eval export schema test | `tests/Ontogony.Infrastructure.Tests/EvalEvidenceExportBundleSchemaTests.cs` | PFH bundle schema ↔ fixture |
| Manual QA prep package | `docs/product-hardening/repo-cleaning-documentation-manual-qa-prep-v1/` | Prompts, sequences, standards — docs-only; no stale hardcoded repo paths in validators |
| Runtime artifacts | `.gitignore` (`artifacts/`, `bin/`, `obj/`) | Local `artifacts/` present on disk, **gitignored** — not committed |
| Report validators | `validate-*-report.ps1`, `inspect-*-evidence.ps1` under Docker-local scripts | Parse-clean; redaction patterns inherited from prior ENV hardening |

## Commands

```powershell
cd C:\dev\ontogony-platform

# PowerShell parse (all Docker-local scripts)
Get-ChildItem docker/local-working-system/scripts/*.ps1 | ForEach-Object {
  $tokens = $null; $errors = $null
  [void][System.Management.Automation.Language.Parser]::ParseFile($_.FullName, [ref]$tokens, [ref]$errors)
  if ($errors.Count) { throw "Parse failed: $($_.Name)" }
}

dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj -c Release

dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj -c Release --no-build `
  --filter "FullyQualifiedName~SchemaFixture|FullyQualifiedName~EvalEvidence"

git status --short
```

## Results

| Check | Result |
| --- | --- |
| PowerShell parse (17 scripts) | **PASS** |
| `Ontogony.Infrastructure.Tests` (full) | **PASS** (281 tests) |
| Schema fixture + eval export bundle tests | **PASS** (filtered) |
| `git status` (runtime artifacts) | **clean** — no staged `bin/`, `obj/`, or `artifacts/` |
| Platform `src/` code diff for RCQ | **none** — hygiene already satisfied at PFH/ENV closeout |
| Workflow files changed | **none** |
| Secrets in evidence | **none** |

## Code changes

**None.** Platform RCQ-CODE-001 closes with evidence only. Executable fixes landed in sister repos during their sweeps (Allagma, Kanon, Conexus, ontogony-ui, ontogony-frontend).

## Known limitations

- Full Docker-local guided main flow not re-run in this sweep (operator acceptance defers to `PRODUCT-MANUAL-QA-001`).
- `artifacts/` may exist locally after package-smoke runs; must remain gitignored.
- Platform does not host product OpenAPI or operator UI routes — those are verified in service/frontend repos.

## Safety

- No secrets
- No workflow changes
- Not production readiness

## Related evidence

- RCQ package setup: [`RCQ_000_PACKAGE_SETUP_EVIDENCE.md`](./RCQ_000_PACKAGE_SETUP_EVIDENCE.md)
- Documentation sweep: [`RCQ_DOCS_001_ONTOGONY_PLATFORM_EVIDENCE.md`](./RCQ_DOCS_001_ONTOGONY_PLATFORM_EVIDENCE.md)
- Six-repo closeout: [`RCQ_DOCS_FINAL_001_REPO_CLEANING_CLOSEOUT_EVIDENCE.md`](./RCQ_DOCS_FINAL_001_REPO_CLEANING_CLOSEOUT_EVIDENCE.md)
