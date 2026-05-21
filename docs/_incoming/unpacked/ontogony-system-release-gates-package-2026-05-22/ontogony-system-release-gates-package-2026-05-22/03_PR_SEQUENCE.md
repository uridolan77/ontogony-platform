# PR sequence

## SYSTEM-ALPHA-007A — Evidence contract and validator

Repo: `ontogony-platform`

Adds:

- `docs/releases/runtime-release-evidence/RUNTIME_RELEASE_EVIDENCE_CONTRACT.md`
- `schemas/runtime-release-evidence-bundle.schema.json`
- `scripts/release/validate-runtime-release-evidence.ps1`
- `scripts/release/write-runtime-release-evidence.ps1`
- operator runbook

Acceptance:

- Validator fails missing required sections.
- Validator rejects `moving-main` evidence as release evidence.
- Validator verifies four repo commit ids are present.
- Validator verifies package versions and scenario verdicts.

## PLATFORM-REL-001A — Package-mode release train contract

Repo: `allagma-dotnet` primarily, `ontogony-platform` docs secondarily.

Adds:

- first-class package-mode release summary JSON;
- validator for package version alignment with runtime lock;
- explicit no-sibling-reference assertion;
- release-train runbook;
- package-mode evidence upload.

Acceptance:

- Allagma restores/builds/tests with package mode only.
- Version row matches runtime lock.
- Evidence summary records feed path, package ids, versions, and test verdict.
- No sibling project references are used in package-mode validation.

## SYSTEM-ALPHA-007B — Full locked-runtime release gate command

Repo: `ontogony-platform`

Adds:

- `scripts/release/run-locked-runtime-release-gate.ps1`
- GitHub workflow template for manual locked release gate.
- bundle writer/collector.

Acceptance:

- Can checkout all four repos at pinned lock commits.
- Can run repo-local gates.
- Can invoke package-mode release train.
- Can invoke cohesion suite or consume its output.
- Emits one evidence bundle.

## SYSTEM-ALPHA-008A — Scheduled full-system cohesion workflow

Repo: `allagma-dotnet`, with Conexus capacity sub-call.

Adds:

- `scripts/release/run-scheduled-system-cohesion.ps1`
- `.github/workflows/system-cohesion-scheduled.yml`
- `system-cohesion-scheduled-summary.schema.json`
- cohesion runbook.

Acceptance:

- Manual dispatch works.
- Nightly schedule works.
- Runs completed run, human gates, Kanon assistance, Conexus fallback, restart survival, streaming smoke, capacity baseline.
- Writes JSON and Markdown summaries.
- Marks drift mode clearly when not using locked commits.

## SYSTEM-ALPHA-007C — First release bundle closeout

Repo: `ontogony-platform`

Adds:

- recorded evidence document under `docs/evidence/`
- release closeout doc under `docs/releases/`
- lock baseline update recommendation if all gates pass.

Acceptance:

- Evidence bundle exists and validates.
- Bundle references immutable artifact paths.
- Release verdict is PASS or explicitly FAIL with failure class.
