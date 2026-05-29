# Frontend live evidence-spine validation

## Goal

Move from handoff-only to route-compatible UI validation.

## Required UI capabilities

- Search/lookup by traceId.
- Timeline display.
- Graph display.
- Reconstructability v2 summary.
- Required-edge status table.
- Missing/ambiguous edge diagnostics.
- Suggested producer fix display.
- Producer/native-ID filters.
- Bundle export/download trigger.
- Auth/config state display.

## Frontend contract smoke

A script should verify that frontend config can reach Aisthesis and that route assumptions match:

```powershell
./scripts/system/run-frontend-aisthesis-contract-smoke.ps1   -FrontendRoot C:\Dev\ontogony-frontend   -AisthesisBaseUrl http://localhost:5084
```

## If not implemented

Close as:

```yaml
frontendStatus: HANDOFF_ONLY
rcBlocker: false|true
reason:
owner: ontogony-frontend
```

Do not claim frontend live backing until tested against running Aisthesis.
