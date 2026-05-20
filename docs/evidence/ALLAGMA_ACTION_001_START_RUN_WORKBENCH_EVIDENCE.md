# ALLAGMA-ACTION-001 — Start run workbench evidence (platform)

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS**  
**Cross-repo index:** [`ALLAGMA_ACTION_000_CURRENT_STATE_AUDIT.md`](../reviews/ALLAGMA_ACTION_000_CURRENT_STATE_AUDIT.md)

## Summary

Operator can start a governed Allagma run from the frontend using the existing `POST /allagma/v0/runs` contract. Implementation is in `ontogony-frontend`; OpenAPI contract fields `modelPurpose` and extended `AgentRunResponse` were added to `allagma-dotnet/docs/api/allagma-openapi-v1.snapshot.json` for alignment.

## Frontend evidence

Primary: `ontogony-frontend/docs/evidence/ALLAGMA_ACTION_001_START_RUN_WORKBENCH_EVIDENCE.md`

## Platform touchpoints

| Concern | Notes |
| --- | --- |
| Local stack | `docker/local-working-system` — Kanon 5081, Conexus 5082, Allagma 5083 |
| Idempotency header | `X-Ontogony-Idempotency-Key` (Ontogony.Http) |
| First-working-system body | `gaming-core@0.1.0`, player context in `context.playerId` |

## Roadmap position

```text
ACTION-000 audit — complete
ACTION-001 start run — complete
ACTION-001A cleanup — complete (see ALLAGMA_ACTION_001A_START_RUN_CLEANUP_EVIDENCE.md)
ACTION-002 human gate resume — complete
ACTION-003 baseline compare — next
```

## Closeout review

[ALLAGMA_ACTION_001_CLOSEOUT.md](../reviews/ALLAGMA_ACTION_001_CLOSEOUT.md)
