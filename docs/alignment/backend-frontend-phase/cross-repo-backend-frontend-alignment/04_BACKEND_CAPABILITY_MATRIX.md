# Backend Capability Matrix

| Capability | Frontend state | Backend target | Primary repo | Priority |
|---|---|---|---|---|
| System health/status | Live | Add source commit/contract metadata to `/health` | all backend repos | P1 |
| OpenAPI provenance | Frontend hashes snapshots | Backend CI publishes OpenAPI artifact + SHA | all backend repos | P1 |
| Conexus request lookup | Single lookup exists | Keep and document stable endpoint | `conexus-dotnet` | P0 |
| Conexus request search/list | Limitation/missing | Add paged admin search/list | `conexus-dotnet` | P0 |
| Conexus streaming | Frontend gated SSE support | Document SSE behavior/capability | `conexus-dotnet` | P1 |
| Kanon semantic plans | Live workbench | Confirm stable schemas and trace/decision metadata | `kanon-dotnet` | P1 |
| Kanon replay bundle lookup | Live via workbench | Stabilize typed schemas and lookup modes | `kanon-dotnet` | P1 |
| Kanon domain pack lifecycle | Frontend consumes lifecycle routes | Include decision IDs and lifecycle history schemas | `kanon-dotnet` | P1 |
| Allagma resume | Live | Keep OpenAPI-backed and idempotent | `allagma-dotnet` | P0 |
| Allagma retry/cancel | Limitation | Implement or explicitly mark unsupported | `allagma-dotnet` | P1 |
| Allagma replay trigger | Limitation | Implement or explicitly mark unsupported | `allagma-dotnet` | P1 |
| Allagma typed replay evidence | Flexible frontend adapters | Add typed schemas | `allagma-dotnet` | P1 |
| Cross-service trace IDs | Frontend correlates IDs | Canonical envelope across services | platform + all | P0 |
| System compatibility CI | Partial repo-level CI | Cross-repo local stack report | platform | P0 |
