# 01 — Current State Review

## Ontogony.Platform

### What is strong

Ontogony.Platform is correctly positioned as a mechanics substrate. It already provides:

- trace/correlation propagation,
- event envelopes,
- error contracts,
- idempotency primitives,
- redaction and secret-reference mechanics,
- execution journal contracts,
- artifact references,
- replay contracts,
- testing fixtures.

It explicitly avoids product semantics and must continue doing so.

### Gap

There is no neutral evaluation-contract package.

The existing execution journal records mechanical runs, steps, attempts, transitions, and checkpoints, but it intentionally does not evaluate quality, topology, or policy correctness. This is good, but now the system needs separate neutral evaluation contracts.

### Enhancement

Add `Ontogony.Evaluation.Contracts` and optionally `Ontogony.Topology.Contracts`.

Do not put any of the following in Ontogony.Platform:

- task-specific planner logic,
- scoring rubrics for business domains,
- model routing strategy,
- Kanon ontology policy,
- Allagma orchestration behavior.

## Kanon.NET

### What is strong

Kanon already owns the semantic and policy layer:

- ontology versions,
- source bindings,
- canonical facts,
- action policies,
- human gates,
- decision records,
- provenance,
- replay bundle preparation.

The existing action evaluator can allow, deny, or require human gates for action policies.

### Gap

Kanon does not yet authorize execution topology. It can authorize an action, but not the orchestration shape that led to that action.

### Enhancement

Add topology-policy evaluation:

```text
POST /ontology/v0/execution-topologies/evaluate
```

Kanon should decide whether an execution topology is semantically/politically permitted for a given ontology/action/risk profile.

## Conexus.NET

### What is strong

Conexus already owns:

- model alias resolution,
- project overrides,
- provider routing,
- fallback chains,
- price catalog rates,
- usage/cost,
- telemetry,
- idempotency,
- streaming and non-streaming execution.

### Gap

Route choice is operationally present but not yet first-class as a decision artifact that can be linked to run outcomes and eval scores.

### Enhancement

Add route-decision records and model capability profiles. Conexus should expose:

```text
routeDecisionId
requested model alias
resolved provider/model
fallback chain
price/capability profile version
constraints applied
selection reason
```

Do not move business semantic policy into Conexus.

## Allagma.NET

### What is strong

Allagma already owns governed execution:

- run lifecycle,
- Kanon planning,
- MAF workflow shell,
- tool intent proposal,
- registry validation,
- Kanon tool evaluation,
- human gate pause/resume,
- Conexus model call,
- audit bundle,
- restart/replay evidence,
- real tool execution blocked by policy.

### Gap

Allagma currently executes a fixed governed path. It does not yet:

- classify task structure,
- select topology,
- compare against baseline,
- produce eval scores,
- use historical eval evidence to decide whether to escalate topology.

### Enhancement

Add task topology classification, topology-selection events, baseline comparison, and eval harness.

Allagma is the main implementation repo for this program.
