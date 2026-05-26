# Cursor unpack prompt — RUNTIME-CONFIG-001

You are implementing `RUNTIME-CONFIG-001` for the Ontogony local operator system.

First read every file in this unpacked package, then inspect the actual current repos before changing code:

- `C:\dev\ontogony-frontend`
- `C:\dev\ontogony-platform`
- `C:\dev\ontogony-ui`
- `C:\dev\conexus-dotnet`
- `C:\dev\kanon-dotnet`
- `C:\dev\allagma-dotnet`

Goal: make the Ontogony operator frontend runtime-configurable and make local/dev/docker environment profiles clearer. A Docker-served/static frontend must be able to point to different Conexus/Kanon/Allagma URLs through a runtime config file without rebuilding the frontend image.

Scope boundaries:

- This is not production security work.
- This is not OIDC/IAM work.
- This is not a rewrite of operator settings.
- Runtime config provides defaults, not hidden authority.
- Browser-local settings and session settings may override runtime defaults, but the UI must clearly show provenance.
- Do not put raw provider secrets, service tokens, admin API keys, project API keys, OpenAI keys, or other secrets in runtime config.
- Do not add silent live-to-fixture fallback.
- Preserve System Truth, Evidence Spine, Domain Switcher, Agent Interaction, governed fake E2E, runtime lock, Playwright docker-live, and contract discipline.

Implementation order:

1. Audit current code against `01_CURRENT_STATE_AUDIT.md`; update the audit if local code differs.
2. Implement `02_RUNTIME_CONFIG_CONTRACT.md` and `04_FRONTEND_BOOTSTRAP_PLAN.md` in `ontogony-frontend`.
3. Implement the precedence/provenance model from `03_CONFIG_PRECEDENCE_AND_PROVENANCE.md`.
4. Update Settings UI according to `06_OPERATOR_SETTINGS_MIGRATION_PLAN.md`, using canonical `@ontogony/ui` patterns.
5. Update Docker/nginx/local-working-system according to `05_DOCKER_NGINX_AND_LOCAL_WORKING_SYSTEM_PLAN.md`.
6. Add tests and smoke updates from `07_TEST_SMOKE_AND_PLAYWRIGHT_PLAN.md`.
7. Update docs and compatibility artifacts according to `08_DOCS_AND_PROFILE_GUIDE.md` and `09_CONTRACT_DISCIPLINE_AND_COMPATIBILITY_PLAN.md`.
8. Run the verification commands in `10_ACCEPTANCE_CHECKLIST.md`.
9. Produce a short implementation report that answers every item in `12_REVIEW_PROMPT.md`.

Important implementation posture:

- Prefer small, reviewable commits/stages.
- Preserve existing local development defaults.
- Do not delete current operator settings behavior; migrate it conservatively.
- Do not overbuild a new environment management system.
- Make source/provenance understandable in Settings without badge overload.
