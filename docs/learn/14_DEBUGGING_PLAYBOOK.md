# Debugging playbook

> **Audience:** operator, developer  
> **Applies to:** full stack  
> **Source of truth:** repo `TESTING.md`, platform Docker README  
> **Last verified:** 2026-05-25

## 1. Confirm the stack

```powershell
# Health
Invoke-WebRequest http://localhost:5081/health -UseBasicParsing
Invoke-WebRequest http://localhost:5082/health -UseBasicParsing
Invoke-WebRequest http://localhost:5083/health -UseBasicParsing
Invoke-WebRequest http://localhost:5084/health -UseBasicParsing
Invoke-WebRequest http://localhost:5085/health -UseBasicParsing
```

Docker: `.\docker\local-working-system\scripts\wait-local-working-system.ps1`

Port mismatch (**5083** vs **5084**) is a frequent smoke failure — align `AllagmaBaseUrl` with your compose map.

## 2. Trace a governed run

1. Note `runId`, `traceId`, `correlationId` from smoke output or console.
2. `GET /allagma/v0/runs/{runId}/events` — event timeline.
3. Evidence Spine — paste identifier ([05_EVIDENCE_SPINE.md](./05_EVIDENCE_SPINE.md)).
4. Agent Interaction workbench — workflow + tool intents ([06_AGENT_INTERACTION.md](./06_AGENT_INTERACTION.md)).

## 3. Collect evidence bundle (Allagma)

```powershell
cd C:\dev\allagma-dotnet
.\scripts\collect-run-evidence.ps1 -RunId <runId>
```

## 4. Common failures

| Symptom | Likely cause | Action |
| --- | --- | --- |
| `evidence_only not in eligibleModes` | Merged eligibility conflict (Conexus target missing) | Check per-service eligibility; replay may still work on allagma ([03](./03_GOVERNED_FAKE_E2E.md)) |
| 401 on Allagma | Wrong service token | Match `appsettings.Development.json` bearer |
| Conexus `skipped` in replay | No admin API key on Allagma | Expected in dev; use `-RequireConexusReplayAttempt` only when configured |
| Frontend stale on :5175 | Container not rebuilt | `verify-frontend-browser-provenance.ps1 -Build` |
| Idempotency 409 | Same key, different payload | Use new key or match original body |
| Kanon policy deny on tool | Missing domain policy | `smoke-first-system-tool-allowed.ps1` path |

## 5. Logs and correlation

- Propagate `X-Ontogony-Trace-Id` / correlation headers ([`../CONTRACTS.md`](../CONTRACTS.md)).
- Conexus: do not log raw prompts/responses by default.

## 6. Validate contracts / docs

```powershell
cd C:\dev\ontogony-frontend
npm run contracts:discipline

cd C:\dev\allagma-dotnet
.\scripts\check-doc-freshness.ps1

cd C:\dev\ontogony-platform
.\scripts\validate-learn-docs.ps1
```

## 7. Platform diagnostics (Docker)

| Script | Purpose |
| --- | --- |
| `inspect-trace-correlation-evidence.ps1` | Trace contract report |
| `inspect-kanon-topology-evidence.ps1` | Kanon topology |
| `diagnose-kanon-topology-ops.ps1` | Topology failures |

## References

- [`../operators/OPERATOR_V1_DEMO_GUIDE.md`](../operators/OPERATOR_V1_DEMO_GUIDE.md)
- [02_RUN_LOCAL_SYSTEM.md](./02_RUN_LOCAL_SYSTEM.md)
