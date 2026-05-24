# KANON-CONSOLE-POLISH-001 — active workstream

**Status:** implemented (2026-05-24)  
**Primary repo:** `ontogony-frontend`, `ontogony-ui` (credential source labels)

## Delivered

- Assistance safe defaults + regression tests (KCP-001)
- Domain-pack inventory separation, generated-name warning, inline action reasons (KCP-002)
- Settings health role presets + browser-local/session credential source (KCP-003)
- Review queue / policies `PartialStateReason` (KCP-004)
- Contextual Kanon decision + Evidence Spine link labels (KCP-005)
- Unit tests + operator patterns in polish module (KCP-006)

## Verify

```powershell
cd C:\dev\ontogony-frontend
npm run typecheck
npm test -- --run src/kanon src/app/settings/credentialPersistenceCopy.test.ts
cd C:\dev\ontogony-ui
npm test -- --run src/components/security
```

## Next

`SOURCE-BINDINGS-POLISH-001` per [`NEXT.md`](../NEXT.md).
