# ONTOGONY-SAMPLING-POLICY-SPINE-001 — Cross-repo evidence

Package: `ONTOGONY-SAMPLING-POLICY-SPINE-001`  
Status: **implemented (v0 spine)** — May 2026

## Implemented

### Conexus (`conexus-dotnet`)

- Domain contracts under `Conexus.Domain.Sampling`
- JSON profile registry: `docs/contracts/sampling-profiles.v0.json`
- Resolver, validator, provider translator, chat applicator
- Routes:
  - `GET /llm/v0/sampling-profiles`
  - `GET /llm/v0/sampling-profiles/{profileId}`
  - `POST /llm/v0/sampling-policy/resolve`
  - `POST /llm/v0/sampling-policy/validate`
- Chat completion path applies effective sampling and records telemetry metadata keys (`sampling_*`)
- Optional Kanon gateway (`Conexus:Kanon:BaseUrl`, `Conexus:SamplingPolicy:KanonAuthorityMode`)

### Kanon (`kanon-dotnet`)

- `POST /ontology/v0/sampling-policy/evaluate`
- Advisory by default; strict mode host-gated via `Kanon:SamplingPolicy:StrictModeEnabled`

### Allagma (`allagma-dotnet`)

- Run timeline events: `Allagma.SamplingPolicyResolved`, warning/violation/override events
- Conexus request metadata propagation from model purpose
- Side-effect gate blocking direct execution after `CreativeIdeation` / `DiversityProbe`

### Frontend (`ontogony-frontend`)

- `src/sampling/*` types, parser, panel, badge
- Model-call detail panel shows sampling policy trace when telemetry parameters are present

## Deferred

- Global strict mode default-on for all contract-bound call sites
- OpenAPI snapshot regeneration for `/llm/v0` in frontend `openapi/conexus.v0.json`
- Human-gate workflow for profile override approval (event contracts present; UI flow deferred)
- `AdHocLegacy` canonical profile (legacy raw params emit warning only)

## Verification

```bash
# Conexus
dotnet test c:/Dev/conexus-dotnet --filter SamplingPolicy

# Kanon
dotnet test c:/Dev/kanon-dotnet --filter SamplingPolicy

# Allagma
dotnet test c:/Dev/allagma-dotnet --filter SamplingPolicy

# Frontend
cd c:/Dev/ontogony-frontend && npm test -- sampling
```

## Owner repo evidence links

- Conexus implementation: `c:/Dev/conexus-dotnet/docs/contracts/sampling-profiles.v0.json`
- Kanon evaluate route: `c:/Dev/kanon-dotnet/src/Kanon.Api/Endpoints/SamplingPolicyEndpoints.cs`
- Allagma events: `c:/Dev/allagma-dotnet/src/Allagma.Application/Ports.cs`
- Frontend panel: `c:/Dev/ontogony-frontend/src/sampling/SamplingPolicyPanel.tsx`
