# 02 — Prioritized backlog

## P0 — Governance and baseline promotion

### 1. `SYSTEM-SPRINT4-STATUS-RECON-001`

Reconcile platform sprint plan and acceptance matrix with current service evidence. Mark Sprint 4 source/evidence closeout as closed, while keeping runtime lock promotion pending.

### 2. `SYSTEM-ALPHA-004-PREP`

Prepare validation artifacts for a new runtime baseline. This is prep only; do not update lock yet.

### 3. `SYSTEM-ALPHA-004-CUT`

After validation passes, update `ontogony-runtime.lock.json`, evidence artifact paths, baseline docs, and closeout.

## P1 — Baseline governance and drift prevention

### 4. `SYSTEM-BASELINE-001`

Create `ontogony-platform/docs/system/CURRENT_SYSTEM_BASELINE.md`.

### 5. `REPO-DOCS-ARCHIVE-001`

Archive/quarantine stale `_incoming`, old planning packages, and historical prompt packages so search results do not confuse current implementation.

### 6. `ROUTE-INVENTORY-001`

Generate and test route/security inventories for:

- Conexus `/admin/**`
- Kanon `/ontology/v0/**`
- Allagma `/allagma/v0/**`

### 7. `FE-LIVE-SMOKE-002`

Add a richer Docker-live browser smoke that checks one safe operator path per service and one evidence/correlation path.

## P1/P2 — Operational maturity

### 8. `SYSTEM-OBS-METERS-001`

Add `/ready` SLI, Kanon plan-compile meters, Conexus cost OTEL counters, and Allagma streaming/evidence meters.

### 9. `CONEXUS-RETENTION-002`

Add durable Postgres-backed retention maintenance-run history. This is not a Sprint 4 blocker; it is the production-hardening successor to process-local `lastRun`.

## P2 — UI/package maturity

### 10. `UI-BUNDLE-001`

Generate subpath dependency and bundle impact report for `@ontogony/ui`.

### 11. `UI-VISUAL-001`

Create visual/a11y coverage matrix for shared UI primitives.

### 12. `PROD-AUTH-ROADMAP-001`

Create a cross-repo production auth roadmap covering frontend local gate vs backend auth, Conexus admin/project keys, Kanon service tokens, Allagma bearer auth, JWT/mTLS/RBAC path.
