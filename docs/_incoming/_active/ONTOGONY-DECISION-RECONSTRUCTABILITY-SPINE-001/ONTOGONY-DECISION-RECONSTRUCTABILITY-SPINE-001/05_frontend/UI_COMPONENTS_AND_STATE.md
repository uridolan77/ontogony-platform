# Frontend Components and State

## `ontogony-ui` shared components

Candidate components:

```text
ReconstructabilityBadge
ReconstructabilityScoreCard
ReconstructabilityPropertyTable
MissingEvidenceDiagnosticsList
EvidenceFragmentRefsList
DecisionReconstructionHeader
ReasoningEvidenceNotice
PostConditionStateSummary
```

Keep these presentational and service-agnostic where possible.

## `ontogony-frontend` feature components

Candidate feature module:

```text
src/features/decisionReconstruction/
```

Suggested files:

```text
api/getDecisionReconstruction.ts
types/decisionReconstruction.ts
components/DecisionReconstructionPanel.tsx
components/DecisionReconstructionDrawer.tsx
components/DecisionReconstructionRoute.tsx
components/DecisionReconstructionSummaryCard.tsx
utils/classifyReconstructabilityClientFallback.ts
fixtures/decisionReconstructionFixtures.ts
```

Client fallback classification should only be used for fixture/demo mode. Backend classification is authoritative.

## Data-fetching behavior

Preferred backend call:

```ts
getDecisionReconstruction(decisionEventId)
```

Convenience calls:

```ts
getDecisionReconstructionByRun(runId)
getDecisionReconstructionByTrace(traceId)
getDecisionReconstructionByModelCall(modelCallId)
getDecisionReconstructionByKanonDecision(kanonDecisionId)
```

## Visual grammar

Suggested badge labels:

```text
F  Complete
P  Partial
S  Not applicable / structural
O  Opaque
```

Suggested status labels:

```text
PASS  Governance evidence complete enough
WARN  Non-blocking evidence gaps
FAIL  Blocking evidence missing
```

## Console cleanup rule

Do not add another dense wall of badges to already crowded pages. Use:

- one compact status chip in list/table views;
- a drawer/panel for details;
- collapsible raw evidence;
- clear owner-service suggestions.

## Tests

Add tests for:

- full report rendering;
- missing evidence rendering;
- hidden chain-of-thought notice rendering;
- fixture label rendering;
- drawer opens from Evidence Spine node;
- property classification badge text and accessible labels.
