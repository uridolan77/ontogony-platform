# ADR 0004 — Postgres outbox before Kafka

## Status

Accepted

## Context

The Ontogony ecosystem now spans Athanor, Agentor, Conexus, and future protocol recorder services. Shared infrastructure must improve consistency without stealing domain ownership from those services.

## Decision

We will begin with outbox abstractions and a Postgres implementation before adopting Kafka/NATS/Event Hubs.

## Consequences

Prevents premature distributed infrastructure and keeps MVP deployable.

## Non-goals

- This ADR does not define Athanor, Agentor, or Conexus domain behavior.
- This ADR does not require immediate migration of all existing code.
