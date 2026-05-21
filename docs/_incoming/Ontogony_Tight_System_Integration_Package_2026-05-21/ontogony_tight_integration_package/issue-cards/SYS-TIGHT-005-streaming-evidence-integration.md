# SYS-TIGHT-005 — Streaming evidence integration

**Repo:** allagma-dotnet, conexus-dotnet, ontogony-frontend  
**Type:** integration + UI  
**Priority:** P1

## Goal

Make opt-in streaming auditable without storing streamed payload by default.

## Scope

- Confirm stream lifecycle events in audit bundle.
- Add operator stream panel.
- Add smoke with `Stream=true`, `PersistStreamedOutput=false`.

## Acceptance

- Stream start/chunk/completed/interrupted lifecycle visible.
- Payload not persisted by default.
- Package-mode build verifies Conexus streaming client contract.
