# Resolution algorithm

## Input

```ts
type EvidenceLookupInput = {
  raw: string;
  kind?: EvidenceIdentifierKind;
  includeExports?: boolean;
  includeEvents?: boolean;
  includeRelated?: boolean;
};
```

## Output

```ts
type EvidenceResolutionResult = {
  query: EvidenceLookupInput;
  graph: EvidenceGraph;
  attemptedSources: EvidenceSourceAttempt[];
  completeness: EvidenceCompleteness;
  warnings: EvidenceWarning[];
};
```

## Algorithm v1

1. Parse input into candidate ID kinds.
2. Resolve direct node:
   - run ID → Allagma run
   - eval ID → Allagma evaluation
   - model call ID → Conexus execution run/request
   - decision ID → Kanon decision
   - baseline ID → Allagma baseline comparison
3. Extract secondary IDs from direct node:
   - trace ID
   - correlation ID
   - run ID
   - decision ID
   - model call ID
   - evaluation ID
   - baseline comparison ID
4. Resolve linked nodes.
5. Load events/audit/evidence if requested.
6. Build normalized graph.
7. Build page links and export links.
8. Return completeness/missing-edge reasons.

## Cycle control

Limit recursive expansion:

```text
maxDepth: 3
maxNodes: 50
maxApiCalls: 30
```

## Confidence

Each edge gets confidence:

```text
direct       — explicit ID field in source DTO
derived      — inferred from trace/correlation lookup
weak         — first match in filtered list
unresolved   — expected edge missing
```
