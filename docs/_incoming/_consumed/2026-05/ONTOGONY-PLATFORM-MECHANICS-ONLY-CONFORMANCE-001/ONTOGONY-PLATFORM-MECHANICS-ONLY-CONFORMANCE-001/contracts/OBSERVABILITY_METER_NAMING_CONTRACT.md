# Observability meter naming contract

## Purpose

Make cross-service metric names predictable without importing product meaning.

## Naming shape

```text
ontogony.<service>.<mechanical_area>.<measurement>
```

Examples:

- `ontogony.conexus.http.client.duration`
- `ontogony.kanon.errors.count`
- `ontogony.allagma.idempotency.conflict.count`
- `ontogony.metabole.pipeline.duration`
- `ontogony.aisthesis.evidence.ingest.count`

## Forbidden

- provider-specific model names in meter names;
- ontology concept names in Platform meter contracts;
- workflow business names in Platform meter contracts.
