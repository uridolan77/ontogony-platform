# EVIDENCE-SPINE-REAL-001 — Cursor Implementation Package

## Purpose

Make the Evidence Spine behave like a real cross-service evidence resolver, not a mostly-live diagnostic page with fixture-era assumptions.

This package targets the concrete issues observed in the operator console:

- route decision IDs appear in evidence but `/admin/v0/route-decisions/{routeDecisionId}` fails with a generic error;
- direct Conexus model calls can be treated as if a Kanon decision is always expected;
- Kanon semantic-graph placeholder nodes can duplicate resolved Allagma/Kanon/Conexus nodes;
- model call IDs, request IDs, and execution run IDs are conflated;
- Allagma route templates are inconsistent (`/runs/{runId}` vs `/allagma/v0/runs/{runId}`);
- source attempts expose generic errors instead of stable reason codes;
- partial graph completeness does not explain what is missing;
- exports and page links can look more complete than the evidence actually is.

## Target outcome

After this work, one governed fake-provider run should resolve cleanly across:

```text
Allagma run
  -> Kanon planning decision
  -> Kanon decision provenance
  -> Kanon semantic graph
  -> Conexus model call
  -> Conexus evidence links
  -> Conexus route decision
  -> Conexus execution run / provider attempt
  -> Allagma run events / audit / replay / evaluations
  -> exported Evidence Spine bundle
```

The UI must distinguish:

- resolved evidence;
- partial evidence;
- not applicable relationships;
- recorded but not yet resolved relationships;
- backend-missing relationships;
- authorization failures;
- fixture/demo-only evidence;
- not implemented evidence.

## Repositories likely involved

Verify actual paths first. Expected repositories:

- `ontogony-frontend` — Evidence Spine resolver, graph model, page rendering, export bundle, tests.
- `conexus-dotnet` — route decision detail lookup and evidence-link consistency.
- `kanon-dotnet` — semantic graph edge completeness and provenance semantics.
- `allagma-dotnet` — run/evidence/replay IDs, route template normalization, live chain seed.
- `ontogony-ui` — only if shared evidence/status primitives need to be extracted.

## Package structure

```text
README.md
00_EXECUTIVE_BRIEF.md
01_MASTER_CURSOR_PROMPT.md
02_IMPLEMENTATION_PLAN.md
03_ACCEPTANCE_CHECKLIST.md
04_REGRESSION_TEST_MATRIX.md
contracts/
  EVIDENCE_RESOLVER_CONTRACT.md
  MISSING_REASON_CODES.md
  GRAPH_CANONICALIZATION_CONTRACT.md
  SOURCE_ATTEMPT_CONTRACT.md
  ROUTE_DECISION_EVIDENCE_CONTRACT.md
prompts/
  00_REPO_AUDIT.md
  01_FRONTEND_RESOLVER.md
  02_CONEXUS_ROUTE_DECISION.md
  03_KANON_SEMANTIC_GRAPH.md
  04_ALLAGMA_REPLAY_AND_LINKAGE.md
  05_E2E_AND_TESTS.md
  06_FINAL_REVIEW.md
schemas/
  evidence-spine-resolution-result.schema.json
  evidence-missing-reason-code.schema.json
scripts/
  validate-evidence-spine-real-001.ps1
  validate-evidence-spine-real-001.sh
examples/
  expected-governed-fake-provider-chain.md
  expected-source-attempt-display.md
```

## Cursor operating rule

Do not blindly apply this package as a patch. Use it as a repo-aware implementation brief:

1. inspect the actual current code;
2. identify exact files and contracts;
3. implement the smallest coherent change that satisfies the acceptance criteria;
4. add tests before or alongside changes;
5. avoid stale docs and fixture-only success claims.
