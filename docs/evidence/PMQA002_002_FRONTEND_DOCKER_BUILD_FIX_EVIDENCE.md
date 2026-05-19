# PMQA002-002 — Frontend Docker build blocker fix evidence

- Date: `2026-05-19`
- Scope: Docker-local frontend build/toolchain reliability for manual QA re-run
- Boundary: build correctness only; no backend contract/product behavior changes; no TLS bypass; no secrets

## 1) Reproduction

Command:

```powershell
cd C:\dev\ontogony-platform
.\docker\local-working-system\scripts\start-local-working-system.ps1 -Build
```

Failure observed in `ontogony-frontend`:

- Stage: `build-ui`
- Command: `RUN npm run build`
- Error:
  - `> tsc -b && vite build --config vite.lib.config.ts`
  - `sh: 1: tsc: not found`

## 2) Root cause analysis

The failure was not backend-related and not a route contract defect. It was frontend build toolchain setup under Docker-local TLS interception.

Key findings:

- `typescript` exists in lockfiles as `devDependency`.
- Build-stage install path for npm was not deterministic in this environment:
  - npm emitted internal failure output (`Exit handler never called!`) during `npm ci`.
  - resulting build stage did not have `tsc` available at runtime of `npm run build`.
- Local TLS interception impacts npm registry trust in Node-based build stages unless trust is explicitly aligned.

## 3) Narrow fix

### `ontogony-frontend/Dockerfile`

- Added `ARG EXTRA_CA_CERT_BASE64` to `build-ui` and `build-frontend`.
- Installed `ca-certificates` in both build stages.
- When CA arg is present:
  - decode/install cert,
  - run `update-ca-certificates`,
  - configure npm `cafile` to injected CA.
- Set `NODE_OPTIONS=--use-openssl-ca` in build stages.
- Switched build-stage installs to `npm ci --include=dev` (build-time only).
- Kept runtime stage as slim nginx image with only built assets.

### `docker/local-working-system/docker-compose.yml`

- Passed `EXTRA_CA_CERT_BASE64: ${DOCKER_EXTRA_CA_CERT_BASE64:-}` into `ontogony-frontend` build args.

### `docker/local-working-system/scripts/start-local-working-system.ps1`

- Extended auto-CA detection to include npm registry TLS probe (in addition to NuGet probe).
- If either probe fails and no override is set, the script injects trusted local root CA from Windows trust store into compose build args.
- Preserves TLS verification (no insecure flags).

## 4) Verification

Full-stack rebuild:

```powershell
.\docker\local-working-system\scripts\start-local-working-system.ps1 -Build
```

Observed:

- `Image local-working-system-ontogony-frontend Built`
- `All requested services are healthy.`
- `Docker local working system is up.`

Frontend route probes after rebuild:

- `GET /` on `http://localhost:5175` → `200`
- `GET /allagma/evaluations` → `200`
- `GET /allagma/evaluations/baseline-comparisons` → `200`
- `GET /allagma/evaluations/datasets` → `200`
- `GET /allagma/replay` → `200`

## 5) Acceptance status

- Full-stack Docker-local build succeeds, including frontend: **PASS**
- Frontend runtime container serves required SPA routes on `5175`: **PASS**
- `tsc` issue fixed without silent bypass: **PASS**
- Dev dependencies remain build-stage only; runtime image remains nginx-only: **PASS**
- No secrets or TLS verification bypass introduced: **PASS**
