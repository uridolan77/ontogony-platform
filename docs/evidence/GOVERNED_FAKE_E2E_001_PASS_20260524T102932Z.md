# GOVERNED-FAKE-E2E-001 — cross-repo closure (PASS)

**Status:** CLOSED  
**Recorded:** 2026-05-24T10:29:32Z (smoke) · 2026-05-24 (Docker-local Playwright confirmation)  
**Package:** `GOVERNED_FAKE_E2E_001_Cursor_Package_2026-05-24` → [`docs/_incoming/_consumed/2026-05/`](../_incoming/_consumed/2026-05/GOVERNED_FAKE_E2E_001_Cursor_Package_2026-05-24/)

---

## Environment

| Service | URL |
| --- | --- |
| Kanon | `http://localhost:5081` |
| Conexus | `http://localhost:5082` |
| Allagma | `http://localhost:5083` |
| Frontend (Docker) | `http://localhost:5175` |

SYSTEM-TRUTH smoke: **WARNING** (Conexus `not_ready` without optional real provider credentials; expected in local fake mode).

---

## Commands

```powershell
powershell -File c:\dev\ontogony-platform\scripts\smoke\system_truth_smoke.ps1
powershell -File c:\dev\allagma-dotnet\scripts\smoke\run-governed-fake-e2e.ps1
cd c:\dev\ontogony-frontend
npx playwright test -c playwright.docker-local.config.ts governed-fake-e2e-docker-live
```

Docker frontend rebuild (required once for Agent Interaction live-summary panels):

```powershell
cd c:\dev\ontogony-platform\docker\local-working-system
docker compose build ontogony-frontend
docker compose up -d ontogony-frontend
```

---

## Identifiers (2026-05-24T10:29:32Z smoke)

| Field | Value |
| --- | --- |
| runId | `run_4d40eaae95964b43af0f64cd97ef7eaf` |
| traceId | `gov-fake-trace-c69ecdeae15047ca844d3096b7e4ea8d` |
| correlationId | `gov-fake-corr-0310ce97d3a0413e8553df1a9ad82f95` |
| planningDecisionId | `decision_0013aeb7946a428c9f64d05b040cbcd1` |
| modelCallId | `chatcmpl-0HNLP48JDQU7O-00000001` |
| routeDecisionId | `rd-0HNLP48JDQU7O-00000001` |
| provider | `fake` / `fake.chat` |
| ontologyVersionId | `gaming-core@0.1.0` |
| modelPurpose | `summarize-player-risk` |

---

## Verdict

| Gate | Result |
| --- | --- |
| Allagma governed fake run | Completed |
| Kanon decision | Resolved |
| Conexus model call + route decision | `fake` / `fake.chat` via admin APIs |
| Evidence Spine (7 required node kinds) | PASS |
| Agent Interaction live lookup (not fixture replay) | PASS (3/3 Playwright tests after frontend image refresh) |

### Playwright (`governed-fake-e2e-docker-live`)

1. Start governed fake run via API and capture identifiers — **pass**
2. Evidence Spine resolves governed fake graph from run id — **pass**
3. Agent Interaction opens live run stream, not fixture replay — **pass** (requires current `ontogony-frontend` Docker image)

---

## Preserved artifacts

- Allagma: `allagma-dotnet/artifacts/governed-fake-e2e/20260524T102932Z/`
- Platform mirror: [`artifacts/governed-fake-e2e/20260524T102932Z/`](./artifacts/governed-fake-e2e/20260524T102932Z/)
- Earlier closure note: [`GOVERNED_FAKE_E2E_001_PASS_20260523T232255Z.md`](./GOVERNED_FAKE_E2E_001_PASS_20260523T232255Z.md)
- Allagma prior smoke: `allagma-dotnet/docs/evidence/GOVERNED_FAKE_E2E_001_PASS_20260523T232255Z.md`

---

## Implementing commits (reference)

| Repo | Notes |
| --- | --- |
| `allagma-dotnet` | Trace/correlation propagation, model purpose, Conexus alias persistence |
| `ontogony-frontend` | Live summary, provider panel, message/tool panels, docker-live E2E |

---

## Known caveats

- Local fake-provider mode only; not production IAM.
- Conexus may show `not_ready` in SYSTEM-TRUTH while fake path works.
- **RUNTIME-LOCK-CI-GOVERNED-E2E-001** (Stage 2–3): see [`docs/operators/RUNTIME_LOCK_GOVERNED_FAKE_E2E.md`](../operators/RUNTIME_LOCK_GOVERNED_FAKE_E2E.md) — one-command local proof, canonical `governed-fake-e2e-summary.json`, optional `-RequireGovernedFakeE2eEvidence`.

---

## Next workstream

**`AGENT-INTERACTION-LIVE-001`** — see [`docs/_incoming/_consumed/2026-05/AGENT-INTERACTION-LIVE-001.md`](../_incoming/_consumed/2026-05/AGENT-INTERACTION-LIVE-001.md) and [`ALLAGMA-AGENT-INTERACTION-001`](../_incoming/_consumed/2026-05/ALLAGMA-AGENT-INTERACTION-001/).
