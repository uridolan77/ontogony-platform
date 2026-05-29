# Manual regression retirement report

Generated: 2026-05-30 (ONTOGONY-SYSTEM-TEST-HARNESS-001 first implementation pass)

Harness location: `docs/_incoming/ONTOGONY-SYSTEM-TEST-HARNESS-001/ONTOGONY-SYSTEM-TEST-HARNESS-001/`

Run (five-service stack on locked ports 5081–5085):

```powershell
cd C:\dev\ontogony-platform\docs\_incoming\ONTOGONY-SYSTEM-TEST-HARNESS-001\ONTOGONY-SYSTEM-TEST-HARNESS-001\tests\dotnet
dotnet test Ontogony.SystemTestHarness.sln --filter "Category=Readiness|Category=SystemE2E"
```

Prerequisites: Kanon, Conexus (fake providers only), and Allagma running with dev tokens from `.env.example`. No real tool execution or external LLM providers.

## Retired or ready to retire (automated replacement exists)

| Manual check | Prior script / habit | Harness test ID | Retire? | Notes |
|---|---|---|---|---|
| Local stack health before RC | `Wait-Healthy` in multiple smokes | `READINESS-*` health + ready | **Yes** | Hard fail if `/health` or `/ready` not 2xx on 5081–5083 |
| Governed Allagma → Kanon → Conexus first loop | `scripts/lib/backend-system-e2e-001.ps1`, `smoke-first-real-system.ps1` happy path | `E2E-001` | **Yes** | Asserts `Completed`, planning/model refs, ordered milestone events |
| Run start idempotency replay | `FirstWorkingSystemApiTests` / cohesion POST replay | `E2E-002`, `E2E-002b` | **Yes** | Same key replays run id; different payload → `allagma.idempotency.conflict` |
| Human gate approve → resume → complete | `smoke-first-real-system.ps1` gate approve | `E2E-003a` | **Yes** | Requires `gaming-core@0.1.0`, `[maf-consequential-probe]`, PaymentsOperator resolve |
| Human gate deny → no Conexus model | `smoke-first-real-system.ps1` gate deny | `E2E-003b` | **Yes** | Asserts `Denied`, no `ConexusModel*` events after gate |
| Kanon Conexus assistance draft + redaction | `smoke-kanon-assistance-conexus.ps1`, Kanon unit tests | `E2E-004` | **Yes** | Fails fast with 503 if assistance disabled; asserts `draft_only`, no `secret-live-key` in body |
| Conexus fake fallback chain | `system-cohesion-e2e.ps1` fallback probe | `E2E-005` | **Yes** | Upserts `fake-fast` alias; expects `[fallback-route]`; no external providers |
| Correlation / trace on run | Manual inspect run JSON | `E2E-010` | **Yes** | Asserts trace or correlation id present |
| Conexus non-streaming chat on fake alias | Ad-hoc curl to `/v1/chat/completions` | `CONEXUS-CHAT-001` | **Yes** | Uses `risk-summary-v0` |
| Streaming + idempotency rejection | Manual negative check | `CONEXUS-STREAM-IDEM-001` | **Yes** | Expects 4xx |

## Partial replacement (keep manual until next pass)

| Manual check | Harness coverage | Keep manual because |
|---|---|---|
| UI console service coverage | Playwright skeleton only (`UI-*`) | Frontend routes not calibrated; no stable operator journey assertions |
| Metabole transformation / SLOD pipeline | Readiness health only | Real routes exist (`/metabole/v0/pipeline-runs`) but no deterministic E2E fixture yet |
| Aisthesis memory write/query | Readiness health only | API is `/aisthesis/v0/*` evidence routes, not placeholder `/memories` |
| Restart survival (process kill) | Not in harness (`RUN_RESTART_TESTS=false`) | Requires `PostgresRestartSurvivalE2ETests` / interactive restart |
| Postgres persistence / migration smoke | Not in harness | Still owned by service-repo persistence smokes |
| Observability dashboards (Grafana/Jaeger) | Not in harness | Metrics/log assertions deferred |
| Real provider live certification | Explicitly off (`RUN_EXTERNAL_PROVIDER_TESTS=false`) | By design |

## Should remain manual

- Exploratory UX and visual polish (see `docs/11_MANUAL_TEST_REPLACEMENT_CHECKLIST.md`)
- Product/semantic judgment on ontology content
- New feature acceptance before contracts stabilize
- Security review for new trust boundaries
- Philosophy/coherence review for Aisthesis phenomenological semantics

## Superseded scripts (candidate deprecation after one green CI week)

After harness is promoted from `_incoming` and runs green in CI:

1. `allagma-dotnet/scripts/smoke-first-real-system.ps1` — core scenarios covered by `E2E-001`–`E2E-003b`
2. `allagma-dotnet/scripts/lib/backend-system-e2e-001.ps1` — subset covered by `E2E-001` (keep for artifact schema until harness emits same JSON)
3. Ad-hoc `curl` readiness loops before demos — replaced by `Category=Readiness`

Do **not** retire yet:

- `run-local-stack.ps1` / docker compose boot (harness does not start services)
- `run-allagma-cohesion-option-a.ps1` (broader cohesion scenarios: streaming, restart, topology)
- Provider live certification harnesses in Conexus

## Evidence

Every E2E test writes `ontogony-system-test-harness-evidence-v1` JSON under `ONTOGONY_EVIDENCE_DIR` (default `./evidence/local`).

## Calibration sources

| Service | Source consulted |
|---|---|
| Allagma | `StartAgentRunRequest`, `FirstWorkingSystemApiTests`, `smoke-first-real-system.ps1` |
| Kanon | `ActionEndpoints`, `ConexusAssistanceContracts`, `KanonAuthorityTests` |
| Conexus | `FallbackEvidencePackAcceptanceTests`, port lock 5082 |
| Metabole / Aisthesis | `launchSettings.json`, route catalogs (health-only in harness) |
