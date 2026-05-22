# Mock Services Harness Sketch

Use these fixtures to test UI and adapter behavior without live services.

Recommended commands once implemented:

```bash
npm test -- src/agent-interaction
npm run test:run -- --run src/agent-interaction/tests/jsonlFixtureAdapter.test.ts
```

Suggested mock server endpoints:

```http
GET /mock/agent-interaction/sample-run.jsonl
GET /mock/agent-interaction/sample-human-gate-interrupt.jsonl
```

The mock server should support deterministic replay with artificial delay:

```http
GET /mock/agent-interaction/sample-run/stream?delayMs=250
```
