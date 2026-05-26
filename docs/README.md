# Ontogony platform — documentation

**Repo:** `uridolan77/ontogony-platform`  
**Role:** Shared infrastructure (`Ontogony.*` packages) and cross-repo **mechanical** contracts.

---

## What this repo owns

Ontogony.Platform is the **neutral mechanics layer** for the Ontogony stack:

- tracing and correlation
- error envelopes and HTTP client utilities
- hashing, idempotency, and persistence/outbox primitives
- observability and AI telemetry **contracts** (not provider SDKs)
- system compatibility gates, protocol registry, and shared test harnesses
- packaging, versioning, and consumer integration patterns

## What this repo does **not** own

Do not treat platform docs as product documentation for:

| Concern | Owner |
| --- | --- |
| Semantic authority, ontology, decisions | **Kanon** (`kanon-dotnet`) |
| Model gateway, routing, provider policy | **Conexus** (`conexus-dotnet`) |
| Governed runs, tool gates, agent orchestration | **Allagma** (`allagma-dotnet`) |
| Operator console UX and page flows | **ontogony-frontend** / **ontogony-ui** |

Those systems may appear here only as **consumers** or examples of platform mechanics.

---

## How docs are organized

| Section | Location |
| --- | --- |
| **Navigation index** | [`INDEX.md`](./INDEX.md) |
| **Current truth** | [`CURRENT_STATE.md`](./CURRENT_STATE.md) |
| **Architecture** | [`ARCHITECTURE.md`](./ARCHITECTURE.md), [`architecture/`](./architecture/) |
| **Contracts** | [`CONTRACTS.md`](./CONTRACTS.md), [`contracts/`](./contracts/) |
| **Errors / HTTP / observability** | [`operators/`](./operators/), [`packages/Ontogony.Errors.md`](./packages/Ontogony.Errors.md), [`packages/Ontogony.Http.md`](./packages/Ontogony.Http.md) |
| **Idempotency / persistence** | [`packages/Ontogony.Persistence.md`](./packages/Ontogony.Persistence.md), [`adoption/conformance-kits.md`](./adoption/conformance-kits.md) |
| **Hosting / local stack** | [`environments/`](./environments/), [`../docker/local-working-system/`](../docker/local-working-system/) |
| **Testing / evidence** | [`TESTING.md`](./TESTING.md), [`evidence/`](./evidence/) |
| **Runbooks / operator mechanics** | [`operators/`](./operators/) |
| **Decisions** | [`adr/`](./adr/), [`migrations/`](./migrations/) |
| **Release / governance** | [`governance/`](./governance/) |
| **Cursor intake (temporary)** | [`_incoming/`](./_incoming/) |

---

## Start here

| Document | Purpose |
| --- | --- |
| [**INDEX.md**](./INDEX.md) | Full table of canonical docs |
| [**Learning path (all repos)**](./learn/INDEX.md) | Cross-repo guides for new developers |
| [`CURRENT_STATE.md`](./CURRENT_STATE.md) | What is implemented vs not |
| [`ARCHITECTURE.md`](./ARCHITECTURE.md) | Package layers, forbidden edges |
| [`CONTRACTS.md`](./CONTRACTS.md) | Trace, errors, headers, compatibility |
| [`DEVELOPMENT.md`](./DEVELOPMENT.md) | Build, ports, Docker |
| [`TESTING.md`](./TESTING.md) | CI and conformance |
| [`INTEGRATION.md`](./INTEGRATION.md) | Consumer blueprints |
| [`KNOWN_LIMITATIONS.md`](./KNOWN_LIMITATIONS.md) | Non-goals |

---

## Intake policy (`docs/_incoming`)

Cursor packages and handoff specs live under [`_incoming/`](./_incoming/) until promoted or archived:

- **Active:** [`_incoming/_active/`](./_incoming/_active/) + [`MANIFEST.md`](./_incoming/_active/MANIFEST.md)
- **Consumed:** [`_incoming/_consumed/<YYYY-MM>/`](./_incoming/_consumed/) + [`MANIFEST.md`](./_incoming/_consumed/MANIFEST.md)

Only `_active`, `_consumed`, and `_incoming/README.md` may sit directly under `_incoming/`. No zip files.

**When a package is done:** move it to `_consumed/<YYYY-MM>/` — required procedure in [`_incoming/README.md`](./_incoming/README.md#when-to-move-a-package-to-_consumed-required).

---

## Sister repos

| Repo | Docs |
| --- | --- |
| allagma-dotnet | Governed execution, system E2E |
| kanon-dotnet | Semantic authority |
| conexus-dotnet | Model gateway |
| ontogony-frontend | Operator UI |
| ontogony-ui | Shared UI components |
