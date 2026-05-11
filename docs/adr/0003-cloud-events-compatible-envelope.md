# ADR 0003 — CloudEvents-compatible envelope

## Status

Accepted

## Context

The Ontogony ecosystem now spans Athanor, Agentor, Conexus, and future protocol recorder services. Shared infrastructure must improve consistency without stealing domain ownership from those services.

## Decision

We will use a simple internal event envelope that is compatible in spirit with CloudEvents while remaining ergonomic for .NET services.

## Consequences

Keeps the system protocol-neutral and future-compatible with event buses.

## Non-goals

- This ADR does not define Athanor, Agentor, or Conexus domain behavior.
- This ADR does not require immediate migration of all existing code.
