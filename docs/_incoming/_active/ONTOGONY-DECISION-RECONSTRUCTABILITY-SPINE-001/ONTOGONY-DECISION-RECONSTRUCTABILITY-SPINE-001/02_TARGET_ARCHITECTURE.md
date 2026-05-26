# Target Architecture

## Conceptual model

```text
Allagma run / operation / human gate
        │
        ├── DecisionEventFragment: lifecycle, actor, operation, state-before/after
        │
Conexus route decision / model call / tool call
        │
        ├── DecisionEventFragment: model/provider, routing policy, tool request, output action
        │
Kanon decision / policy / ontology / provenance
        │
        ├── DecisionEventFragment: policy basis, semantic authority, decision lifecycle, audit
        │
        ▼
DecisionEventAssembler
        ▼
ReconstructabilityClassifier
        ▼
ReconstructabilityReport + MissingEvidenceDiagnostics
        ▼
Evidence Spine Graph + Operator Console
```

## Cross-service responsibilities

### Allagma

Owns orchestration and lifecycle evidence:

- run id;
- operation id;
- workflow status;
- actor context;
- human gate state;
- operation selected;
- state before/after;
- correlation id and trace id.

### Conexus

Owns model and routing evidence:

- model call id;
- route decision id;
- provider profile;
- target model;
- routing policy id/version;
- input/output hashes;
- tool-call metadata;
- guardrail / fallback / retry metadata.

### Kanon

Owns semantic authority and governance interpretation:

- policy basis;
- ontology version;
- domain pack;
- semantic decision id;
- provenance/audit entries;
- classification algorithm;
- missing evidence diagnostics;
- governance status.

### Frontend/UI

Owns operator comprehension:

- property matrix;
- score summary;
- blocking warnings;
- missing evidence list;
- linked evidence fragments;
- before/after state view;
- reconstruction timeline.

## Recommended data flow

1. Services emit or expose typed evidence fragments using existing DTO conventions.
2. Kanon or an Evidence Spine service assembles the fragments into a `DecisionEvent` view.
3. The classifier runs deterministic classification over the view.
4. The report is exposed through a read endpoint and linked from existing Evidence Spine graph nodes.
5. The frontend fetches the report for any decision/run/model-call/action node.

## Important separation

Do not couple reconstructability classification to task success. A failed action can be fully reconstructable. A successful action can be governance-opaque.

```text
Task correctness answers: Did the agent solve the problem?
Reconstructability answers: Can we reconstruct why and under what authority it acted?
```
