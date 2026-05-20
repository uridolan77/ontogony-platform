# System dashboard and SLO pack — canonical entry (SYSTEM-DASH-002)

**Status:** Alpha operator pack for the **three-node runtime** (Allagma + Kanon + Conexus).  
**Canonical owner (docs entrypoint):** `ontogony-platform` (this file).  
**Implementation assets:** `allagma-dotnet` (compose, Grafana JSON, verifier, SLO/panel runbooks).

This is **not** production SLO governance. Targets are for operator discussion and regression detection until baselined in your environment.

---

## What to read first

| Document | Repo | Purpose |
| --- | --- | --- |
| [SYSTEM_SLO_STARTER_PACK.md](https://github.com/uridolan77/allagma-dotnet/blob/main/docs/operations/SYSTEM_SLO_STARTER_PACK.md) | `allagma-dotnet` | SLO ids, alpha vs production targets, measurement sources |
| [SYSTEM_DASH_PANEL_PACK.md](https://github.com/uridolan77/allagma-dotnet/blob/main/docs/operations/SYSTEM_DASH_PANEL_PACK.md) | `allagma-dotnet` | Panel map: operator need → Grafana section → PromQL/SQL → SLO id |
| [SYSTEM_OBSERVABILITY_PACK.md](https://github.com/uridolan77/allagma-dotnet/blob/main/docs/operations/SYSTEM_OBSERVABILITY_PACK.md) | `allagma-dotnet` | OTLP stack architecture, metric names, privacy guardrails |
| [SYSTEM_ALERT_RULES.md](https://github.com/uridolan77/allagma-dotnet/blob/main/docs/operations/SYSTEM_ALERT_RULES.md) | `allagma-dotnet` | Starter Prometheus alert rules |
| [SYSTEM_OBSERVABILITY_VERIFICATION.md](https://github.com/uridolan77/allagma-dotnet/blob/main/docs/operations/SYSTEM_OBSERVABILITY_VERIFICATION.md) | `allagma-dotnet` | Live verifier contract (`verify-system-observability.ps1`) |

**Platform mechanics (per-service OTLP, not three-node dashboard):** [`docs/observability/operations-pack.md`](../observability/operations-pack.md), [`metrics-catalog.md`](../observability/metrics-catalog.md).

**Evidence:** `SYSTEM-DASH-001` — [`allagma-dotnet/docs/evidence/SYSTEM_DASH_001_EVIDENCE.md`](https://github.com/uridolan77/allagma-dotnet/blob/main/docs/evidence/SYSTEM_DASH_001_EVIDENCE.md).  
**Evidence:** `SYSTEM-DASH-002` — [`docs/evidence/SYSTEM_DASH_002_EVIDENCE.md`](../evidence/SYSTEM_DASH_002_EVIDENCE.md).

---

## Implementation assets (stay in `allagma-dotnet`)

| Asset | Path |
| --- | --- |
| Grafana dashboard JSON | `allagma-dotnet/dashboards/grafana/ontogony-alpha-runtime.json` |
| Observability compose | `allagma-dotnet/docker-compose.observability.yml` |
| Collector / Prometheus / Grafana provisioning | `allagma-dotnet/docker/observability/` |
| Live verifier | `allagma-dotnet/scripts/verify-system-observability.ps1` |
| Local API + OTLP helper | `allagma-dotnet/scripts/run-local-stack.ps1` (`-EnableOtlpExport`, `-StartObservability`) |

Do not duplicate these files into `ontogony-platform` unless a future ticket explicitly migrates hosting. **SYSTEM-DASH-002** centralizes **where operators start**, not where binaries live.

---

## Provisioning from the platform local stack

The [Docker local working system](../../docker/local-working-system/README.md) runs Postgres + Kanon + Conexus + Allagma + frontend on host ports **5081** / **5082** / **5083** / **5175**. The observability stack (Grafana/Jaeger/Prometheus/collector) is a **separate compose** in `allagma-dotnet`, started from platform scripts.

### 1. Start the runtime (platform)

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\start-local-working-system.ps1 -Build
.\docker\local-working-system\scripts\wait-local-working-system.ps1
```

Optional seed for traffic and route evidence:

```powershell
.\docker\local-working-system\scripts\seed-and-verify-local-working-system.ps1
```

### 2. Start observability (platform → allagma compose)

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\start-observability-stack.ps1
```

Defaults: Grafana `http://localhost:3000`, Jaeger `http://localhost:16686`, Prometheus `http://localhost:9090`, OTLP gRPC `localhost:4317`. Override Grafana port: `-GrafanaHostPort 3001`.

Dashboard: **Ontogony → Ontogony Alpha Runtime** (provisioned from `allagma-dotnet/dashboards/grafana/`).

### 3. Enable OTLP on Docker-local APIs

Docker-local APIs do not enable OTLP export by default. For metrics in Grafana while using the platform compose stack, either:

**A — Rebuild/run APIs with OTLP** (document in Allagma/Kanon/Conexus compose or env): set `OTEL_EXPORTER_OTLP_ENDPOINT=http://host.docker.internal:4317` (Windows/Mac Docker) or the host gateway IP on Linux, plus `OTEL_SERVICE_NAME` per service.

**B — Headless local APIs with OTLP** (Allagma script, host ports must match Docker-local **5081–5083**):

```powershell
cd C:\dev\allagma-dotnet
.\scripts\run-local-stack.ps1 -EnableOtlpExport `
  -KanonBaseUrl http://localhost:5081 `
  -ConexusBaseUrl http://localhost:5082 `
  -AllagmaBaseUrl http://localhost:5083
```

Stop host processes on those ports first if the Docker stack is already bound (see [local-working-system troubleshooting](../../docker/local-working-system/README.md#troubleshooting)).

### 4. Verify provisioning (optional)

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\verify-observability-stack.ps1
```

Or full live gate from Allagma (starts compose + headless APIs if not skipped):

```powershell
cd C:\dev\allagma-dotnet
.\scripts\verify-system-observability.ps1 -UseExistingServices `
  -KanonBaseUrl http://localhost:5081 `
  -ConexusBaseUrl http://localhost:5082 `
  -AllagmaBaseUrl http://localhost:5083
```

---

## Endpoints (default profile)

| UI | URL | Notes |
| --- | --- | --- |
| Grafana | `http://localhost:${GRAFANA_HOST_PORT:-3000}` | Anonymous viewer; admin `admin`/`admin` for edits |
| Jaeger | `http://localhost:16686` | Cross-service traces when OTLP export is on |
| Prometheus | `http://localhost:9090` | Scrapes collector `:9464` |
| Collector metrics | `http://localhost:9464/metrics` | Direct fallback when Prometheus ingest is degraded |
| Docker-local APIs | `5081` / `5082` / `5083` | Kanon / Conexus / Allagma |
| Operator frontend | `http://localhost:5175` | `ontogony-frontend` in platform compose |

---

## Documented gaps (Sprint 3.5: `SYSTEM-OBS-METERS-001`)

| Gap | Current workaround |
| --- | --- |
| `/ready` not in Prometheus scrape | Orchestrator probes; smoke scripts; HTTP traffic as weak proxy |
| Kanon plan-compile dedicated meters | HTTP duration + Allagma `CompilePlan` integration histogram |
| Token/cost panels | SQL on `conexus_llm_response`; OTEL cost counters when stable |
| Dashboard ownership split | This index in platform; assets in Allagma until a future migration |

---

## Related operator docs

- [Docker local working system](../../docker/local-working-system/README.md)
- [Trace/correlation contract](../operators/TRACE_CORRELATION_CONTRACT.md)
- [Failure triage (Allagma)](https://github.com/uridolan77/allagma-dotnet/blob/main/docs/operations/FAILURE_TRIAGE_INDEX.md)
