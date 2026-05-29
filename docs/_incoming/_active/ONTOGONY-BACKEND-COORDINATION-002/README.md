# ONTOGONY-BACKEND-COORDINATION-002

**Post-cleanup backend cohesion sprint** — eight ordered slices that close the gaps identified by `BACKEND-REPO-CLEANUP-ORGANIZATION-001`.

## Entry point

Start with [`00_UNPACK_PROMPT.md`](./00_UNPACK_PROMPT.md), then [`04_MASTER_IMPLEMENTATION_SEQUENCE.md`](./04_MASTER_IMPLEMENTATION_SEQUENCE.md).

## Slices (execute in order)

| # | Slice | Owner repo | Folder |
| ---: | --- | --- | --- |
| 1 | BACKEND-REPO-DOCS-ORDER-002 | All six backends | [`slices/BACKEND-REPO-DOCS-ORDER-002/`](./slices/BACKEND-REPO-DOCS-ORDER-002/) |
| 2 | SYSTEM-COMPATIBILITY-MATRIX-001 | allagma-dotnet | [`slices/SYSTEM-COMPATIBILITY-MATRIX-001/`](./slices/SYSTEM-COMPATIBILITY-MATRIX-001/) |
| 3 | SHARED-ERROR-CONTRACT-001 | ontogony-platform | [`slices/SHARED-ERROR-CONTRACT-001/`](./slices/SHARED-ERROR-CONTRACT-001/) — **PASS** 2026-05-29 |
| 4 | CROSS-REPO-IDENTITY-CORRELATION-001 | ontogony-platform | [`slices/CROSS-REPO-IDENTITY-CORRELATION-001/`](./slices/CROSS-REPO-IDENTITY-CORRELATION-001/) |
| 5 | ALLAGMA-CONEXUS-MODEL-ALIAS-001 | allagma-dotnet + conexus-dotnet | [`slices/ALLAGMA-CONEXUS-MODEL-ALIAS-001/`](./slices/ALLAGMA-CONEXUS-MODEL-ALIAS-001/) |
| 6 | BACKEND-SYSTEM-E2E-001 | allagma-dotnet | [`slices/BACKEND-SYSTEM-E2E-001/`](./slices/BACKEND-SYSTEM-E2E-001/) |
| 7 | AISTHESIS-RECONSTRUCTABILITY-SPINE-001 | aisthesis-dotnet | [`slices/AISTHESIS-RECONSTRUCTABILITY-SPINE-001/`](./slices/AISTHESIS-RECONSTRUCTABILITY-SPINE-001/) |
| 8 | METABOLE-DATA-SPINE-HARDENING-001 | metabole-dotnet | [`slices/METABOLE-DATA-SPINE-HARDENING-001/`](./slices/METABOLE-DATA-SPINE-HARDENING-001/) |

## Package spine

| Doc | Purpose |
| --- | --- |
| [`01_PACKAGE_MANIFEST.md`](./01_PACKAGE_MANIFEST.md) | Machine + human manifest |
| [`02_CURRENT_STATE_BASELINE.md`](./02_CURRENT_STATE_BASELINE.md) | Verified starting truth |
| [`03_SCOPE_AND_BOUNDARY.md`](./03_SCOPE_AND_BOUNDARY.md) | Ownership and forbidden moves |
| [`05_ACCEPTANCE_MATRIX.md`](./05_ACCEPTANCE_MATRIX.md) | Cross-slice acceptance |
| [`06_TARGET_FILE_MAP.md`](./06_TARGET_FILE_MAP.md) | Canonical paths per repo |
| [`07_CROSS_REPO_VALIDATION_PLAN.md`](./07_CROSS_REPO_VALIDATION_PLAN.md) | Commands and evidence |
| [`08_RISK_REGISTER.md`](./08_RISK_REGISTER.md) | Risks and mitigations |
| [`10_CLOSEOUT_TEMPLATE.md`](./10_CLOSEOUT_TEMPLATE.md) | Final closeout |

## Execution assets

- **Phased prompts:** [`prompts/`](./prompts/) (`P01`–`P08`)
- **Per-repo tasks:** [`repo-tasks/`](./repo-tasks/)
- **Per-repo Cursor prompts:** [`repo-prompts/`](./repo-prompts/)
- **Shared contracts:** [`contracts/`](./contracts/)
- **Preflight script:** [`scripts/validate-backend-coordination-002-preflight.ps1`](./scripts/validate-backend-coordination-002-preflight.ps1)

## Boundary (frozen)

```text
Platform  = neutral mechanics
Conexus   = model access
Kanon     = semantic authority
Allagma   = governed execution
Metabole  = data transformation spine
Aisthesis = perception / evidence / reconstructability
```
