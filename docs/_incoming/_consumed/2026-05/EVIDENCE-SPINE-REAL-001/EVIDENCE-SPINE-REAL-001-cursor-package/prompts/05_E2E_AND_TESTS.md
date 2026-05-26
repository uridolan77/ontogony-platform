# Prompt 05 — E2E and Tests

```text
Add EVIDENCE-SPINE-REAL-001 tests and a repeatable validation path.

Test layers:

1. Frontend unit tests:
   - applicability matrix;
   - missing reason mapping;
   - graph canonicalization;
   - ID normalization;
   - source attempt rendering;
   - export bundle shape.

2. Conexus contract/integration tests:
   - fake provider chat creates model call;
   - evidence links include routeDecisionId only if route decision detail can be resolved or typed as not recorded;
   - route decision endpoint returns 200 or typed 404.

3. Kanon tests:
   - semantic graph reference nodes include canonical IDs and placeholder authority;
   - decision provenance contains IDs needed by resolver.

4. Allagma tests:
   - fake-provider governed run exposes planningDecisionId and Conexus modelCallId;
   - run/events/audit/evaluation routes use /allagma/v0 templates;
   - replay/audit bundle contains IDs needed for Evidence Spine.

5. Manual/local E2E script:
   - start stack;
   - run fake-provider governed run;
   - resolve Evidence Spine by run ID;
   - export bundle;
   - assert no generic errors, no duplicate nodes, and required chain present.

Output a final evidence summary with exact test commands and result counts.
```
