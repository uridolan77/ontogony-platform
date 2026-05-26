# ONTOGONY-IMPLEMENTATION-DEPTH-OVER9-001

Goal: raise **Implementation Depth** for all four backend repos above **9/10**.

| Repo | Current implementation-depth score | Target |
| --- | ---: | ---: |
| `ontogony-platform` | 8.0 | 9.1+ |
| `conexus-dotnet` | 8.5 | 9.1+ |
| `kanon-dotnet` | 8.5 | 9.1+ |
| `allagma-dotnet` | 8.5 | 9.1+ |

This package focuses on real implemented depth: code/runtime behavior, tests, generated artifacts, and evidence. It is not a planning-only package.

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

## High-level outcome

After this package, the backend system should have:

1. deeper Platform mechanics with real conformance harnesses;
2. Conexus operational depth around provider capabilities, maintenance history, streaming posture, and routing/fallback evidence;
3. Kanon semantic authority depth around lifecycle, client coverage, domain governance, semantic graph, and Postgres semantic proof;
4. Allagma runtime depth around replay persistence, route-decision replay orchestration, topology E2E, streaming governed path, and MAF depth;
5. one closeout report proving all four repos now merit implementation-depth scores above 9.

## Merge order

1. `ontogony-platform` mechanics expansions consumed downstream.
2. `conexus-dotnet` and `kanon-dotnet` repo-local depth slices.
3. `allagma-dotnet` integration/runtime slices.
4. Cross-repo validation and score closeout.

## Non-goals

- No production-readiness claim.
- No enterprise IAM.
- No real external tool execution.
- No new product semantics in Platform.
- No Conexus provider SDKs in Allagma core.
