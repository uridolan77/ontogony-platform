# 02 — Runtime config contract

## Chosen path

```text
/operator-runtime-config.json
```

## Why this path

Current `ontogony-frontend/nginx/default.conf` serves static files from `/usr/share/nginx/html` and falls back to `index.html` for unknown paths. A root-level JSON path is the simplest path that:

- works in Vite dev via `public/operator-runtime-config.json`;
- works in nginx via a mounted/copied file;
- is obviously not a backend API route;
- can receive exact no-cache headers;
- avoids route inventory / generated client confusion.

## Contract versioning

The config includes both a schema identifier and version:

```json
{
  "configSchema": "ontogony.operator-runtime-config.v1",
  "version": 1
}
```

The loader must reject unknown future major schemas unless an explicit tolerant mode is added later.

## Full contract

```json
{
  "configSchema": "ontogony.operator-runtime-config.v1",
  "version": 1,
  "environmentName": "Local",
  "profileName": "docker-local",
  "generatedAt": "2026-05-24T00:00:00.000Z",
  "build": {
    "source": "ontogony-platform/docker/local-working-system",
    "gitSha": "local",
    "description": "Docker local working system runtime defaults"
  },
  "services": {
    "conexus": {
      "baseUrl": "http://localhost:5082"
    },
    "kanon": {
      "baseUrl": "http://localhost:5081"
    },
    "allagma": {
      "baseUrl": "http://localhost:5083"
    }
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

## TypeScript shape

Create `src/app/runtime-config/operatorRuntimeConfigTypes.ts`:

```ts
export type RuntimeConfigStatus =
  | "loaded"
  | "missing"
  | "invalid"
  | "fallback";

export type OperatorRuntimeConfig = {
  configSchema: "ontogony.operator-runtime-config.v1";
  version: 1;
  environmentName: string;
  profileName: string;
  generatedAt?: string;
  build?: {
    source?: string;
    gitSha?: string;
    description?: string;
  };
  services: {
    conexus: { baseUrl: string };
    kanon: { baseUrl: string };
    allagma: { baseUrl: string };
  };
  frontend?: {
    defaultRoute?: string;
    enableFixtureRoutes?: boolean;
    enableExpertModeDefault?: boolean;
    routeSearchEnabled?: boolean;
  };
  evidence?: {
    enableRawPreviewDefault?: boolean;
    exportRedactionMode?: "safe" | "strict" | "none";
  };
  localAlpha?: {
    allowBrowserCredentialStorage?: boolean;
    showLocalCredentialWarnings?: boolean;
  };
  diagnostics?: {
    enableDiagnosticExport?: boolean;
  };
  kanon?: {
    ontologyId?: string;
    ontologyVersionId?: string;
  };
  conexus?: {
    projectId?: string;
    modelAlias?: string;
    streamingEnabled?: boolean;
  };
};

export type RuntimeConfigSnapshot = {
  status: RuntimeConfigStatus;
  config: OperatorRuntimeConfig;
  sourceUrl: string;
  loadedAt: string;
  errors: string[];
  warnings: string[];
  usedFallback: boolean;
};
```

## Validation rules

Required:

- `configSchema` equals `ontogony.operator-runtime-config.v1`.
- `version` equals `1`.
- `environmentName` non-empty string.
- `profileName` non-empty string.
- `services.conexus.baseUrl`, `services.kanon.baseUrl`, `services.allagma.baseUrl` non-empty absolute `http`/`https` URLs.

Optional but validated when present:

- `frontend.defaultRoute` starts with `/`.
- `evidence.exportRedactionMode` is one of `safe`, `strict`, `none`.
- `generatedAt` parses as ISO timestamp or is omitted.
- no unknown top-level `secrets`, `credentials`, `apiKeys`, `tokens`, `providerKeys` field.

## Secret denial rule

Runtime config must reject suspicious secret-bearing keys by default. The validator should scan recursively for key names matching:

```text
secret, token, apiKey, api_key, password, credential, bearer, authorization, openai, anthropic, gemini
```

Exception: boolean/local policy names like `allowBrowserCredentialStorage` and warning copy names are allowed. This is a guardrail, not production DLP.

## Fallback defaults

Create `operatorRuntimeConfigDefaults.ts` with safe local fallback defaults matching current local dev behavior:

```ts
export const safeFallbackRuntimeConfig: OperatorRuntimeConfig = {
  configSchema: "ontogony.operator-runtime-config.v1",
  version: 1,
  environmentName: "Local",
  profileName: "vite-dev-fallback",
  services: {
    conexus: { baseUrl: "http://localhost:5082" },
    kanon: { baseUrl: "http://localhost:5081" },
    allagma: { baseUrl: "http://localhost:5083" },
  },
  frontend: {
    defaultRoute: "/",
    enableFixtureRoutes: false,
    enableExpertModeDefault: false,
    routeSearchEnabled: true,
  },
  evidence: {
    enableRawPreviewDefault: false,
    exportRedactionMode: "safe",
  },
  localAlpha: {
    allowBrowserCredentialStorage: true,
    showLocalCredentialWarnings: true,
  },
  diagnostics: {
    enableDiagnosticExport: true,
  },
  kanon: {
    ontologyId: "gaming-core",
    ontologyVersionId: "gaming-core@0.1.0",
  },
  conexus: {
    projectId: "dev-project",
    modelAlias: "risk-summary-v0",
    streamingEnabled: false,
  },
};
```

## Missing runtime config behavior

Vite dev mode:

- Missing file: non-fatal; use safe fallback and show Settings diagnostic `Runtime config: missing; using fallback defaults`.
- Invalid file: non-fatal; use safe fallback and show `invalid` warning with validation details.

Docker/static mode:

- Missing file: app still loads but Settings/System Truth must show a clear degraded config warning.
- Smoke scripts should fail Docker-local readiness if the file is missing.

## No backend API inventory inclusion

`/operator-runtime-config.json` is a static frontend asset. It must not be added to Conexus/Kanon/Allagma service route inventories. It may be added to frontend docs or smoke catalogs as a static runtime asset.
