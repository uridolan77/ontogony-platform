# 03 — Cross-Repo Phase Roadmap

## Phase 0 — Evaluation foundation

Goal: introduce neutral contracts and documentation without changing runtime behavior.

PRs:

- `PLAT-EVAL-001` — Neutral evaluation contract package.
- `PLAT-TOPO-001` — Neutral topology contract vocabulary.
- `SYS-EVAL-DOC-001` — Cross-repo eval terminology and ownership doc.

Outcome:

- All repos can reference the same neutral eval/topology DTOs.
- No product semantics enter Ontogony.Platform.

## Phase 1 — Allagma topology capture

Goal: make current runtime topology explicit without changing behavior.

PRs:

- `AGM-TOPO-001` — Task classification and topology selection.
- `AGM-TOPO-002` — Topology events in run audit bundle.
- `AGM-TOPO-003` — Topology metadata in persistence/query APIs.

Outcome:

- Every run records classification and selected topology.
- Default topology remains `single_workflow`.
- No parallel execution yet.

## Phase 2 — Kanon topology policy

Goal: let Kanon authorize or reject topology choices.

PRs:

- `KANON-TOPO-001` — Topology policy evaluation endpoint.
- `KANON-TOPO-002` — Domain-pack topology policy definitions.
- `KANON-TOPO-003` — Decision provenance links for topology decisions.

Outcome:

- Allagma can ask Kanon whether a selected topology is semantically permitted.
- High-risk topologies can require human gates.

## Phase 3 — Conexus route evidence

Goal: make model-routing decisions evaluable.

PRs:

- `CX-ROUTE-EVIDENCE-001` — Route-decision records.
- `CX-MODEL-CAP-001` — Model capability profiles.
- `CX-EVAL-FEEDBACK-001` — Model-quality feedback ingestion.

Outcome:

- Every model call can be linked to a route decision.
- Eval results can later inform model alias/provider policy.

## Phase 4 — Eval harness and baseline comparisons

Goal: evaluate topology decisions against single-workflow baseline.

PRs:

- `AGM-EVAL-001` — Eval harness and scenario runner.
- `AGM-EVAL-002` — Baseline comparison records.
- `AGM-EVAL-003` — Eval evidence export.
- `SYS-EVAL-001` — Cross-repo eval smoke.

Outcome:

- Existing first-system flow has machine-readable eval scores.
- Baseline comparison is available before topology expansion.

## Phase 5 — Observability and operatorization

Goal: expose eval/topology signals.

PRs:

- `SYS-OBS-EVAL-001` — Eval metrics and dashboards.
- `SYS-OPS-EVAL-001` — Operator runbook and triage guide.
- `SYS-REL-EVAL-001` — Release evidence gate.

Outcome:

- Operators can see topology choice, score, baseline comparison, cost, latency, and failure class.

## Phase 6 — Controlled topology expansion

Goal: add parallel/hybrid modes only after evidence exists.

Candidate PRs:

- `AGM-TOPO-010` — Parallel review simulation mode.
- `AGM-TOPO-011` — Centralized validation bottleneck.
- `AGM-TOPO-012` — Promotion rules based on eval evidence.

Outcome:

- Multi-agent-style patterns are added only when measurable value is demonstrated.
