# PR-PLAT-NP-013 — Consumer-State Docs Reconciliation

## Purpose

Ontogony.Platform is now an active package substrate consumed by Conexus.NET in sibling-source mode and package mode. Documentation must stop describing Conexus as merely a future target.

## Non-goals

- No new packages.
- No public API changes.
- No durable-store expansion.
- No cloud secret resolver packages.
- No Conexus product semantics.

## Required README replacement

Replace stale language such as:

```text
It is safe to break before v1 because no external consumers exist. The first target consumer is Conexus.NET.
```

with:

```text
Ontogony.Platform is pre-1.0 and still evolving, but Conexus.NET is now its first active consumer. Breaking changes are allowed only with public API snapshot review, CHANGELOG notes, migration guidance when applicable, and Conexus compatibility validation.
```

## Required changes

1. Add an active-consumer section.
2. Clarify pre-1.0 versioning now that Conexus consumes packages.
3. Link:
   - `docs/VERSION_COMPATIBILITY_MATRIX.md`
   - `docs/public-api-review.md`
   - `docs/consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md`
   - release evidence
   - security/supply-chain evidence
4. Keep PLAT-NP-008 as a future maintenance guard.

## Acceptance criteria

- README no longer says no active consumers exist.
- Conexus.NET is described as active alpha consumer.
- Public API governance is linked.
- Conexus package-mode proof is linked.
- No code or package expansion.
