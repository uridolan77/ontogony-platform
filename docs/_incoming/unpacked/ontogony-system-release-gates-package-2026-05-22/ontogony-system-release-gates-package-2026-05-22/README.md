# Ontogony system release gates package — SYSTEM-ALPHA-007 / SYSTEM-ALPHA-008 / PLATFORM-REL-001

**Package date:** 2026-05-22  
**Scope:** four-repo .NET runtime release discipline for:

1. `SYSTEM-ALPHA-007` — full locked-runtime release gate.
2. `SYSTEM-ALPHA-008` — scheduled full-system cohesion.
3. `PLATFORM-REL-001` — package-mode release train.

## Intended unpack location

Recommended:

```text
ontogony-platform/docs/_incoming/ontogony-system-release-gates-package-2026-05-22/
```

This package is intentionally cross-repo. It contains canonical planning docs plus repo-specific implementation stubs under `repo-patches/`.

## Repos

```text
ontogony-platform   shared mechanics / release authority
kanon-dotnet        semantic authority
conexus-dotnet      model gateway
allagma-dotnet      governed runtime / runtime lock owner
```

## Strategic intent

The current system has enough alpha cohesion to justify a formal release train. These three work packages convert the current moving-main, evidence-rich workflow into a repeatable release mechanism:

```text
runtime lock -> locked checkout -> full evidence run -> package-mode proof -> release bundle
```

## Non-goals

- Not production readiness.
- Not enterprise IAM.
- Not real external tool execution.
- Not changing Kanon v0 semantics.
- Not merging frontend work into this package.
- Not replacing existing repo CI; this adds release-grade gates above repo-local CI.

## Package contents

```text
00_EXECUTIVE_BRIEF.md
01_CURRENT_STATE_BASELINE.md
02_TARGET_ARCHITECTURE.md
03_PR_SEQUENCE.md
04_ACCEPTANCE_DASHBOARD.md
05_IMPLEMENTATION_ORDER.md
06_FAILURE_POLICY.md
07_SOURCE_MAP.md

pr-specs/
  SYSTEM_ALPHA_007_FULL_LOCKED_RUNTIME_RELEASE_GATE.md
  SYSTEM_ALPHA_008_SCHEDULED_FULL_SYSTEM_COHESION.md
  PLATFORM_REL_001_PACKAGE_MODE_RELEASE_TRAIN.md

contracts/
  RUNTIME_RELEASE_EVIDENCE_CONTRACT.md
  SYSTEM_COHESION_SCENARIO_CONTRACT.md
  PACKAGE_MODE_RELEASE_CONTRACT.md
  LOCKED_RUNTIME_DRIFT_POLICY.md

schemas/
  runtime-release-evidence-bundle.schema.json
  system-cohesion-scheduled-summary.schema.json
  package-mode-release-summary.schema.json

operator/
  RUNBOOK_FULL_LOCKED_RUNTIME_RELEASE_GATE.md
  RUNBOOK_SCHEDULED_SYSTEM_COHESION.md
  RUNBOOK_PACKAGE_MODE_RELEASE_TRAIN.md
  TROUBLESHOOTING.md

repo-patches/
  ontogony-platform/
  allagma-dotnet/
  kanon-dotnet/
  conexus-dotnet/

implementation/
  IMPLEMENTER_PROMPT.md
  REVIEWER_CHECKLIST.md
  BRANCH_AND_MERGE_PLAN.md
```

## Recommended implementation sequence

1. `SYSTEM-ALPHA-007A` — add release evidence contract and validator.
2. `PLATFORM-REL-001A` — lift current Allagma package-mode proof into a first-class release train.
3. `SYSTEM-ALPHA-007B` — implement locked-runtime release command/workflow.
4. `SYSTEM-ALPHA-008A` — implement scheduled/manual cohesion workflow.
5. `SYSTEM-ALPHA-007C` — close with first recorded release evidence bundle.

## Acceptance summary

The package is complete when one operator can run:

```powershell
./scripts/release/run-locked-runtime-release-gate.ps1 -ReleaseId SYSTEM-ALPHA-007 -Mode Locked
```

and receive a single evidence directory containing:

```text
runtime-lock snapshot
checked repo commits
repo-local build/test summaries
contract-gate summaries
package-mode release summary
system-cohesion scenario summary
capacity baseline summary
restart survival summary
streaming smoke summary
release verdict: PASS / FAIL
```
