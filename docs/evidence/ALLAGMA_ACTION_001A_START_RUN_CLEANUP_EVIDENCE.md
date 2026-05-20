# ALLAGMA-ACTION-001A — Start run cleanup evidence

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS**  
**Parent:** ALLAGMA-ACTION-001

## Changes

| Area | Change |
| --- | --- |
| Result navigation | `resolveStartRunNextAction` + tests (`blocked`, human gate, dependency wait, failed, completed) |
| UX copy | Streaming preset renamed; preset-only context note; reset-from-settings button |
| Submit | `try/catch` around `mutateAsync` |
| OpenAPI | `AgentRunResponse` required fields include `ontologyVersionId`, `actorId` |
| Conexus aliases | Allagma `ModelPurposes` → `risk-summary-v0` / `risk-summary-stream-v0`; local seed registers semantic aliases |
| Platform index | `docs/evidence/README.md` Allagma actionability section; [ALLAGMA_ACTION_001_CLOSEOUT.md](../reviews/ALLAGMA_ACTION_001_CLOSEOUT.md) |

## Validation

```text
npm run typecheck (ontogony-frontend)
vitest: src/allagma/startRun, allagmaMutations.startRun, lifecycle capability
npx playwright test e2e/allagma-start-run.spec.ts
```

## Deferred (not 001A)

- Advanced JSON context editor on start-run form
- Live streaming chunk viewer in UI
