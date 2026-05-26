# Expected Governed Fake-Provider Chain

A successful EVIDENCE-SPINE-REAL-001 proof should look like this.

## Root

```text
allagmaRunId: run_...
```

## Required identifiers

```text
allagmaRunId: run_...
planningDecisionId: decision_...
kanonDecisionId: decision_...
conexusModelCallId: chatcmpl-...
requestId: 0HN...
executionRunId: chat-0HN...
conexusRouteDecisionId: rd-...
ontologyVersionId: gaming-core@0.1.0
traceId: ...
correlationId: ...
```

## Required nodes

```text
allagma.run
allagma.runEvent[]
allagma.auditBundle
kanon.decision
kanon.provenance
kanon.ontologyVersion
kanon.semanticPlan
kanon.policy/actionPolicy
conexus.modelCall
conexus.providerAttempt
conexus.executionRun
conexus.routeDecision
platform.trace
platform.correlation
```

## Required edges

```text
Allagma run -> Kanon decision                  used_kanon_decision
Allagma run -> Conexus model call              used_model_call
Allagma run -> Run events                      derived_from
Allagma run -> Audit bundle                    has_audit_bundle
Kanon decision -> Ontology version             used_ontology_version
Kanon decision -> Provenance                   derived_from
Kanon decision -> Trace                        has_trace
Kanon decision -> Correlation                  has_correlation
Conexus model call -> Provider attempt         derived_from
Conexus model call -> Execution run            derived_from
Conexus model call -> Route decision           used_route_decision
Conexus model call -> Trace                    has_trace
Conexus model call -> Correlation              has_correlation
```

## Allowed partial states

Only acceptable partial states after this package:

- `not_recorded`: backend intentionally does not persist optional evidence;
- `not_applicable`: relationship is not expected for root kind;
- `authorization_failed`: operator lacks current credential/role;
- `backend_missing`: ID was emitted but detail row is absent, treated as a bug or migration gap.

Not acceptable:

```text
An unexpected error occurred.
unknown
missing with no reason
route detail was not loaded with no endpoint/status
placeholder duplicate of a resolved node
```
