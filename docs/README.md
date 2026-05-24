# Ontogony platform — documentation

**Repo:** `uridolan77/ontogony-platform`  
**Role:** Shared infrastructure (`Ontogony.*` packages) and cross-repo **mechanical** contracts.

**Boundary:** Platform docs describe mechanics. Product semantics live in Kanon, Allagma, and Conexus. Closed operator programs are **not production readiness**.

---

## Start here

| Document | Purpose |
| --- | --- |
| [`CURRENT_STATE.md`](./CURRENT_STATE.md) | Single source of platform truth |
| [`ARCHITECTURE.md`](./ARCHITECTURE.md) | Package layers, forbidden edges |
| [`CONTRACTS.md`](./CONTRACTS.md) | Trace, errors, headers, system-compat |
| [`DEVELOPMENT.md`](./DEVELOPMENT.md) | Build, local ports, Docker |
| [`TESTING.md`](./TESTING.md) | Tests, CI, conformance kits |
| [`INTEGRATION.md`](./INTEGRATION.md) | Consumer blueprints and package mode |
| [`KNOWN_LIMITATIONS.md`](./KNOWN_LIMITATIONS.md) | Limitations and non-goals |

---

## Operators and contracts

| Area | Path |
| --- | --- |
| Operator contracts | [`operators/`](./operators/) |
| Cross-service gates | [`contracts/`](./contracts/) |
| Protocol registry | [`system/`](./system/) |
| Terminology | [`operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md`](./operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md) |
| Documentation standard (six repos) | [`operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md`](./operators/ONTOGONY_DOCUMENTATION_STRUCTURE_STANDARD.md) |

---

## Packages and governance

| Area | Path |
| --- | --- |
| Per-package docs | [`packages/`](./packages/) |
| Package dependency matrix | [`architecture/package-levels.md`](./architecture/package-levels.md) |
| ADRs | [`adr/`](./adr/) |
| Migrations | [`migrations/`](./migrations/) |
| Consumer blueprints | [`consumer-blueprints/`](./consumer-blueprints/) |
| Consumer compatibility | [`governance/`](./governance/) |
| Quality / coverage policy | [`quality/`](./quality/) |

---

## Evidence

Platform gate verification only: [`evidence/README.md`](./evidence/README.md).

**GOVERNED-FAKE-E2E-001** — passed locally (smoke + Docker-live Playwright); see [`evidence/GOVERNED_FAKE_E2E_001_PASS_20260524T102932Z.md`](./evidence/GOVERNED_FAKE_E2E_001_PASS_20260524T102932Z.md).

System runtime baseline: [`allagma-dotnet/docs/evidence/README.md`](https://github.com/uridolan77/allagma-dotnet/blob/main/docs/evidence/README.md).

---

## Docker local

- Compose tree: [`../docker/local-working-system/README.md`](../docker/local-working-system/README.md)
- Settings reference: [`environments/docker-local-working-system/`](./environments/docker-local-working-system/)

---

## Sister repos

| Repo | Docs |
| --- | --- |
| allagma-dotnet | Governed execution, system E2E |
| kanon-dotnet | Semantic authority |
| conexus-dotnet | Model gateway |
| ontogony-frontend | Operator UI |
| ontogony-ui | Shared UI components |
