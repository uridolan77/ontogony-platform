# Grafana dashboard import (PLAT-9-005)

## Starter dashboard

| File | UID | Purpose |
| --- | --- | --- |
| [`grafana-dashboard-starter.json`](./grafana-dashboard-starter.json) | `ontogony-observability-mechanics` | Cross-service HTTP + integration overview with Allagma/Kanon/Conexus rows |

## Prerequisites

- Prometheus scraping service `/metrics` endpoints (local stack: Allagma observability compose or equivalent).
- Grafana with a Prometheus datasource — default uid **`prometheus`** (matches starter JSON).

Extended three-node dashboard (more panels): `allagma-dotnet/dashboards/grafana/ontogony-alpha-runtime.json`.

## Import steps

1. Open Grafana → **Dashboards** → **New** → **Import**.
2. Upload `grafana-dashboard-starter.json` or paste JSON.
3. Select Prometheus datasource when prompted.
4. Save dashboard.

## Verify queries return data

With docker-local APIs on **5081** (Kanon), **5082** (Conexus), **5083** (Allagma):

```powershell
# Example: confirm platform HTTP counter exists
curl -s http://localhost:5083/metrics | Select-String "ontogony_http_server_request_count"
```

Generate traffic via platform trace script:

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\inspect-trace-correlation-evidence.ps1
```

## Service label reference

| Service | `service` tag on `ontogony.http.server.*` |
| --- | --- |
| Allagma | `allagma-api` |
| Conexus | `conexus-dotnet` |
| Kanon | `Kanon.Api` |

Product-specific meters (`allagma.*`, `conexus.*`, `kanon.*`) use each repo's catalog — see [metrics-catalog.md](../metrics-catalog.md).
