# Implementation order

## PR-001 — Allagma → Kanon classifier closure

Repo focus:

```text
allagma-dotnet
kanon-dotnet, only if classifier/client contract mismatch is discovered
```

Goal:

```text
Feed Allagma decision-events into Kanon classifier and assert high/critical events do not FAIL.
```

Deliverables:
- Allagma fixture builder or test-host flow producing real `RunDecisionEventsResponse`.
- Classifier client/test adapter.
- Golden Allagma decision-event fixture.
- Tests:
  - no dangling fragment refs;
  - no high/critical FAIL from Kanon classifier;
  - classifier response is persisted/exportable as evidence.

## PR-002 — Conexus decision-event emitter contract

Repo focus:

```text
conexus-dotnet
kanon-dotnet only if reusable contract DTOs need minor compatibility changes
```

Goal:

```text
Define Conexus normalized decision-event shape and map existing model-call/routing evidence into it.
```

Deliverables:
- Contracts for Conexus decision-event response.
- Projector for route decisions/model calls/provider attempts/quota/cache/streaming lifecycle.
- API endpoint(s) or admin evidence extension.
- Tests for shape, redaction, route inventory, no raw prompt leakage.

## PR-003 — Conexus → Kanon classifier closure

Repo focus:

```text
conexus-dotnet
kanon-dotnet
```

Goal:

```text
Classify Conexus decision events through Kanon; high/critical events must not FAIL.
```

Deliverables:
- Fixture from Conexus model-call evidence.
- Classifier integration test.
- Evidence diagnostics.
- Fix Conexus emitters until classifier output is acceptable.

## PR-004 — Cross-service golden trace

Repo focus:

```text
allagma-dotnet as orchestrator/evidence repo
kanon-dotnet
conexus-dotnet
```

Goal:

```text
One run, one trace, multi-service decision events, one reconstructability report.
```

Deliverables:
- Guided local script.
- Golden trace JSON bundle.
- Markdown evidence report.
- Machine-readable summary.
- CI-compatible non-provider fake-mode test.

## PR-005 — Platform consumer conformance kits

Repo focus:

```text
ontogony-platform
allagma-dotnet
kanon-dotnet
conexus-dotnet
```

Goal:

```text
Consumers prove they use Platform mechanics correctly.
```

Deliverables:
- Conformance kits for headers/correlation, error envelope, idempotency, observability naming, artifact/export safety.
- Consumer adoption tests.
- Compatibility/adoption manifest.

## PR-006 — Optional frontend panel contract

Repo focus:

```text
ontogony-frontend
ontogony-ui
```

Start only after PR-004 is green.

Goal:

```text
Expose reconstructability results to operators without inventing frontend semantics.
```
