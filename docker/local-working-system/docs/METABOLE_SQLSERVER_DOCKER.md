# Metabole SQL Server from Docker (local-working-system)

This stack can run Metabole in Docker and connect to a real SQL Server (for example ProgressPlay `DailyActionsDB`) when **configuration** and **network** are both correct.

## 1. Enable Metabole SQL Server features (compose)

In `.env` (copy from `.env.example`), ensure:

```env
ASPNETCORE_ENVIRONMENT=Development
METABOLE_CORS_ENABLED=true
METABOLE_SQLSERVER_ENABLED=true
METABOLE_SOURCE_CREDENTIALS_ENABLED=true
FRONTEND_CORS_ORIGIN_LOCALHOST=http://localhost:5175
```

`docker-compose.yml` already maps these into the `metabole-api` container. Rebuild and recreate after changing:

```powershell
cd C:\dev\ontogony-platform\docker\local-working-system
docker compose build metabole-api
docker compose up -d --force-recreate metabole-api
```

Verify:

```powershell
(Invoke-RestMethod http://localhost:5084/ready).sqlServerExtractor   # enabled
```

## 2. Provide the connection string (two options)

### Option A — UI registration (quick, not durable)

1. Open `http://localhost:5175/metabole/sources`.
2. Paste the connection string and **Save source** (registers connector + credential → `secretref:gaming/progressplay-dailyactionsdb`).
3. **Test connection** / Extract.

**Save source** only makes the connector appear in **Registered sources**; it does not prove connectivity until **Test connection** succeeds.

When the same `secretref` exists in both the UI store and `.env`, Metabole uses the **UI-saved** connection string for tests (so you can fix a password in the form without editing `.env`).

Credentials are stored **in memory** inside the container. **`docker compose up --force-recreate metabole-api` clears them** — save again after every recreate (or keep Option B `.env` binding).

### Option B — `.env` config binding (recommended for Docker)

Put the secret only in `.env` (never commit). Maps to `secretref:gaming/progressplay-dailyactionsdb`:

```env
# Key must match credential ref suffix: gaming/progressplay-dailyactionsdb
METABOLE_SQLSERVER_CREDENTIAL_GAMING_PROGRESSPLAY_DAILYACTIONSDB=Server=...;Database=DailyActionsDB;...
```

Add `TrustServerCertificate=True` if the server uses a non-trusted TLS cert.

Register the connector once in the UI **without** pasting the connection string again if you pre-bound the credential — create connector with the same `credentialRef` only.

## 3. Network: container → SQL Server

Metabole in Docker uses the **Docker Desktop VM’s outbound path** (NAT through your PC). The remote SQL firewall must allow that egress IP (often your office/home public IP), not `localhost`.

### Remote SQL (e.g. `185.64.x.x`)

- Allow **TCP 1433** from your machine’s public IP on the SQL firewall.
- If you need **VPN** to reach the DB, connect VPN on the host **before** testing. Some VPN clients do not route Docker traffic through the tunnel; if Test still fails from Docker but works from `dotnet run` on the host, run Metabole on the host for that workflow (see below).

### SQL Server on your Windows host

Use `host.docker.internal` in the connection string:

```text
Server=host.docker.internal,1433;Database=...;...
```

### Optional local SQL sandbox (same machine)

```powershell
cd C:\dev\metabole-dotnet
docker compose -f docker-compose.sqlserver.yml up -d
```

From `metabole-api` container, target `host.docker.internal,1433` (published port) or attach both compose files to a shared network.

## 4. SSMS works but Metabole Test connection fails

Metabole uses **Microsoft.Data.SqlClient** with the **SQL authentication** connection string you provide. It does **not** use SSMS Windows Authentication or saved SSMS profiles.

If Test fails but SSMS connects:

1. In SSMS, open **Connect** → **Options** → note whether you use **SQL Server Authentication** or **Windows Authentication**.
2. Metabole requires **SQL Server Authentication** with `User ID` / `Password` in the string (or a working SQL login).
3. In SSMS (SQL auth), use **Connect** → your database → **Properties** → copy the connection string, or script `CREATE LOGIN` and test that login explicitly.
4. Paste that string into **Sources** → **Connection string** → **Save source** → **Test connection**.

Quick check from your PC (same string Metabole uses — should not print secrets):

```powershell
cd C:\dev\ontogony-platform\docker\local-working-system
$cs = docker compose exec -T metabole-api printenv Metabole__SqlServer__Credentials__gaming/progressplay-dailyactionsdb
# Then test with SqlClient / sqlcmd using $cs — if login fails here, Docker is not the problem.
```

Common outcomes:

| Symptom | Likely cause |
|--------|----------------|
| `Login failed for user '...'` (18456) | Wrong password, wrong login, or SSMS was using Windows auth |
| `Cannot open database` (4060) | Wrong `Database` / `Initial Catalog` name |
| TCP unreachable from container only | Docker/VPN/firewall (see section 3) |

## 5. Smoke test from inside the container

```powershell
cd C:\dev\ontogony-platform\docker\local-working-system
.\scripts\test-metabole-sqlserver-egress.ps1
```

Or manually:

```powershell
docker compose exec metabole-api bash -lc "timeout 3 bash -c 'cat < /dev/null > /dev/tcp/185.64.56.157/1433' && echo OK || echo FAIL"
```

Replace host/port with your SQL endpoint.

## 5. When Docker cannot reach the DB

Run Metabole on the **host** (same port only if Docker Metabole is stopped):

```powershell
docker compose stop metabole-api
cd C:\dev\metabole-dotnet\src\Metabole.Api
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:Metabole__SqlServer__Enabled = "true"
$env:Metabole__SourceCredentials__Enabled = "true"
$env:Metabole__Storage = "InMemory"
dotnet run --urls http://localhost:5084
```

Frontend Metabole URL stays `http://localhost:5084`.

## Troubleshooting

| Symptom | Likely cause |
| --- | --- |
| `sqlServerExtractor=disabled` | `METABOLE_SQLSERVER_ENABLED` not true or container not recreated |
| Extract 400 `sqlserver.not_enabled` | Same as above |
| Test `failed` 0ms, safeReason network | SQL firewall / VPN / wrong host from container |
| Test 400 `credential_missing` | No UI save and no `.env` credential binding |
| CORS / unreachable in browser | `ASPNETCORE_ENVIRONMENT=Development`, frontend on 5175, rebuild API |
| Connector missing after recreate | In-memory store; re-save source or use `.env` credential |
