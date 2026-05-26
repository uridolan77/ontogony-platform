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

This package is contract discipline, not feature development. Prefer generated truth and tests over prose.

For every contract change:

1. identify the source of truth;
2. generate derived artifacts from the source of truth;
3. add a drift test;
4. update docs only through generated fragments where possible;
5. update package/runtime lock only after validation;
6. write evidence.

Do not hand-edit generated inventories except through the documented generator path.
