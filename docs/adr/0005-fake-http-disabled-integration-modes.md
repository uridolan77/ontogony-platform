# ADR 0005 — Fake/HTTP/Disabled integration modes

## Status

Accepted

## Context

The Ontogony ecosystem now spans Athanor, Agentor, Conexus, and future protocol recorder services. Shared infrastructure must improve consistency without stealing domain ownership from those services.

## Decision

Integration clients should support Fake, HTTP, and Disabled modes.

## Consequences

This pattern supports local development, tests, partial deployments, and smoke tests.

## Non-goals

- This ADR does not define Athanor, Agentor, or Conexus domain behavior.
- This ADR does not require immediate migration of all existing code.
