# AISTHESIS-LIVE-SPINE-RC-CERTIFICATION-006 — package manifest

## Classification

```yaml
package: AISTHESIS-LIVE-SPINE-RC-CERTIFICATION-006
type: rc-certification / integrated-hardening / cross-repo-readiness
primaryRepo: aisthesis-dotnet
sourcePackageAbsorbed: AISTHESIS-LIVE-SPINE-RC-READINESS-005
reviewInputsAbsorbed:
  - live five-service certification gap
  - required-edge matrix v2 gap
  - durable evaluation gap
  - Aisthesis.Client evaluation coverage gap
  - producer edge authorization gap
  - production IAM gap
  - retention/erasure implementation gap
  - OpenTelemetry distributed trace export gap
  - frontend live validation gap
productionClaimAllowed: false
```

## What this package is

A comprehensive implementation package for making Aisthesis the system-level evidence certification point for Ontogony's five backend repos.

## What this package is not

It is not a feature expansion package. It must not turn Aisthesis into a semantic engine, orchestrator, model gateway, data transformer, or UI app.

## File groups

| Group | Purpose |
|---|---|
| Root docs | Execution, acceptance, target map, risk/rollback, closeout |
| `contracts/` | Stable contracts to promote into `aisthesis-dotnet/docs/contracts/` |
| `docs-to-promote/` | Draft operational/evidence docs to copy/adapt into repos |
| `fixtures/` | Example golden/missing evidence and summary fixtures |
| `scripts/` | PowerShell script templates to adapt into `scripts/system/` |
| `repo-prompts/` | Cursor prompts for Aisthesis, producer repos, frontend, platform |
| `patch-guides/` | Implementation guidance for code-level changes |

## Expected closeout classifications

```text
RC-certification candidate
RC-certification partial
Blocked
```

Do not use `production-ready` unless production IAM, retention/erasure, OTel export, live CI, and frontend live backing are independently complete.
