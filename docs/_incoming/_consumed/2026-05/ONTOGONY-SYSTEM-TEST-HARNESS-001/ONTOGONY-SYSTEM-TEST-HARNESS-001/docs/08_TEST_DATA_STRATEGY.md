# Test Data Strategy

## Principles

1. Use deterministic fake/local providers for normal CI.
2. Use external providers only in explicitly tagged smoke tests.
3. Separate read-only tests from destructive tests.
4. Use fixed IDs/payloads where possible.
5. Every test should create data with a recognizable prefix.
6. Tests must clean up after themselves where supported.
7. Evidence bundles should not contain secrets or raw sensitive content.

## Naming prefixes

```text
sys-test-
sys-e2e-
sys-idem-
sys-gate-
sys-ui-
```

## Recommended seed objects

| Area | Seed object |
|---|---|
| Kanon | `gaming-core` domain pack / ontology version / policy fixtures |
| Allagma | `SummarizePlayerRisk` governed run fixture |
| Conexus | `risk-summary-v0` model alias using fake/local provider |
| Metabole | deterministic transformation fixture |
| Aisthesis | deterministic experience trace and retrieval fixture |
| Frontend | seeded read-only console state |

## Data reset

The harness should eventually support:

```text
scripts/reset-test-data.ps1
scripts/reset-test-data.sh
```

Reset must be environment-aware and should refuse to run against production unless explicitly forced with multiple confirmations.
