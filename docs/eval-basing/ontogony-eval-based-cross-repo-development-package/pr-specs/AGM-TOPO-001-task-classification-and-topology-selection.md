# PR Spec — AGM-TOPO-001 — Task Classification and Topology Selection

## Repo

`allagma-dotnet`

## Goal

Before execution, classify task structure and record selected topology. Do not change execution behavior yet.

## Add

```text
TaskTopologyClassifier
TopologySelectionService
TaskTopologyClassificationResult
TopologySelectionResult
Allagma.TaskClassified event
Allagma.TopologySelected event
```

## Default behavior

All normal runs select:

```text
single_workflow
```

unless deterministic rules classify them as high-risk, human-gated, or explicitly overridden.

## Deterministic classification v0

Signals:

```text
objective contains consequential probe marker -> high_risk + human_gate_likely
context contains toolIntentHints -> tool_heavy
context contains independentItems count > 1 -> parallelizable
modelPurpose only and no action markers -> model_only
default -> sequential
```

## Run event payloads

Use schemas in `/schemas/topology-selection.schema.json`.

## Audit bundle

Add:

```text
taskClassification
topologySelection
```

to `GET /allagma/v0/runs/{runId}/audit`.

## Tests

- default low-risk request selects `single_workflow`.
- high-risk marker records `high_risk`.
- explicit override records override but does not yet change runtime behavior.
- audit bundle includes topology information.
- event order: `RunCreated` -> `TaskClassified` -> `TopologySelected` -> `KanonPlanRequested`.

## Acceptance

- existing first-system tests still pass.
- no parallel execution added.
- no Kanon topology call in this PR.
