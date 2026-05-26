# Current state baseline

## Platform

Ontogony.Platform is the shared mechanics base. It provides packages for tracing, hashing, error envelopes, HTTP resilience, idempotency, observability, security/actor context, persistence/outbox ports, AI telemetry DTOs, and compatibility gates.

Baseline score: 8.5/10.

Primary 9+ gap:
- package conformance across consumers is not yet strong enough to be considered system-wide proof.

## Kanon

Kanon is the semantic authority. It owns ontology, policy, provenance, canonical facts, semantic plans, action policies, human gates, decision records, and the reconstructability classifier core.

Baseline score: 8.3/10.

Primary 9+ gap:
- classifier core exists, but cross-service external event classification is not yet closed.

## Allagma

Allagma is the governed execution layer. It owns runs, events, durable execution, human gates, Kanon/Conexus integration, evaluation harnesses, observability, and now Allagma decision-event projection.

Baseline score: 8.0/10.

Primary 9+ gap:
- Allagma decision-events are structurally solid, but Kanon classifier compatibility against real serialized Allagma payloads is not yet proven.

## Conexus

Conexus is the model gateway/model-access authority. It owns model aliases, project keys, provider configuration, routing/fallback, quotas, usage/cost, OpenAI-compatible chat API, streaming, admin evidence, and provider capability runtime.

Baseline score: 8.2/10.

Primary 9+ gap:
- Conexus does not yet emit normalized reconstructability decision events for route/model/quota/cache/streaming decisions.

## System gap

The system has strong local subsystems but needs a proof-backed spine:

```text
Allagma run → Kanon plan/policy → Conexus route/model call → Allagma run evidence → Kanon reconstructability classification
```
