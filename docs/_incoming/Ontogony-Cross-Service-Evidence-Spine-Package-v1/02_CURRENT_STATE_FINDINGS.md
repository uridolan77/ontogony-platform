# Current state findings

## Existing resolver

The frontend already has a trace/correlation resolver under `src/system/correlation`. It can take trace/run/decision/model-call inputs and call:
- Allagma run APIs
- Allagma event APIs
- Conexus execution-run lookup
- Kanon decision lookup
- Kanon decision-by-trace lookup

It then returns a `TraceCorrelationView` with trace ID, correlation ID, Allagma run ID, Kanon decision ID, planning decision ID, human gate ID, Conexus model call ID, and links.

## Current strengths

```text
- Useful frontend resolver exists.
- Cross-service links card exists.
- Query params can carry trace/run/decision/model-call IDs.
- Allagma run detail and evaluation pages already show evidence journeys.
- Conexus observability has request lookup.
- Kanon provenance evidence utilities exist.
- Unit tests cover correlation adapters and trace enrichment.
```

## Current weaknesses

```text
- Resolution is still trace/correlation-oriented, not general ID-oriented.
- Evaluation IDs, baseline comparison IDs, dataset IDs, routeDecisionIds, and audit/evidence bundles are not first-class graph roots.
- There is no canonical graph data model shared across pages.
- There is no single “paste any ID” workbench.
- Missing edges are not consistently explained.
- Browser/e2e proof of complete graph resolution is still thin.
- Export bundles exist but are not yet unified into one cross-service evidence package.
```

## Strategic implication

The next step is not to add more isolated links. The next step is to define the evidence graph as a first-class frontend/application concept and then progressively backfill backend routes where the graph cannot be resolved from current APIs.
