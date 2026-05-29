# Platform mechanics-only proposal gate

**Package:** `ONTOGONY-PLATFORM-MECHANICS-ONLY-CONFORMANCE-001`  
**Status:** promoted (2026-05-29)

## Gate question

Every Platform proposal must answer:

```text
Can this be reused by Conexus, Kanon, Allagma, Metabole, and Aisthesis without importing product meaning?
```

## Validate a proposal

```powershell
.\scripts\governance\check-platform-mechanics-only.ps1 `
  -RepoRoot . `
  -ProposalPath .\fixtures\mechanics\v1\valid\platform-proposal-accepted.json
```

Rejected example (must exit non-zero when routed to platform):

```powershell
.\scripts\governance\check-platform-mechanics-only.ps1 `
  -ProposalPath .\fixtures\mechanics\v1\invalid\platform-proposal-rejected.json
```

## Required fields for proposals

Schema: `schemas/mechanics/v1/platform-proposal-gate.schema.json`

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

## Related

- [`NO_PRODUCT_SEMANTICS_CONTRACT.md`](../contracts/NO_PRODUCT_SEMANTICS_CONTRACT.md)
- `scripts/check-no-product-semantics.ps1`
- `tests/Ontogony.Architecture.Tests/MechanicsOnlyProposalGateTests.cs`

