# TASK-003 — Conexus Readiness Detail

## Goal

Explain Conexus local readiness precisely.

## Required checks

```text
conexus.postgres.connection
conexus.postgres.migrations
conexus.projects.default
conexus.project_key.dev
conexus.provider_registry.fake
conexus.provider_registry.openai
conexus.provider_credentials.openai
conexus.routing.aliases
conexus.routing.project_overrides
conexus.route_preview.default
conexus.model_call_store
conexus.route_decision_store
conexus.telemetry.store
conexus.admin_auth
```

## Important policy

Missing OpenAI API key should be optional warning unless an enabled alias requires OpenAI.

## Acceptance

The operator can see exactly why Conexus is ready/degraded/not-ready.
