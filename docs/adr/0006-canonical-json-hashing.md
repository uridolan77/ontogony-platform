# ADR 0006 — Canonical JSON hashing

## Status

Accepted

## Context

The Ontogony ecosystem now spans Athanor, Agentor, Conexus, and future protocol recorder services. Shared infrastructure must improve consistency without stealing domain ownership from those services.

## Decision

Hashes/fingerprints should use compact JSON with recursively sorted object keys.

## Consequences

Equivalent payloads should produce the same idempotency keys regardless of object key order.

## Non-goals

- This ADR does not define Athanor, Agentor, or Conexus domain behavior.
- This ADR does not require immediate migration of all existing code.
