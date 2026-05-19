# Evidence graph data model

## Core types

```ts
type EvidenceGraph = {
  root: EvidenceNodeRef;
  nodes: EvidenceNode[];
  edges: EvidenceEdge[];
  sources: EvidenceSourceAttempt[];
  completeness: EvidenceCompleteness;
};

type EvidenceNode = {
  id: string;
  kind: EvidenceNodeKind;
  service: "allagma" | "conexus" | "kanon" | "platform";
  label: string;
  status?: string;
  summary?: string;
  identifiers: Record<string, string>;
  links: EvidencePageLink[];
  sourceRefs: string[];
  rawPreview?: unknown;
};

type EvidenceEdge = {
  from: string;
  to: string;
  kind: EvidenceEdgeKind;
  confidence: "direct" | "derived" | "weak" | "unresolved";
  sourceRef?: string;
  reason?: string;
};
```

## Node kinds

```text
allagma.run
allagma.runEvent
allagma.evaluation
allagma.evidenceExport
allagma.auditBundle
allagma.baselineComparison
conexus.modelCall
conexus.executionRun
conexus.routeDecision
kanon.decision
kanon.provenance
kanon.humanGate
platform.trace
platform.correlation
```

## Edge kinds

```text
has_trace
has_correlation
produced_evaluation
has_baseline_comparison
used_model_call
used_route_decision
used_kanon_decision
has_human_gate
has_audit_bundle
has_evidence_export
derived_from
unresolved_expected_link
```
