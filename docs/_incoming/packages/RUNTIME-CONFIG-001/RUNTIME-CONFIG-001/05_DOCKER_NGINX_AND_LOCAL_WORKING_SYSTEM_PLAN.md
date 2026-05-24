# 05 — Docker, nginx, and local-working-system plan

## Current Docker issue

Current `ontogony-frontend/Dockerfile` accepts service URL build args and converts them into Vite env vars before `npm run build`. This means changing Conexus/Kanon/Allagma browser URLs requires rebuilding the frontend image.

Current `ontogony-platform/docker/local-working-system/docker-compose.yml` passes:

```yaml
VITE_KANON_BASE_URL: ${FRONTEND_VITE_KANON_BASE_URL:-http://localhost:5081}
VITE_CONEXUS_BASE_URL: ${FRONTEND_VITE_CONEXUS_BASE_URL:-http://localhost:5082}
VITE_ALLAGMA_BASE_URL: ${FRONTEND_VITE_ALLAGMA_BASE_URL:-http://localhost:5083}
```

This should become runtime config generation/mounting, not build-time image content.

## Target Docker model

### Frontend image

The frontend image should contain static app assets plus a safe placeholder runtime config. The runtime config may be overridden by a mounted file.

Add to `ontogony-frontend/public/operator-runtime-config.json`:

```json
{
  "configSchema": "ontogony.operator-runtime-config.v1",
  "version": 1,
  "environmentName": "Local",
  "profileName": "vite-dev",
  "services": {
    "conexus": { "baseUrl": "http://localhost:5082" },
    "kanon": { "baseUrl": "http://localhost:5081" },
    "allagma": { "baseUrl": "http://localhost:5083" }
  },
  "frontend": {
    "defaultRoute": "/",
    "enableFixtureRoutes": false,
    "enableExpertModeDefault": false,
    "routeSearchEnabled": true
  },
  "evidence": {
    "enableRawPreviewDefault": false,
    "exportRedactionMode": "safe"
  },
  "localAlpha": {
    "allowBrowserCredentialStorage": true,
    "showLocalCredentialWarnings": true
  },
  "diagnostics": {
    "enableDiagnosticExport": true
  },
  "kanon": {
    "ontologyId": "gaming-core",
    "ontologyVersionId": "gaming-core@0.1.0"
  },
  "conexus": {
    "projectId": "dev-project",
    "modelAlias": "risk-summary-v0",
    "streamingEnabled": false
  }
}
```

Demote/remove frontend Docker build args for service URLs. Keep build metadata args.

### nginx no-cache

Update `ontogony-frontend/nginx/default.conf`:

```nginx
server {
    listen 8080;
    server_name _;

    root /usr/share/nginx/html;
    index index.html;

    location = /operator-runtime-config.json {
        add_header Cache-Control "no-store, no-cache, must-revalidate, proxy-revalidate" always;
        add_header Pragma "no-cache" always;
        add_header Expires "0" always;
        try_files /operator-runtime-config.json =404;
    }

    location / {
        try_files $uri $uri/ /index.html;
    }
}
```

## Platform config generation

Create in `ontogony-platform`:

```text
docker/local-working-system/config/operator-runtime-config.local.template.json
docker/local-working-system/generated/operator-runtime-config.json
scripts/local-working-system/write-operator-runtime-config.ps1
scripts/local-working-system/write-operator-runtime-config.sh
```

PowerShell script inputs:

```powershell
param(
  [string]$OutputPath = ".\docker\local-working-system\generated\operator-runtime-config.json",
  [string]$EnvironmentName = $env:FRONTEND_RUNTIME_ENVIRONMENT_NAME ?? "Local",
  [string]$ProfileName = $env:FRONTEND_RUNTIME_PROFILE_NAME ?? "docker-local",
  [string]$ConexusBaseUrl = $env:FRONTEND_RUNTIME_CONEXUS_BASE_URL ?? "http://localhost:5082",
  [string]$KanonBaseUrl = $env:FRONTEND_RUNTIME_KANON_BASE_URL ?? "http://localhost:5081",
  [string]$AllagmaBaseUrl = $env:FRONTEND_RUNTIME_ALLAGMA_BASE_URL ?? "http://localhost:5083",
  [string]$KanonOntologyId = $env:FRONTEND_RUNTIME_KANON_ONTOLOGY_ID ?? "gaming-core",
  [string]$KanonOntologyVersionId = $env:FRONTEND_RUNTIME_KANON_ONTOLOGY_VERSION_ID ?? "gaming-core@0.1.0",
  [string]$ConexusProjectId = $env:FRONTEND_RUNTIME_CONEXUS_PROJECT_ID ?? "dev-project",
  [string]$ConexusModelAlias = $env:FRONTEND_RUNTIME_CONEXUS_MODEL_ALIAS ?? "risk-summary-v0"
)
```

Do not read secrets. Do not include service tokens/admin/project API keys. `ConexusProjectId` is not a credential; `ConexusProjectApiKey` is a credential and must stay out.

## Compose changes

In `docker/local-working-system/docker-compose.yml`, add runtime config generation expectation and mount:

```yaml
ontogony-frontend:
  build:
    context: ../../../ontogony-frontend
    additional_contexts:
      ontogony_ui: ../../../ontogony-ui
      ontogony_agent_interaction: ../../../ontogony-platform/packages/ontogony-agent-interaction
    args:
      EXTRA_CA_CERT_BASE64: ${DOCKER_EXTRA_CA_CERT_BASE64:-}
      VITE_APP_VERSION: ${FRONTEND_VITE_APP_VERSION:-}
      VITE_GIT_SHA: ${FRONTEND_VITE_GIT_SHA:-local}
      VITE_BUILD_TIME: ${FRONTEND_VITE_BUILD_TIME:-}
  volumes:
    - ./generated/operator-runtime-config.json:/usr/share/nginx/html/operator-runtime-config.json:ro
  ports:
    - "${FRONTEND_HOST_PORT:-5175}:8080"
```

Keep old `FRONTEND_VITE_*` env vars during transition only if scripts still depend on them. Prefer new names:

```text
FRONTEND_RUNTIME_CONEXUS_BASE_URL
FRONTEND_RUNTIME_KANON_BASE_URL
FRONTEND_RUNTIME_ALLAGMA_BASE_URL
FRONTEND_RUNTIME_KANON_ONTOLOGY_ID
FRONTEND_RUNTIME_KANON_ONTOLOGY_VERSION_ID
FRONTEND_RUNTIME_CONEXUS_PROJECT_ID
FRONTEND_RUNTIME_CONEXUS_MODEL_ALIAS
```

## Browser vs container contexts

Runtime config is consumed by the **browser**, not by backend containers. Therefore Docker-local runtime config must use host-reachable URLs:

```text
http://localhost:5081
http://localhost:5082
http://localhost:5083
```

Do not use:

```text
http://kanon-api:8080
http://conexus-api:8080
http://allagma-api:8080
```

Those internal URLs are only for backend-to-backend calls inside compose.

## Smoke verification

Add script:

```powershell
pwsh .\scripts\smokessert-operator-runtime-config.ps1 -BaseUrl http://localhost:5175
```

Checks:

- `GET /operator-runtime-config.json` returns `200`.
- Content type is JSON or parseable JSON.
- Schema is `ontogony.operator-runtime-config.v1`.
- URLs match expected profile.
- No forbidden secret-like keys are present.
- Response has no-cache headers.

## Updating config without rebuild

Workflow:

```powershell
# edit env or pass params
pwsh .\scripts\local-working-system\write-operator-runtime-config.ps1 `
  -ConexusBaseUrl http://localhost:6092 `
  -KanonBaseUrl http://localhost:6091 `
  -AllagmaBaseUrl http://localhost:6093

# no frontend image rebuild required
# refresh browser; if compose volume is mounted, nginx serves updated JSON immediately
```

If Docker Desktop does not update bind-mounted files reliably on Windows, document a container restart as acceptable. The requirement is no **image rebuild**, not necessarily no container restart.

## Backward compatibility

- Vite dev: works with `public/operator-runtime-config.json`.
- Missing config: app loads with fallback defaults and visible warning.
- Existing compose defaults: still serve `localhost:5081/5082/5083`.
- Existing CORS assumptions: unchanged.
- Existing backend tokens/secrets: unchanged and remain out of runtime config.
