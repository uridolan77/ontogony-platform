# ALLAGMA-ACTION-002 — Human gate resume workbench evidence (platform)

**Recorded at (UTC):** 2026-05-20  
**Verdict:** **PASS**  
**Cross-repo index:** [`ALLAGMA_ACTION_000_CURRENT_STATE_AUDIT.md`](../reviews/ALLAGMA_ACTION_000_CURRENT_STATE_AUDIT.md)

## Summary

Operator can resume governed Allagma runs waiting on human gate from the frontend using `POST /allagma/v0/runs/{runId}/resume`, with Kanon gate resolve + resume on the Human Gates workbench. No backend contract changes required; Allagma already returns `not_waiting` as 409 with `ResumeAgentRunResponse` body.

## Frontend evidence

Primary: `ontogony-frontend/docs/evidence/ALLAGMA_ACTION_002_HUMAN_GATE_RESUME_WORKBENCH_EVIDENCE.md`

## Platform touchpoints

| Concern | Notes |
| --- | --- |
| Local stack | `docker/local-working-system` — Kanon 5081, Allagma 5083 |
| Resume route | `POST /allagma/v0/runs/{runId}/resume` — outcomes per `ResumeAgentRunService` |
| List filter | `GET /allagma/v0/runs?waitingForHumanGate=true` |

## Hardening (002A)

Approve/deny in Human Gates require a confirmed `humanGateId` from pause events or run metadata. The UI no longer falls back to the synthetic queue id (`gate-{runId}`). Resume-only remains when the run is still waiting.

## Roadmap position

```text
ACTION-000 audit — complete
ACTION-001 start run — complete
ACTION-002 human gate resume — complete (002A gate-id guard)
ACTION-003 baseline compare — next
```
