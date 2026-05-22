# Architecture — mechanics only

Ontogony.Platform is the **mechanical substrate** for Ontogony backend services. It answers *how* services trace, hash, format errors, propagate headers, and publish events — not *what* is canonically true or which model to route.

```text
Ontogony.Platform = shared mechanics
Kanon             = semantic authority
Allagma           = governed execution
Conexus           = model gateway
```

See [`operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md`](./operators/ONTOGONY_TERMINOLOGY_GLOSSARY.md).

---

## Strategic rule

```text
Share mechanics. Do not share meaning.
```

**Allowed here:** trace context, error shape, hashing, idempotency ledger ports, HTTP resilience handlers, actor propagation, secret references, mechanical quota windows, protocol-neutral DTOs.

**Forbidden here:** canonization, agent plans, provider routing policy, business approval rules, RAG/graph extraction, UI.

Full contributor rules: [`../AGENTS.md`](../AGENTS.md).

---

## Package layers (27 shipping libraries)

Levels are a **mental model**. Authoritative `ProjectReference` edges are in [`architecture/package-levels.md`](./architecture/package-levels.md) and `scripts/validate-package-levels.ps1`.

```text
Level 0 — Pure foundation
  Ontogony.Primitives, Ontogony.Configuration

Level 0.5 — Shared representation
  Ontogony.Contracts, Ontogony.Hashing

Level 1 — Service mechanics
  Ontogony.Hosting, Ontogony.Observability, Ontogony.Errors, Ontogony.Http,
  Ontogony.Security, Ontogony.Logging, Ontogony.Redaction, Ontogony.Secrets,
  Ontogony.Secrets.AzureKeyVault (optional adapter)

Level 2 — Event, consistency, persistence mechanics
  Ontogony.Messaging, Ontogony.Idempotency, Ontogony.Persistence,
  Ontogony.Persistence.Postgres, Ontogony.ProtocolIngress, Ontogony.Quotas

Level 3 — AI runtime mechanics
  Ontogony.AI.Contracts, Ontogony.Artifacts, Ontogony.Evaluation.Contracts,
  Ontogony.Execution, Ontogony.Replay.Contracts, Ontogony.Topology.Contracts

Cross-cutting gates
  Ontogony.SystemCompatibility

Aggregate (not a runtime tier)
  Ontogony.Testing — test fixtures only; shipping libs must not reference it
```

Per-package guarantees: [`packages/`](./packages/).

---

## Ownership — where product concepts belong

| Concept | Owning repo |
| --- | --- |
| Ontology, canonical facts, semantic decisions | Kanon |
| Runs, tool intents, human gates, eval orchestration | Allagma |
| Provider routing, model aliases, gateway quota policy | Conexus |
| iGaming / domain workflows | Domain app or plugin |

Platform may define **neutral references** (e.g. `EntityRef`) but not semantic finality types.

---

## Forbidden dependency rules (summary)

1. No `ProjectReference` edges outside the golden matrix (CI: `validate-package-levels.ps1`).
2. `Ontogony.Execution` must not reference `Ontogony.Artifacts` directly.
3. AI runtime packages stay provider-neutral (`validate-ai-runtime-boundaries.ps1`).
4. Shipping libraries must not reference `Ontogony.Testing`.
5. No references to product repo **implementation** assemblies from Platform.

---

## Repository layout

```text
src/           # 27 Ontogony.* packages
tests/         # Unit and integration tests
docs/          # This documentation spine
examples/      # Compile-only consumer smokes
scripts/       # Validation and gates
schemas/       # JSON schemas for mechanical contracts
docker/        # Local working-system compose (operator program)
```

Historical extraction sources live under `_donors/` (read-only reference, not active architecture).

---

## Related

- [`CURRENT_STATE.md`](./CURRENT_STATE.md) — what is implemented today
- [`CONTRACTS.md`](./CONTRACTS.md) — mechanical contract index
- [`INTEGRATION.md`](./INTEGRATION.md) — how consumers adopt packages
- [`adr/`](./adr/) — architecture decision records
