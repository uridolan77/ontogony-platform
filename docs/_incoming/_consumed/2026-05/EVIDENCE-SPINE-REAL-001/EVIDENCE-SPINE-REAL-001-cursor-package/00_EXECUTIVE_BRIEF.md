# 00 — Executive Brief

## Work item

`EVIDENCE-SPINE-REAL-001 — Route decision, Kanon applicability, graph merge, missing reasons`

## Why this exists

The Evidence Spine already resolves many cross-service nodes. However, the current operator evidence proves several truth defects:

1. A Conexus `routeDecisionId` can be present, but route-decision detail resolution fails.
2. The source attempt error for route-decision lookup is generic: “An unexpected error occurred.”
3. The graph can include both authoritative and placeholder nodes for the same entity.
4. A direct Conexus call can be interpreted as missing a Kanon decision, even when no governed flow existed.
5. Model-call/request/execution IDs are not clearly separated.
6. Allagma routes are normalized inconsistently.
7. Partial graph status does not tell the operator exactly which relationships are missing and why.

## Architectural principle

Evidence Spine must be a truth-preserving resolver, not a completeness theater. It is better to say:

```text
not_applicable: direct Conexus call did not pass through Kanon
```

than to say:

```text
missing Kanon decision
```

It is better to say:

```text
backend_missing: routeDecisionId was recorded, but no detail row exists
```

than to show:

```text
An unexpected error occurred
```

## Scope

In scope:

- shared resolution result shape;
- stable missing-reason codes;
- graph node canonicalization and placeholder merge;
- route-decision lookup truth;
- Kanon applicability semantics;
- source-attempt display cleanup;
- Allagma route-template normalization;
- distinct ID display for model call/request/execution run;
- governed fake-provider E2E proof;
- export bundle schema update, if needed.

Out of scope:

- real external tool execution;
- full health/readiness standardization across all services;
- broad Kanon console polish;
- broad Agent Interaction rebuild;
- production security/IAM;
- model-provider production readiness.

## Definition of done

A governed fake-provider run resolves through Evidence Spine with:

- no duplicate placeholder/resolved nodes for the same canonical entity;
- no generic `unexpected error` source attempt;
- route-decision evidence either resolved or reported with a stable structured reason;
- Kanon links marked `required` only when the root identifier belongs to a governed Kanon/Allagma flow;
- direct Conexus model-call root marks Kanon decision relationship as `not_applicable`, not missing;
- exported bundle includes graph, attempts, missing reasons, applicability, warnings, and build metadata.
