# Ontogony runtime port lock — v1

**Package:** `CONEXUS-BACKEND-INTEGRATION-HARDENING-001` (runtime port lock slice)  
**Owner:** `ontogony-platform` (local-dev / docker-local mechanical topology)  
**Status:** promoted (2026-05-29)

## Purpose

Freeze the **local-dev and docker-local host-port assignment** for the Ontogony
backend services so every repo's certification scripts, runtime lock, operator
runtime config, and producer `BaseUrl` defaults agree on a single mechanical
topology.

This contract is **mechanics, not meaning**. It does not describe model routing,
orchestration, semantic authority, or evidence ownership. It only fixes *which
host port reaches which service* in the local and docker-local profiles, and how
that maps onto the existing `{SERVICE}_HOST_PORT` / `{SERVICE}__BaseUrl` /
`ontogony.operator-runtime-config.v1` conventions.

It resolves a real, documented drift: some legacy combined-stack launchers bound
**Aisthesis on 5084** and **Metabole on 5085**. That layout is **deprecated**.

## Locked host-port matrix (local-dev / docker-local profile)

| Service | Host port | Container port | Health | Service id |
| --- | ---: | ---: | --- | --- |
| Kanon | 5081 | 8080 | `/health` | `kanon` |
| Conexus | 5082 | 8080 | `/health` | `conexus` |
| Allagma | 5083 | 8080 | `/health` | `allagma` |
| Metabole | 5084 | 8080 | `/health`, `/ready` | `metabole` |
| Aisthesis | 5085 | 8080 | `/health`, `/ready` | `aisthesis` |
| Frontend (dev) | 5173 | — | — | `frontend` |

This matrix is canonical for: five-service certification, runbooks, the Allagma
`ontogony-runtime.lock.json` `ports` block, the operator runtime config, and
Metabole → Aisthesis evidence emission defaults. It is identical to
[`metabole-dotnet/docs/system/ONTOGONY_FIVE_SERVICE_PORT_MATRIX.md`](../../metabole-dotnet/docs/system/ONTOGONY_FIVE_SERVICE_PORT_MATRIX.md)
and is the upstream lock that document defers to.

## Container vs host URL rules

- Inside a container each API listens on `ASPNETCORE_URLS=http://+:8080`.
- Host → container mapping is `"${<SERVICE>_HOST_PORT:-<port>}:8080"`.
- Inter-container calls use the compose service name, e.g.
  `Conexus__BaseUrl=http://conexus-api:8080`, never the host port.
- Host-process (non-docker) calls use `http://localhost:<host port>`.

## Environment variable mapping

| Convention | Example | Meaning |
| --- | --- | --- |
| `{SERVICE}_HOST_PORT` | `CONEXUS_HOST_PORT=5082` | docker-compose host port override |
| `{SERVICE}__BaseUrl` | `Conexus__BaseUrl=http://localhost:5082` | .NET options binding (host profile) |
| `{SERVICE}__BaseUrl` | `Conexus__BaseUrl=http://conexus-api:8080` | .NET options binding (container profile) |
| `VITE_{SERVICE}_BASE_URL` | `VITE_CONEXUS_BASE_URL=http://localhost:5082` | frontend dev consumption |

## Operator runtime config alignment

The locked matrix must equal the `services[].baseUrl` block of
`ontogony.operator-runtime-config.v1`:

```json
{
  "services": {
    "kanon":     { "baseUrl": "http://localhost:5081" },
    "conexus":   { "baseUrl": "http://localhost:5082" },
    "allagma":   { "baseUrl": "http://localhost:5083" },
    "metabole":  { "baseUrl": "http://localhost:5084" },
    "aisthesis": { "baseUrl": "http://localhost:5085" }
  }
}
```

## Override policy

- An operator may rebind any host port via `{SERVICE}_HOST_PORT`, but the
  **default** profile and all certification gates use the locked matrix.
- An alternate conflict-avoidance profile (e.g. `5181`–`5183`) is allowed for
  ad-hoc local work but is **not** a certification fallback.
- Binding Metabole on 5085 or Aisthesis on 5084 is **forbidden** for
  certification and must not be treated as a fallback.

## Deprecated legacy layout (do not use)

| Layout | Status |
| --- | --- |
| Aisthesis = 5084, Metabole = 5085 | **Deprecated.** Not certification-supported; not reflected in the runtime lock `ports`. |

## Verification

| Proof | Location |
| --- | --- |
| Canonical matrix (downstream mirror) | `metabole-dotnet/docs/system/ONTOGONY_FIVE_SERVICE_PORT_MATRIX.md` |
| Operator runtime config schema | `ontogony.operator-runtime-config.v1` (docker/local-working-system) |
| Compose host mappings | `docker/local-working-system/docker-compose.yml` |
| Runtime lock `ports` | `allagma-dotnet/docs/system/ontogony-runtime.lock.json` |
| Machine-readable matrix | [`docs/schemas/ontogony-runtime-port-lock-v1.json`](../schemas/ontogony-runtime-port-lock-v1.json) |
| Schema | [`docs/schemas/ontogony-runtime-port-lock-v1.schema.json`](../schemas/ontogony-runtime-port-lock-v1.schema.json) |

## Schema

[`docs/schemas/ontogony-runtime-port-lock-v1.schema.json`](../schemas/ontogony-runtime-port-lock-v1.schema.json)

## Related

- [`CROSS_SERVICE_CONTEXT_PROPAGATION_V1.md`](./CROSS_SERVICE_CONTEXT_PROPAGATION_V1.md)
- [`ONTOGONY_SIX_REPO_COMPATIBILITY_LOCK.md`](./ONTOGONY_SIX_REPO_COMPATIBILITY_LOCK.md)
- `metabole-dotnet/docs/system/ONTOGONY_FIVE_SERVICE_PORT_MATRIX.md`
