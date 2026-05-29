# Platform mechanics-only proposal gate

## Gate question

Every Platform proposal must answer:

```text
Can this be reused by Conexus, Kanon, Allagma, Metabole, and Aisthesis without importing product meaning?
```

## Required fields for proposals

- `proposalId`
- `title`
- `targetPackage`
- `mechanicalConcern`
- `consumers`
- `reuseJustification`
- `forbiddenSemanticsAssessment`
- `ownerRoutingDecision`
- `schemasTouched`
- `testsAdded`
- `conformanceImpact`

## Accepted mechanical concerns

- trace/correlation;
- error envelope;
- header propagation;
- actor context;
- idempotency;
- outbox/artifact references;
- neutral evidence references;
- HTTP resilience;
- redaction/secret references;
- mechanical schema validation;
- protocol-neutral DTOs;
- observability meter naming;
- consumer conformance harnesses.

## Rejected concerns

- ontology/canonical fact acceptance;
- provider/model routing;
- workflow run policy;
- human approval semantics;
- source-specific mapping logic;
- reconstructability scoring;
- UI layout;
- product domain workflows.

## Decision states

- `accepted_platform_mechanics`
- `rejected_product_semantics`
- `needs_owner_routing`
- `needs_schema_versioning`
- `needs_consumer_impact_review`
