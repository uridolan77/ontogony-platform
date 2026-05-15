# Master Prompt — Ontogony.Platform Next Phase

You are working in `ontogony-platform`.

Goal:
Implement the next platform phase while preserving:

```text
Share mechanics. Do not share meaning.
```

Recommended order:

1. PLAT-INT-001 — service-to-service integration conventions.
2. PLAT-OBS-001 — integration metrics helper.
3. PLAT-TEST-001 — architecture test helpers.
4. Defer durable Postgres idempotency/journal extraction until reused.
5. Defer durable artifact provider until needed.

Do not add:
- Kanon ontology semantics;
- Agentor runtime semantics;
- Conexus provider routing;
- provider SDKs;
- product clients.

Run build/test and update docs/CHANGELOG/public API snapshots as needed.
