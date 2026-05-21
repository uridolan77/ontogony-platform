# 05 — Acceptance matrix

## Release target

`SYSTEM-TIGHT-001` may be cut only when all P0 and P1 gates pass.

## P0 gates — required

| Gate | Owner | Command / Evidence | Pass condition |
|---|---|---|---|
| Runtime lock validates | Allagma | `scripts/validate-runtime-lock.ps1 -ReleaseMode -RequireEvidence` | PASS |
| Lock crossrefs validate | Allagma | `scripts/validate-release-lock-crossref.ps1` | PASS |
| Allagma default tests | Allagma | `dotnet test Allagma.sln -c Release --filter Category!=CrossRepo&Category!=PersistenceSmoke` | PASS |
| Allagma package mode | Allagma | `scripts/run-package-mode-build.ps1` | PASS |
| Cross-repo conformance | Allagma | `scripts/architecture-conformance/run-cross-repo-conformance.ps1` | PASS |
| System cohesion smoke | Allagma | `scripts/run-system-cohesion-smoke.ps1 -UseExistingServices -IncludeKanonAssistance -IncludeConexusFallback` | PASS |
| Restart survival | Allagma | Docker restart E2E canonical path | PASS |
| Kanon manifest conformance | Kanon + Allagma | `KanonCompatibilityManifestTests`, `KanonCompatibilityManifestConformanceTests` | PASS |
| Kanon v0 drift gates | Kanon | route inventory, OpenAPI, freeze, evidence spine tests | PASS |
| Conexus gateway tests | Conexus | default CI tests excluding opt-in categories | PASS |
| Conexus route-preview/quota tests | Conexus | route preview + quota endpoint filters | PASS |
| Evidence index updated | Platform + Allagma | system evidence index validation | PASS |

## P1 gates — required for operator tightness

| Gate | Owner | Pass condition |
|---|---|---|
| Evidence spine resolver complete | Platform/Frontend | runId, decisionId, modelCallId, routeDecisionId, traceId tested |
| Operator audit journey | Frontend/UI | Docker-local Playwright smoke covers cross-service audit |
| Streaming lifecycle visible | Allagma/Frontend | stream metadata appears in audit without payload persistence |
| Conexus route-preview wired | Frontend/Conexus | UI can preview route without provider call |
| Quota status wired | Frontend/Conexus | UI can display quota status before/after call |
| Failure taxonomy surfaced | Platform/Allagma/Frontend | representative downstream errors show normalized operator message |

## P2 gates — defer unless explicitly promoted

| Gate | Reason deferred |
|---|---|
| Enterprise IAM | production-readiness track |
| Real tool execution | safety/trust-model track |
| Managed production observability | production ops track |
| Durable external artifact store | retention/replay production track |
| `/ontology/v1` | Kanon v1 graduation track |
