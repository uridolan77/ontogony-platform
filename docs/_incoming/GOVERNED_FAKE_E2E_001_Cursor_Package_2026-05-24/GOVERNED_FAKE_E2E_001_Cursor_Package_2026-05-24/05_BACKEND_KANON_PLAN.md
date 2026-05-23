# 05 — Backend Plan: Kanon

## Goal

Kanon must emit a semantic/planning decision that Evidence Spine can resolve for the governed fake run.

## Required behavior

For the governed fake path:

1. Allagma calls Kanon for semantic plan / compile intent / policy decision.
2. Kanon records a decision with:
   - `decisionId`
   - `decisionType`
   - `traceId`
   - `correlationId`
   - `actorId`
   - `ontologyVersionId`
   - relevant entity/run references
3. Kanon exposes that decision through:
   - direct decision endpoint;
   - by-trace endpoint;
   - provenance endpoint where available;
   - replay bundle endpoint if supported.

## Candidate code areas

```text
kanon-dotnet/src/Kanon.Api/
kanon-dotnet/src/Kanon.Application/*Decision*
kanon-dotnet/src/Kanon.Application/*Semantic*
kanon-dotnet/src/Kanon.Application/*Provenance*
kanon-dotnet/src/Kanon.Application/*Replay*
kanon-dotnet/tests/Kanon.Tests/
```

## Required endpoints for this sprint

At minimum:

```text
GET /ontology/v0/decision-records/{decisionId}
GET /ontology/v0/decision-records/by-trace/{traceId}
GET /ontology/v0/decision-records/{decisionId}/provenance
```

If replay bundle exists:

```text
GET /ontology/v0/decision-records/{decisionId}/replay-bundles
```

## Applicability rule

Kanon decision is required when the root is:

```text
Allagma governed run
Kanon decision
governed fake e2e trace
semantic plan / compile intent
```

Kanon decision is not required when the root is:

```text
direct Conexus chat
standalone model call with no governed-run marker
```

## Acceptance

- A governed fake run produces at least one Kanon decision.
- That decision is returned by trace lookup.
- Evidence Spine links the Allagma run to the Kanon decision using live identifiers.
