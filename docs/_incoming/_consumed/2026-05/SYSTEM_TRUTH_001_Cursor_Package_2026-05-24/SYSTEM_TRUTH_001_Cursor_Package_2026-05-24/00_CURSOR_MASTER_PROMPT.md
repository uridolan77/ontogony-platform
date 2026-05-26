# Cursor Master Prompt — SYSTEM-TRUTH-001

You are working in the Ontogony local-alpha multi-repo system.

## Goal

Implement **SYSTEM-TRUTH-001**: make the operator console truthful and non-misleading about health, readiness, service version alignment, compatibility, fixture/demo/generated data, and release-readiness posture.

The current operator surface is functionally impressive but inconsistent. It shows services as live/healthy while also showing health payload warnings, Conexus strict readiness not ready, missing service version metadata, missing compatibility artifact, fixture/demo readiness, and optimistic "ready" language. Your job is to turn that into a reliable operator-grade truth cockpit.

## Repos likely involved

Use the local checkout layout:

```text
C:\dev\ontogony-platform
C:\dev\conexus-dotnet
C:\dev\kanon-dotnet
C:\dev\allagma-dotnet
C:\dev\ontogony-ui
C:\dev\ontogony-frontend
```

Prefer small, testable commits. Do not over-expand the scope.

## Non-negotiable principles

1. **Healthy is not ready.**
2. **Ready is not release-ready.**
3. **Fixture/demo/generated is not live evidence.**
4. **Unknown is not green.**
5. **Partial must say what is missing.**
6. **Every green badge must be backed by live or generated evidence whose source is explicit.**
7. **The operator must never infer from contradictory text.**

## First actions in Cursor

1. Read this file.
2. Read `01_CURRENT_DIAGNOSIS.md`.
3. Read `02_TARGET_STATE.md`.
4. Read `03_BACKEND_PLAN.md`.
5. Read `04_FRONTEND_PLAN.md`.
6. Run or adapt the smoke scripts under `scripts/smoke/`.
7. Implement backend contracts first, then frontend parsing/visual taxonomy, then compatibility artifact generation.

## Scope boundary

This package is only SYSTEM-TRUTH-001.

Do not implement:
- full Evidence Spine graph completion,
- Kanon source-binding UI cleanup,
- Agent Interaction live message/tool/gate timeline,
- evaluation metadata expansion,
- domain-switcher UX.

You may create TODO hooks for those, but do not bury the truth-hardening sprint under adjacent work.
