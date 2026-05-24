# 08 — Docs and profile guide

## Documentation goals

Docs must explain runtime config as a simple default layer, not as a new secret store or production environment manager.

The docs should answer:

- What is runtime config?
- What is it not?
- Which values come from runtime config?
- Which values remain operator-local overrides?
- What is the precedence order?
- How does Docker inject config?
- How do tests seed/override config?
- How do I clear local overrides?
- Why are credentials/secrets excluded?

## Files to create in ontogony-frontend

```text
docs/runtime-config/RUNTIME_CONFIG.md
docs/runtime-config/LOCAL_PROFILE.md
docs/runtime-config/DOCKER_LOCAL_PROFILE.md
docs/runtime-config/CUSTOM_STACK_PROFILE.md
```

## Files to update in ontogony-frontend

```text
docs/LOCAL_DEV.md
docs/SYSTEM_STATUS_AND_SETTINGS.md
docs/generated/FE_FRONTEND_ENV_CATALOG.md
scripts/frontend-env-catalog.json
.env.example
README.md
```

## Files to update in ontogony-platform

```text
docker/local-working-system/README.md
scripts/smoke/README.md or relevant smoke docs
docs/local-working-system/* if present
docs/generated/system compatibility artifacts if they mention frontend URL defaults
```

## Backend docs to update only if necessary

```text
conexus-dotnet/docs/DEVELOPMENT.md
kanon-dotnet/docs/README.md or local setup docs
allagma-dotnet/docs/system/README.md
```

Backend docs should not be rewritten. Add one small note if useful:

```text
The operator frontend reads browser-facing service URLs from `/operator-runtime-config.json`. Backend-to-backend URLs remain service-local configuration and may use Docker compose service names.
```

## Suggested RUNTIME_CONFIG.md outline

```md
# Operator runtime config

## Summary
Runtime config is a static JSON file served by the frontend host at `/operator-runtime-config.json`. It supplies browser-visible defaults for service URLs and profile flags.

## It is not
- a secret store
- provider API key configuration
- backend service configuration
- OIDC/IAM
- a replacement for operator settings

## Config file path
/operator-runtime-config.json

## Precedence
hardcoded fallback -> runtime config -> browser-local -> session -> test/URL -> page draft

## Values included
services, frontend flags, evidence flags, diagnostics flags, local-alpha policy, optional Kanon/Conexus defaults

## Values excluded
service tokens, admin API keys, project API keys, provider API keys, passwords

## Missing/invalid behavior
...

## Docker-local behavior
...

## Custom stack behavior
...
```

## Suggested LOCAL_PROFILE.md

Explain:

- Vite dev serves `public/operator-runtime-config.json`.
- Default values remain `localhost:5081/5082/5083`.
- Missing runtime config is allowed in dev but shown as fallback.
- `.env.example` service `VITE_*` values are fallback/migration only.

## Suggested DOCKER_LOCAL_PROFILE.md

Explain:

- `ontogony-platform` generates `docker/local-working-system/generated/operator-runtime-config.json`.
- Compose mounts it into nginx.
- Browser-facing URLs must be host URLs.
- Backend internal URLs still use compose DNS names.
- Config can change without rebuilding frontend image.
- Smoke script validates config.

## Suggested CUSTOM_STACK_PROFILE.md

Explain:

- Create or generate a custom `/operator-runtime-config.json`.
- Use browser-reachable URLs.
- Clear local overrides in Settings if the UI keeps old URLs.
- Do not include credentials.
- Use Settings to configure local credentials/session tokens intentionally.

## Settings help copy

Add compact help copy in Settings:

```text
Runtime config supplies defaults for this frontend instance. Local browser settings can override these defaults. Credentials are never loaded from runtime config.
```

For local override:

```text
This value is overridden in this browser. Clear the override to use the runtime default again.
```

For missing config:

```text
Runtime config was not found. The console is using safe local fallback defaults. Docker-local smoke should treat this as a configuration issue.
```

For invalid config:

```text
Runtime config is invalid. The console is using safe fallback defaults. Open Runtime config details for validation errors.
```

## Env catalog update

`frontend-env-catalog.json` should distinguish:

- `build-metadata` — still build-time and required.
- `runtime-default-fallback` — old service URL/ontology VITE vars retained temporarily.
- `deprecated-runtime-default` — once runtime config fully replaces them.

Do not remove catalog discipline. Update it to reflect the new runtime config source-of-truth.
