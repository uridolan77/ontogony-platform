# Graph Canonicalization Contract

## Problem

The current graph can show a real resolved node and a semantic-graph placeholder for the same entity. Example class of issue:

```text
Allagma run run_fcde...       allagma.run · allagma · Completed
Allagma run run_fcde...       allagma.run · allagma · placeholder
```

This makes the graph look larger and less reliable than it is.

## Canonical key

Every node must have a deterministic canonical key:

```text
{system}:{nodeKind}:{canonicalIdentifier}
```

Examples:

```text
allagma:run:run_fcde6bdd7fe541a99dfcb6cc40e16069
kanon:decision:decision_f1c60ae581bc4f5496188e05ce9b1cc7
conexus:modelCall:chatcmpl-0HNLMJJQFVG3N-00000003
conexus:request:0HNLMJJQFVG3N-00000003
conexus:executionRun:chat-0HNLMJJQFVG3N-00000003
conexus:routeDecision:rd-0HNLMJJQFVG3N-00000003
platform:trace:52ee25d4f7a25c2562d3ef6264175ef8
platform:correlation:1e6590f246af4c6497d6c3e73b8c908b
```

## Merge algorithm

Pseudo-code:

```ts
function mergeGraphNodes(nodes: EvidenceNode[]): EvidenceNode[] {
  const byKey = new Map<string, EvidenceNode>();

  for (const node of nodes) {
    const key = computeCanonicalKey(node);
    const existing = byKey.get(key);
    if (!existing) {
      byKey.set(key, { ...node, canonicalKey: key });
      continue;
    }

    byKey.set(key, mergeNode(existing, { ...node, canonicalKey: key }));
  }

  return [...byKey.values()];
}
```

Merge precedence:

```text
live_api > semantic_graph > evidence_export > derived > placeholder > fixture
```

Merge fields:

- `label`: prefer most specific live API label;
- `status`: preserve raw statuses, display highest-authority status;
- `authority`: choose strongest authority;
- `identifiers`: union;
- `aliases`: union;
- `sourceAttemptIds`: union;
- `pageLinks`: union and dedupe;
- `warnings`: union;
- `rawPreview`: prefer live API, keep secondary previews under `mergedPreviews` if needed.

## Edge rewiring

After node merge, rewrite edges to canonical keys and dedupe edges by:

```text
fromCanonicalKey + relationship + toCanonicalKey + sourceSystem
```

Do not dedupe if two edges represent meaningfully different evidence paths; instead preserve multiple `sourceAttemptIds` on a single edge.

## Tests

Required tests:

1. placeholder + live Allagma run merge;
2. placeholder + live Kanon decision merge;
3. route-decision edge rewired after merge;
4. duplicate page links deduped;
5. source attempts preserved.
