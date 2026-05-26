# Agent instructions

# Context

This package targets the four backend repositories:

- `ontogony-platform` — shared mechanics only.
- `conexus-dotnet` — model gateway / model-access authority.
- `kanon-dotnet` — semantic authority / ontology / policy / provenance.
- `allagma-dotnet` — governed execution runtime and system lock owner.

Boundary rule:

```text
Ontogony.Platform owns mechanics.
Conexus owns model access and routing.
Kanon owns meaning, policy, provenance, and semantic authority.
Allagma owns governed execution, run lifecycle, replay orchestration, and runtime lock.
```

Do not move semantics into Platform, model routing into Allagma, execution orchestration into Kanon, or provider SDKs into Allagma core. Do not enable real external tool execution in this package.

Work repo-by-repo. Do not do broad rewrites. For every slice:

1. inspect current code first;
2. implement the smallest coherent vertical slice;
3. update tests;
4. update generated inventories if applicable;
5. write evidence under `docs/evidence/`;
6. update current-state/known-limitations only when the truth changed;
7. avoid stale roadmap resurrection.

When a slice says “blocked”, implement the block verification and evidence. Do not implement the blocked capability.
