# AGENTS.md — Ontogony Platform Contributor Rules

This repository is shared infrastructure. Treat it as the common nervous system for Allagma, Kanon, Conexus, and other runtime services.

## Non-negotiable rule

```text
Share mechanics, not meaning.
```

Do not add product/domain behavior here. If a change requires understanding what canonization, an agent plan, an iGaming refund, or an LLM provider policy means, it belongs in a product repo, not here.

## Allowed content

You may add reusable mechanics such as:

- correlation and trace propagation
- event envelopes and protocol-neutral DTOs
- hashing/canonical JSON/idempotency helpers
- exception-to-HTTP error infrastructure
- configuration validation and startup guards
- resilient HTTP client registration
- simple messaging abstractions
- test doubles and fixtures
- security/current actor context primitives

## Forbidden content

Do not add:

- Athanor canonization, contradiction, snapshot, review, or epistemic semantics
- Allagma run/plan/tool/human-gate orchestration rules
- Conexus model routing/pricing/provider decision logic
- iGaming/business approval rules
- RAG/knowledge graph extraction logic
- UI components
- generic repositories over arbitrary entities
- service-specific database models

## Coding style

- Prefer small sealed records and simple interfaces.
- Prefer explicit names over clever abstractions.
- Keep dependencies minimal.
- Avoid reflection-heavy magic unless there is a clear operational reason.
- Every public type should answer: “which service needs this mechanical behavior?”
- Every package should remain independently understandable.

## Testing expectations

When adding or changing a package:

1. Add focused unit tests.
2. Add at least one integration-style example if the package affects ASP.NET middleware or HTTP clients.
3. Preserve deterministic JSON output for hashes and fingerprints.
4. Do not introduce nondeterminism into tests unless explicitly controlled by `IClock` / fake clock.

## Breaking changes

Breaking changes require:

- a migration note in `docs/migrations/`
- a changelog entry
- an explanation of which repos must change: Allagma, Kanon, Conexus, or other consumers

## Documentation intake (`docs/_incoming`)

- Only `_active/`, `_consumed/`, and `_incoming/README.md` may sit directly under `docs/_incoming/`.
- **Active** packages: `docs/_incoming/_active/<PACKAGE-NAME>/` — work in progress only.
- **When done:** promote durable content to canonical `docs/`, then **move the package folder** to `docs/_incoming/_consumed/<YYYY-MM>/<PACKAGE-NAME>/`, add `CONSUMED.md`, update both `MANIFEST.md` files, fix links. Do not leave completed packages in `_active`.
- Full procedure: [`docs/_incoming/README.md`](docs/_incoming/README.md).

## Review checklist

Before merging, check:

- [ ] Is this infrastructure and not domain meaning?
- [ ] Does this create hidden coupling between services?
- [ ] Can a service opt out or replace the implementation?
- [ ] Does the package avoid direct dependencies on product repos?
- [ ] Are trace IDs and error shapes stable?
- [ ] Are secrets never logged?
- [ ] Are payload hashes deterministic?
