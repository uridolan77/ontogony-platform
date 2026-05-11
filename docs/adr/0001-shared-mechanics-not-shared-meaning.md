# ADR 0001 — Shared mechanics, not shared meaning

## Status

Accepted

## Context

The Ontogony ecosystem now spans Athanor, Agentor, Conexus, and future protocol recorder services. Shared infrastructure must improve consistency without stealing domain ownership from those services.

## Decision

We will keep this repository limited to cross-cutting infrastructure mechanics. Domain semantics remain in product repos.

## Consequences

Avoids distributed-monolith coupling and preserves service ownership.

## Non-goals

- This ADR does not define Athanor, Agentor, or Conexus domain behavior.
- This ADR does not require immediate migration of all existing code.
