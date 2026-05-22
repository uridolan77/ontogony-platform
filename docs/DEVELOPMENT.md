# Local development

## Prerequisites

- [.NET SDK 9](https://dotnet.microsoft.com/download) — repo pins `9.0.100` in [`global.json`](../global.json) with `rollForward: latestFeature`
- PowerShell 7+ recommended for validation scripts (`pwsh`)

---

## Build and test

```powershell
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln --no-restore
dotnet test Ontogony.Platform.sln --no-build
```

Regenerate solution if needed:

```powershell
pwsh ./scripts/bootstrap-solution.ps1
```

---

## Validation scripts (local)

| Script | Purpose |
| --- | --- |
| `scripts/validate-shipping-inventory.ps1` | Expects 27 `Ontogony.*` projects under `src/` |
| `scripts/validate-package-levels.ps1` | Package reference matrix |
| `scripts/validate-docs-links.ps1` | Relative markdown links under `docs/` |
| `scripts/validate-ai-runtime-docs.ps1` | Banned provider API strings in docs |
| `scripts/run-system-compatibility-gate.ps1` | Cross-repo compat (needs sibling repos) |

See [`TESTING.md`](./TESTING.md) for CI and conformance details.

---

## Six-repo workspace layout

Default operator layout (all repos as siblings under `C:\dev\`):

```text
C:\dev\
  ontogony-platform\
  allagma-dotnet\
  kanon-dotnet\
  conexus-dotnet\
  ontogony-frontend\
  ontogony-ui\
```

---

## Local service ports (script-based sanity)

| Service | Base URL | Health |
| --- | --- | --- |
| Kanon | `http://localhost:5081` | `GET /health`, `GET /ready` |
| Conexus | `http://localhost:5082` | `GET /health`, `GET /ready` |
| Allagma | `http://localhost:5083` | `GET /health`, `GET /ready` |

Alternate ports `5181`–`5183` exist in Allagma runtime profiles for conflicts. See `allagma-dotnet/docs/system/LOCAL_RUNTIME_PROFILES.md`.

### Session environment (PowerShell)

```powershell
$env:ONTOGONY_DEV_ROOT = "C:\dev"
$env:KANON_BASE_URL = "http://localhost:5081"
$env:CONEXUS_BASE_URL = "http://localhost:5082"
$env:ALLAGMA_BASE_URL = "http://localhost:5083"
$env:ASPNETCORE_ENVIRONMENT = "Development"
```

Allagma eval operator path (non-production only):

```powershell
$env:Allagma__Evaluation__ManualWriteEnabled = "true"
$env:Kanon__BaseUrl = "http://localhost:5081"
$env:Conexus__BaseUrl = "http://localhost:5082"
$env:Conexus__ProjectApiKey = "cx-dev-key-change-me"
```

Conexus Development uses the **fake** provider by default — no external model API calls.

---

## Docker local working system

Canonical compose stack and operator scripts:

- [`docker/local-working-system/README.md`](../docker/local-working-system/README.md)
- Planning reference: [`environments/docker-local-working-system/`](./environments/docker-local-working-system/)

Real-provider local validation policy: [`operators/REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md`](./operators/REAL_PROVIDER_LOCAL_VALIDATION_POLICY.md).

**Boundary:** Docker-local programs validate operator workflows; they are **not production readiness**.

---

## Examples

| Example | Purpose |
| --- | --- |
| `examples/MinimalApiWithOntogonyObservability/` | Tracing + outbound correlation |
| `examples/MinimalApiWithOntogonyHosting/` | Service defaults + health endpoints |
| `examples/ConexusDotNetSkeleton/` | Conexus package-slice compile smoke |
| `examples/AllagmaDotNetSkeleton/` | Allagma package-slice compile smoke |

---

## Versioning

Shipping line: **`0.3.0-alpha.1`**. Breaking changes require `CHANGELOG.md`, migrations under [`migrations/`](./migrations/), and consumer validation. See [`INTEGRATION.md`](./INTEGRATION.md) and [`CURRENT_STATE.md`](./CURRENT_STATE.md).
