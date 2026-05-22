# Ontogony Platform

**Ontogony.Platform** is the mechanical infrastructure base for Ontogony backend services.

It provides reusable **mechanics only** — tracing, hashing, error envelopes, HTTP resilience, idempotency, observability, security/actor context, persistence/outbox ports, AI telemetry DTOs, and cross-service compatibility gates. It does **not** own semantic authority, model routing, or governed execution.

```text
Share mechanics. Do not share meaning.
```

**Version:** `0.3.0-alpha.1` (pre-1.0 alpha) · **27** shipping `Ontogony.*` packages · **.NET 9**

Contributor rules: [`AGENTS.md`](AGENTS.md)

---

## Documentation

| Document | Purpose |
| --- | --- |
| [`docs/CURRENT_STATE.md`](docs/CURRENT_STATE.md) | What Platform provides today |
| [`docs/ARCHITECTURE.md`](docs/ARCHITECTURE.md) | Package layers and boundaries |
| [`docs/CONTRACTS.md`](docs/CONTRACTS.md) | Mechanical contract index |
| [`docs/DEVELOPMENT.md`](docs/DEVELOPMENT.md) | Build, ports, Docker-local |
| [`docs/TESTING.md`](docs/TESTING.md) | Tests and CI gates |
| [`docs/INTEGRATION.md`](docs/INTEGRATION.md) | Consumer adoption |
| [`docs/KNOWN_LIMITATIONS.md`](docs/KNOWN_LIMITATIONS.md) | Honest limitations |
| [`docs/README.md`](docs/README.md) | Full docs index |

---

## Build

```powershell
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln --no-restore
dotnet test Ontogony.Platform.sln --no-build
```

---

## Active consumers

- **Conexus.NET** — model gateway substrate (observability, errors, HTTP, security, idempotency, AI telemetry, execution journaling)
- **Allagma.NET** — governed execution substrate (same mechanical spine; no product orchestration rules here)
- **Kanon.NET** — references platform packages per `kanon-dotnet/eng/Ontogony.References.props`

Blueprints: [`docs/consumer-blueprints/README.md`](docs/consumer-blueprints/README.md)

---

## Examples

- `examples/MinimalApiWithOntogonyObservability/` — tracing + outbound correlation
- `examples/MinimalApiWithOntogonyHosting/` — service defaults + health
- `examples/ConexusDotNetSkeleton/` — Conexus compile smoke
- `examples/AllagmaDotNetSkeleton/` — Allagma compile smoke

---

## Related

- [`CHANGELOG.md`](CHANGELOG.md) — breaking changes and PR history
- [`MIGRATION.md`](MIGRATION.md) — consumer migration notes
- [`SECURITY.md`](SECURITY.md) — security policy

Historical extraction reference code: `_donors/` (read-only, not product architecture).
