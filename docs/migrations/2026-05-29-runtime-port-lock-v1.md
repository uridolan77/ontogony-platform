# Migration: runtime port lock v1 (CONEXUS-BACKEND-INTEGRATION-HARDENING-001)

## Summary

Promotes the local-dev / docker-local host-port assignment to a frozen platform
contract: [`docs/contracts/ONTOGONY_RUNTIME_PORT_LOCK_V1.md`](../contracts/ONTOGONY_RUNTIME_PORT_LOCK_V1.md).

The locked matrix is **Kanon 5081, Conexus 5082, Allagma 5083, Metabole 5084,
Aisthesis 5085, Frontend 5173**. This is mechanical local-dev topology only — no
routing, orchestration, or semantic meaning.

## Breaking change

The legacy combined-stack layout that bound **Aisthesis on 5084** and **Metabole
on 5085** is now **deprecated** and unsupported for certification. Stacks using it
must migrate to the locked matrix.

## Consumer impact

| Repo | Action | Status (2026-05-29) |
| --- | --- | --- |
| `ontogony-platform` | Publish `ONTOGONY_RUNTIME_PORT_LOCK_V1` contract + schema + machine matrix | **Done** |
| `metabole-dotnet` | `ONTOGONY_FIVE_SERVICE_PORT_MATRIX.md` already canonical and matches (5084) | **Aligned** |
| `aisthesis-dotnet` | Producer `BaseUrl` defaults / emission target on 5085 | **Aligned** |
| `allagma-dotnet` | `ontogony-runtime.lock.json` `ports` must match locked matrix | **Verify on next lock refresh** |
| `conexus-dotnet` | Operator runtime config / compose `CONEXUS_HOST_PORT=5082` | **Aligned** |
| docker-local-working-system docs | Drop deprecated Aisthesis=5084 / Metabole=5085 table rows | **Follow-up doc cleanup** |

## API / artifacts added

- `docs/contracts/ONTOGONY_RUNTIME_PORT_LOCK_V1.md`
- `docs/schemas/ontogony-runtime-port-lock-v1.schema.json`
- `docs/schemas/ontogony-runtime-port-lock-v1.json` (machine-readable matrix)

## Notes

This contract intentionally consumes (does not redefine) the
`ontogony.operator-runtime-config.v1` `services[].baseUrl` shape and the
`{SERVICE}_HOST_PORT` env-var convention.
