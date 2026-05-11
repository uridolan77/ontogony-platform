# ADR 0002 — Trace correlation first

## Status

Accepted

## Context

The Ontogony ecosystem now spans Athanor, Agentor, Conexus, and future protocol recorder services. Shared infrastructure must improve consistency without stealing domain ownership from those services.

## Decision

We will extract observability and trace propagation before more advanced messaging or protocol recorder packages.

## Consequences

AG-UI/MCP/A2A recording is useless if events cannot be correlated end-to-end.

## Non-goals

- This ADR does not define Athanor, Agentor, or Conexus domain behavior.
- This ADR does not require immediate migration of all existing code.
