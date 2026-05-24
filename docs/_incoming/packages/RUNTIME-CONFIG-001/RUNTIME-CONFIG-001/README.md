# RUNTIME-CONFIG-001 — Ontogony Operator Runtime Configuration

Prepared: 2026-05-24T20:19:16Z

This package is a Cursor-ready development package for making the Ontogony operator frontend runtime-configurable and making local/dev/docker environment profiles clearer.

The package is intentionally **not** production security work, not OIDC/IAM work, and not a replacement of the existing operator settings system. It introduces runtime configuration as a default-source layer below browser-local/session operator settings, so the same Docker/static frontend image can point to different Conexus, Kanon, and Allagma service URLs without rebuilding.

## Reviewed current state

Repos reviewed through the current GitHub-accessible state:

- `uridolan77/ontogony-frontend`
- `uridolan77/ontogony-platform`
- `uridolan77/ontogony-ui`
- `uridolan77/conexus-dotnet`
- `uridolan77/kanon-dotnet`
- `uridolan77/allagma-dotnet`

The audit explicitly accounts for current system truth/health surfaces, operator settings, domain switcher, evidence spine, agent interaction, governed fake E2E, runtime-lock evidence, contract discipline, console canonicalization, Docker local-working-system workflows, and Playwright docker-live tests.

## Package contents

| File | Purpose |
|---|---|
| `00_UNPACK_PROMPT.md` | Paste into Cursor after unpacking. |
| `RUNTIME-CONFIG-001.md` | Main development brief and staged execution plan. |
| `01_CURRENT_STATE_AUDIT.md` | Repo-by-repo audit of current config behavior. |
| `02_RUNTIME_CONFIG_CONTRACT.md` | Runtime config JSON contract and validation expectations. |
| `03_CONFIG_PRECEDENCE_AND_PROVENANCE.md` | Exact precedence order and source/provenance semantics. |
| `04_FRONTEND_BOOTSTRAP_PLAN.md` | Frontend loader, provider, merge, and client integration plan. |
| `05_DOCKER_NGINX_AND_LOCAL_WORKING_SYSTEM_PLAN.md` | Docker/nginx/runtime config injection plan. |
| `06_OPERATOR_SETTINGS_MIGRATION_PLAN.md` | Settings UI and storage migration plan. |
| `07_TEST_SMOKE_AND_PLAYWRIGHT_PLAN.md` | Vitest, Playwright, docker-live, governed fake, and smoke plan. |
| `08_DOCS_AND_PROFILE_GUIDE.md` | Documentation plan for local/docker/custom profiles. |
| `09_CONTRACT_DISCIPLINE_AND_COMPATIBILITY_PLAN.md` | Contract discipline impact plan. |
| `10_ACCEPTANCE_CHECKLIST.md` | Implementation acceptance checklist. |
| `11_RISK_REGISTER.md` | Risks, mitigations, and anti-patterns. |
| `12_REVIEW_PROMPT.md` | Post-implementation review prompt. |
| `manifest.json` | Machine-readable package summary. |

## Recommended implementation order

1. Land the runtime config contract and loader with safe fallback behavior.
2. Thread runtime defaults into operator settings read/merge without breaking existing local storage.
3. Add provenance display and reset-to-runtime controls to the existing Settings page.
4. Replace Docker build-time frontend service URL args with runtime config generation/mounting.
5. Update Playwright helpers and smoke scripts to make overrides explicit.
6. Update docs and contract-discipline artifacts only where actual UI/catalog behavior changes.

## Hard boundaries

- Do not put raw provider API keys or service secrets in runtime config.
- Do not change backend route contracts unless a concrete smoke/test path proves it is unavoidable.
- Do not add automatic live-to-fixture fallback.
- Do not create a new environment-management page unless Settings cannot support the workflow after implementation.
- Keep local-alpha credential warnings accurate and visible.
