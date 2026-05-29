# AISTHESIS-LIVE-SPINE-RC-READINESS-005 — package manifest

## Purpose

Upgrade Aisthesis from a strong alpha live-evidence-spine backend to an RC-readiness candidate by hardening operational gates and truth discipline around live evidence.

This is not a broad feature package.

## Package classification

```text
type: rc-readiness / operational-hardening
primary repo: aisthesis-dotnet
cross-repo: validation-oriented
feature expansion: discouraged
production claim: prohibited unless gates prove it
```

## Files

| Path | Purpose |
|---|---|
| `00_UNPACK_PROMPT.md` | Cursor execution prompt |
| `02_CURRENT_STATE_BASELINE.md` | Current known Aisthesis state |
| `03_SCOPE_AND_BOUNDARY.md` | What is in/out of scope |
| `04_ACCEPTANCE_MATRIX.md` | Closure criteria |
| `05_TARGET_FILE_MAP.md` | Files to add/update |
| `06_IMPLEMENTATION_PLAN.md` | Slice-based implementation plan |
| `07_VALIDATION_MATRIX.md` | Validation gates |
| `08_RELEASE_MODE_GATE.md` | ReleaseMode behavior requirements |
| `09_CI_FIVE_SERVICE_SMOKE.md` | CI-compatible live smoke design |
| `10_LES_002_COMPLETION_PLAN.md` | LES-002 partial-grade closure |
| `11_FRONTEND_CONTRACT_HANDOFF.md` | UI/API handoff |
| `12_PRODUCTION_IAM_GATE.md` | IAM promotion gate |
| `13_RETENTION_ERASURE_GATE.md` | Retention/erasure operational gate |
| `14_OTEL_TRACE_EXPORT_GATE.md` | Distributed trace export gate |
| `15_LOCK_DECISION_FRAMEWORK.md` | `lockRequired` decision record |
| `16_RISK_REGISTER.md` | Risks and mitigations |
| `17_ROLLBACK_PLAN.md` | Rollback and recovery |
| `18_CLOSEOUT_TEMPLATE.md` | Required closeout format |
| `contracts/` | Proposed stable contracts |
| `scripts/` | Script templates |
| `repo-prompts/` | Repo-specific prompts |
| `fixtures/` | Example summaries |
| `docs-to-promote/` | Draft docs to copy/adapt into repos |

## Expected output classification

At closeout, classify as one of:

```text
RC-ready candidate
RC-readiness partial
Blocked
```

Do not use “production-ready” unless production IAM, retention/erasure, OTel export, and full CI gates are complete.
