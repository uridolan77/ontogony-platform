# Evidence Resolver Contract

## Function shape

All Evidence Spine entry points should converge on one conceptual interface:

```ts
resolveEvidenceSpine(settings, input): Promise<EvidenceResolutionResult>
```

Where input is:

```ts
export type EvidenceLookupInput = {
  raw: string;
  kind?: EvidenceIdentifierKind;
  source?: 'operator_lookup' | 'agent_interaction' | 'replay' | 'topology' | 'export';
  allowFixtureFallback?: boolean;
  includeRawPreviews?: boolean;
};
```

## Result shape

```ts
export type EvidenceResolutionResult = {
  schema: 'ontogony-evidence-resolution-result-v1';
  schemaVersion: 1;
  root: EvidenceRoot;
  completeness: EvidenceCompleteness;
  identifiers: EvidenceIdentifierSet;
  graph: EvidenceGraph;
  sourceAttempts: EvidenceSourceAttempt[];
  missing: EvidenceMissingLink[];
  warnings: EvidenceWarning[];
  pageLinks: EvidencePageLink[];
  dataSource: EvidenceDataSource;
  exportedAt?: string;
  build?: EvidenceBuildMetadata;
};
```

## Completeness

```ts
export type EvidenceCompleteness = {
  state: 'resolved' | 'partial' | 'unresolved' | 'not_applicable';
  resolvedNodeCount: number;
  resolvedEdgeCount: number;
  requiredMissingCount: number;
  optionalMissingCount: number;
  notApplicableCount: number;
  sourceFailureCount: number;
  summary: string;
};
```

## Missing link

```ts
export type EvidenceMissingLink = {
  relationship: string;
  from?: EvidenceNodeRef;
  expectedToKind: EvidenceNodeKind;
  expectedIdentifier?: EvidenceIdentifier;
  sourceSystem: 'allagma' | 'kanon' | 'conexus' | 'platform' | 'frontend';
  applicability: 'required' | 'optional' | 'not_applicable';
  reasonCode: EvidenceMissingReasonCode;
  message: string;
  suggestedNextStep?: string;
  sourceAttemptIds?: string[];
};
```

## Graph node

```ts
export type EvidenceNode = {
  id: string;
  canonicalKey: string;
  kind: EvidenceNodeKind;
  system: EvidenceSystem;
  label: string;
  status?: string;
  authority: 'authoritative' | 'advisory' | 'derived' | 'placeholder' | 'fixture';
  dataSource: EvidenceDataSource;
  identifiers: EvidenceIdentifier[];
  aliases?: EvidenceIdentifier[];
  sourceAttemptIds: string[];
  pageLinks?: EvidencePageLink[];
  warnings?: EvidenceWarning[];
  rawPreview?: unknown;
};
```

## Edge

```ts
export type EvidenceEdge = {
  id: string;
  fromCanonicalKey: string;
  toCanonicalKey: string;
  relationship: string;
  confidence: 'direct' | 'derived' | 'weak';
  sourceSystem: EvidenceSystem;
  sourceAttemptIds?: string[];
  applicability?: 'required' | 'optional' | 'not_applicable';
  reason?: string;
};
```

## Data source

```ts
export type EvidenceDataSource =
  | 'live'
  | 'live_with_fallback'
  | 'fixture_only'
  | 'generated'
  | 'imported'
  | 'unknown';
```

## Rule

All pages that show evidence should consume this result shape or an adapter of this shape. Do not let Agent Interaction, Replay, Topology, and Evidence Spine each invent their own missing-link semantics.
