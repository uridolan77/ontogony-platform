Yes. Based on current repo state, **Runtime Config is no longer a greenfield package**. The foundation already exists. The next development should be a **hardening + integration + Docker/profile completion package**, not “add runtime config from scratch.”

Package name:

```text id="q9d8l4"
RUNTIME-CONFIG-DEV-001
Runtime-configurable frontend and clean environment profiles
```

## Current baseline

The frontend already has a runtime config contract. `OperatorRuntimeConfig` includes schema/version, environment/profile names, service base URLs for Conexus/Kanon/Allagma, frontend flags, evidence defaults, local-alpha flags, diagnostics flags, Kanon ontology defaults, and Conexus project/model defaults. It loads from `/operator-runtime-config.json`. 

The app bootstraps by loading runtime config before React render, then passes the snapshot into both `RuntimeConfigProvider` and `OperatorSettingsProvider`. 

The loader already fetches with `cache: "no-store"` and `credentials: "same-origin"`, handles missing/invalid JSON, and falls back to safe defaults. 

Validation already rejects secret-like keys, requires schema/version, requires absolute HTTP(S) service URLs, and validates selected option fields. 

Runtime defaults already flow into operator settings: service URLs, Conexus project/model alias, Kanon ontology defaults, and Allagma actor defaults are built from runtime config where available. 

There is also provenance merging for runtime defaults versus local/legacy overrides, with reset-to-runtime-default behavior for service URLs and defaultable fields. 

So the right question is not “how do we add runtime config?” It is:

```text id="82k51w"
How do we make runtime config complete, visible, reproducible, Docker-friendly, testable, and non-drifting?
```

---

# RUNTIME-CONFIG-DEV-001 plan

## Goal

Make the operator frontend configurable at runtime across:

```text id="wywtpw"
Vite dev
Docker local-working-system
host browser against Docker APIs
remote/custom service profile
Playwright docker-live tests
operator diagnostics
system truth / readiness checks
```

without rebuilding the static frontend bundle.

## Non-goals

```text id="5n34it"
- No production secrets management.
- No backend auth redesign.
- No remote control plane.
- No multi-tenant config service.
- No storing credentials in runtime config.
```

Runtime config must remain **secret-free**.

---

# Phase 1 — Runtime config contract finalization

## Work

Create a canonical contract doc:

```text id="9e7ffz"
docs/contracts/OPERATOR_RUNTIME_CONFIG_V1.md
```

It should define:

```text id="p0ibzt"
configSchema
version
environmentName
profileName
generatedAt
build metadata
services.conexus.baseUrl
services.kanon.baseUrl
services.allagma.baseUrl
frontend flags
evidence flags
localAlpha flags
diagnostics flags
kanon defaults
conexus defaults
forbidden secret-like keys
fallback behavior
```

Also add examples:

```text id="tz17zz"
docs/examples/operator-runtime-config.local-dev.json
docs/examples/operator-runtime-config.docker-local.json
docs/examples/operator-runtime-config.host-to-docker.json
docs/examples/operator-runtime-config.remote-alpha.json
```

## Acceptance

```text id="1s66rs"
- Contract doc matches operatorRuntimeConfigTypes.ts.
- Example configs pass validation.
- Docs explicitly say credentials/tokens/API keys are forbidden.
```

---

# Phase 2 — Profile generator and profile catalog

## Work

Add a generator script:

```text id="z8cz2u"
scripts/runtime-config/generate-operator-runtime-config.mjs
```

Inputs:

```text id="qvcapq"
--profile vite-dev
--profile docker-local
--profile host-to-docker
--profile remote-alpha
--out public/operator-runtime-config.json
--environment-name Local Docker
--conexus-url http://localhost:5082
--kanon-url http://localhost:5081
--allagma-url http://localhost:5083
--ontology gaming-core@0.1.0
--model-alias risk-summary-v0
```

Add a profile catalog:

```text id="68z6pb"
scripts/runtime-config/operator-runtime-profiles.json
```

Suggested profiles:

```text id="bkhign"
vite-dev
docker-local-nginx
host-browser-to-docker
docker-browser-internal
remote-alpha
ci-mocked
playwright-docker-live
```

Add package scripts:

```json id="kfdmgh"
{
  "runtime-config:generate": "node scripts/runtime-config/generate-operator-runtime-config.mjs",
  "runtime-config:check": "node scripts/runtime-config/check-operator-runtime-config.mjs",
  "runtime-config:profiles": "node scripts/runtime-config/list-runtime-profiles.mjs"
}
```

## Acceptance

```text id="bmg8pp"
- Config files can be generated deterministically.
- Generated config validates.
- Generated config contains no secret-like keys.
- Config generation does not require rebuild of frontend bundle.
```

---

# Phase 3 — Docker static bundle integration

## Problem

The runtime config endpoint exists conceptually, but Docker/static serving must make it reliable.

## Work

In `ontogony-platform/docker/local-working-system`:

```text id="0fgv1u"
- Mount or copy operator-runtime-config.json into the nginx public root.
- Make compose profile choose correct service URLs for browser access.
- Ensure host browser receives localhost URLs, not container-internal DNS names.
- Add a dev override file for alternative host ports.
```

Add script:

```text id="pw3n2q"
docker/local-working-system/scripts/write-operator-runtime-config.ps1
```

or platform-level:

```text id="w4mqi0"
scripts/runtime-config/write-local-working-system-config.ps1
```

This should write the frontend-served:

```text id="gg7ai9"
operator-runtime-config.json
```

with correct host-visible URLs.

## Acceptance

```text id="7py0be"
- Rebuilding frontend image is not required to change service URLs.
- Docker frontend serves /operator-runtime-config.json.
- Host browser can use the config against APIs on 5081–5083 or whatever ports are selected.
- Docker-live Playwright proves runtime config is loaded, not fallback.
```

---

# Phase 4 — Settings provenance UX completion

## Current good state

The system already tracks runtime-default versus override provenance for important defaultable fields. 

## Work

On `/settings`, add or tighten:

```text id="5cppc9"
Runtime profile card:
  status: loaded / missing / invalid / fallback
  environmentName
  profileName
  sourceUrl
  loadedAt
  generatedAt
  usedFallback
  warnings/errors

Service connection rows:
  effective value
  runtime default value
  source badge:
    runtime default
    fallback default
    local override
    legacy local
    session override
    test override
  reset to runtime default action
```

Avoid adding clutter: use the canonical disclosure pattern.

Default-visible:

```text id="0o0kwz"
Profile name
Loaded/fallback status
3 service URLs
override count
```

Disclosure:

```text id="csl0ry"
full provenance table
warnings/errors
build metadata
raw config preview, redacted
```

## Acceptance

```text id="gqhhyt"
- Operator can tell whether config came from runtime file or fallback.
- Operator can reset service URLs to runtime defaults.
- Legacy local overrides are visibly classified.
- No duplicate warning cards.
```

---

# Phase 5 — Runtime config consumers audit

## Problem

The config contract contains flags, but not all of them may be consumed consistently.

Audit these fields:

```text id="cm4w35"
frontend.defaultRoute
frontend.enableFixtureRoutes
frontend.enableExpertModeDefault
frontend.routeSearchEnabled
evidence.enableRawPreviewDefault
evidence.exportRedactionMode
localAlpha.allowBrowserCredentialStorage
localAlpha.showLocalCredentialWarnings
diagnostics.enableDiagnosticExport
conexus.streamingEnabled
```

## Work

Create:

```text id="hsjvp9"
docs/generated/runtime-config-consumer-audit.json
docs/generated/RUNTIME_CONFIG_CONSUMER_AUDIT.md
```

Each field gets:

```text id="xmznzn"
field
declared in contract
default value
consumed by code
consumer files
test coverage
status: used / declared_only / intentionally_reserved
```

Then either wire or explicitly mark as reserved.

High-priority consumers:

```text id="7cw5fv"
diagnostics.enableDiagnosticExport
localAlpha.allowBrowserCredentialStorage
localAlpha.showLocalCredentialWarnings
conexus.streamingEnabled
frontend.enableFixtureRoutes
```

## Acceptance

```text id="eg2khe"
- No runtime config field is silently decorative.
- Unused fields are documented as intentionally reserved.
- check script fails if a new field is added without consumer classification.
```

---

# Phase 6 — System Truth / Home / Diagnostics integration

## Work

System Truth and Home should display runtime config posture as part of “why does this environment look like this?”

Add shared model:

```text id="c0a60a"
runtimeConfigTruthModel.ts
```

Status dimensions:

```text id="y18t3l"
config loaded
fallback used
config valid
service URL completeness
local override count
profile freshness
forbidden key validation
```

Add small runtime profile signal to:

```text id="y9w0wi"
/                Home
/settings        Settings
/system/topology Topology
/system/release-readiness
diagnostic export bundle
```

Do not add a huge new panel. Use one concise signal group or disclosure.

## Acceptance

```text id="xi5vyd"
- Diagnostics export includes runtime config snapshot metadata but no secrets.
- Home/Settings can show runtime profile loaded vs fallback.
- System Truth smoke can report runtime config status.
```

---

# Phase 7 — Runtime config gates

## Add checks

```text id="fycyts"
npm run runtime-config:check
```

Should validate:

```text id="bnh38g"
- public/operator-runtime-config.json exists for Docker profile, if required.
- JSON validates.
- no forbidden secret-like keys.
- all service URLs are absolute.
- generated examples validate.
- runtime config consumer audit current.
- frontend env catalog does not conflict with runtime config policy.
```

Add to:

```text id="z4m7je"
npm run check
console-ui:check maybe as advisory
CI local/manual path
Docker-live smoke
```

## Acceptance

```text id="188z2t"
- Invalid runtime config fails check.
- Missing runtime config is allowed in Vite dev only if fallback behavior is expected.
- Docker-local requires runtime config.
```

---

# Phase 8 — Playwright and smoke proof

## Tests

Add/extend:

```text id="rrkadt"
e2e/runtime-config-docker-live.spec.ts
```

Prove:

```text id="id8f9i"
- /operator-runtime-config.json is served.
- Settings shows loaded runtime profile, not fallback.
- Service URLs match generated Docker profile.
- System Truth uses those URLs.
- Changing config file and refreshing changes effective defaults without frontend rebuild.
- Local override remains override after runtime profile changes.
- Reset-to-runtime-default works.
```

Add unit tests:

```text id="cfp51h"
loadOperatorRuntimeConfig.test.ts
operatorRuntimeConfigValidation.test.ts
operatorSettingsRuntimeDefaults.test.ts
operatorSettingsProvenanceMerge.test.ts
runtimeConfigConsumerAudit.test.ts
```

## Acceptance

```text id="ialm2p"
- npm run typecheck passes.
- npm run runtime-config:check passes.
- npm run console-ui:check passes.
- Docker-live runtime config E2E passes.
```

---

# Phase 9 — Documentation

Add:

```text id="h4odef"
docs/operators/RUNTIME_CONFIG.md
docs/development/FRONTEND_ENVIRONMENT_PROFILES.md
docs/development/DOCKER_RUNTIME_CONFIG.md
```

The docs should explain:

```text id="ig5r43"
- Runtime config vs Vite env
- Runtime config vs local browser overrides
- How to generate a profile
- How Docker serves the config
- How to reset overrides
- Why secrets are forbidden
- How Playwright seeds test settings
```

Update the learning guide later to link here.

---

# Final acceptance definition

`RUNTIME-CONFIG-DEV-001` is closed only when:

```text id="pdncrc"
1. /operator-runtime-config.json is the canonical runtime profile source.
2. Docker frontend can change backend URLs without rebuild.
3. Runtime config validates and forbids secrets.
4. Settings clearly shows loaded profile, fallback, and local overrides.
5. System Truth / diagnostics include runtime profile posture.
6. Every runtime config field is consumed or intentionally reserved.
7. Docker-live E2E proves runtime config is used.
8. npm run check includes runtime-config validation.
9. Docs explain how to add/edit a profile.
```

---

# Suggested implementation order

```text id="cv7lgl"
RUNTIME-CONFIG-DEV-001A
  Contract doc + profile generator + examples.

RUNTIME-CONFIG-DEV-001B
  Docker/local-working-system serving + profile write script.

RUNTIME-CONFIG-DEV-001C
  Settings provenance UX + reset-to-runtime-default polish.

RUNTIME-CONFIG-DEV-001D
  Consumer audit + runtime-config:check gate.

RUNTIME-CONFIG-DEV-001E
  System Truth / diagnostics / Docker-live proof.
```

## Best first slice

Start with:

```text id="wx7x46"
RUNTIME-CONFIG-DEV-001A
```

because the type contract already exists. The first slice should formalize it into docs, examples, generation, and validation. After that, Docker mounting and UI polish become much easier.
