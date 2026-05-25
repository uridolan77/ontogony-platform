# GOVERNED-FAKE-REPLAY-E2E-001 — cross-service replay closure (PASS)

**Status:** CLOSED  
**Recorded:** 2026-05-25T08:19:56Z  
**Package:** REPLAY-RUNTIME-PROOF-LOCK-001 / REPLAY-RUNTIME-005

---

## Environment

| Service | URL |
| --- | --- |
| Kanon | `http://localhost:5081` |
| Conexus | `http://localhost:5082` |
| Allagma (replay routes) | `http://localhost:5084` |

Note: Allagma on `:5083` in this workspace was an older build without `/allagma/v0/replay/*`; replay smoke targets `:5084` when present.

---

## Commands

```powershell
powershell -NoProfile -File c:\dev\ontogony-platform\scripts\smoke\run-runtime-lock-governed-fake-replay-e2e.ps1 -AllagmaBaseUrl http://localhost:5084

cd c:\dev\allagma-dotnet
./scripts/validate-runtime-lock.ps1 -RequireGovernedFakeReplayEvidence
```

Combined with governed-fake E2E (platform orchestrator):

```powershell
powershell -NoProfile -File c:\dev\ontogony-platform\scripts\smoke\run-runtime-lock-governed-fake-e2e.ps1 -IncludeReplay
```

---

## Identifiers

| Field | Value |
| --- | --- |
| runId | `run_36871f291f5b40fa80b55c25b873dc1f` |
| replayId | `replay_8f02b80caa5c4af89932dddcada01f29` |
| traceId | `gov-fake-trace-5aa1581a10554855b3522358d5d2184e` |
| correlationId | `gov-fake-corr-a81de082e368494ebed9c2536a608ada` |
| planningDecisionId | `decision_94f45315e84b430aa0a65cde794b044a` |
| modelCallId | `chatcmpl-0HNLPUBGJJKQM-00000001` |
| routeDecisionId | `rd-0HNLPUBGJJKQM-00000001` |

---

## Verdict

| Gate | Result |
| --- | --- |
| Replay mode | `evidence_only` |
| Summary schema | `ontogony-governed-fake-replay-summary-v1` |
| Overall smoke verdict | **PASS** |
| Allagma service attempt | succeeded |
| Kanon service attempt | succeeded |
| Conexus service attempt | skipped (`conexus_not_configured_or_not_found` without admin key on Allagma) |
| Real providers blocked | true |
| Real tools blocked | true |
| Manifest fingerprint | matched |

---

## Preserved artifacts

- Platform: [`artifacts/governed-fake-replay-e2e/20260525T081956Z/`](./artifacts/governed-fake-replay-e2e/20260525T081956Z/)
- Runtime lock: `allagma-dotnet/docs/system/ontogony-runtime.lock.json` → `evidence.governedFakeReplaySummary`

Operator drilldown: `/allagma/replay?runId=run_36871f291f5b40fa80b55c25b873dc1f&replayId=replay_8f02b80caa5c4af89932dddcada01f29`
