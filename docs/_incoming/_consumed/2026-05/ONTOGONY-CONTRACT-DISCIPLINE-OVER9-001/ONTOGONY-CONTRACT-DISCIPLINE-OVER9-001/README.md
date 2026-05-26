# ONTOGONY-CONTRACT-DISCIPLINE-OVER9-001

Goal: raise **Contract Discipline** for all four backend repos above **9/10**.

| Repo | Current contract-discipline score | Target |
| --- | ---: | ---: |
| `ontogony-platform` | 8.5 | 9.1+ |
| `conexus-dotnet` | 8.5 | 9.1+ |
| `kanon-dotnet` | 8.0 | 9.1+ |
| `allagma-dotnet` | 8.5 | 9.1+ |

This package is about truth, compatibility, generated artifacts, route/API inventories, package lines, error/header contracts, and breaking-change gates.

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

## Core problem to solve

The backend system has strong contract machinery, but moving repos still show drift risk:

- runtime lock is behind moving `main`;
- post-lock delta register carries substantial unpromoted movement;
- Kanon route/client/server-only counts disagree across narrative docs vs generated manifest;
- service-specific error shapes are valid but need sharper machine-readable contract classification;
- package-mode and OpenAPI/generation discipline must become uniformly enforced.

## High-level outcome

After this package:

1. generated manifests become the single source of truth;
2. narrative docs cannot silently drift from generated route/client counts;
3. OpenAPI, route inventory, client coverage, package versions, error contracts, and header propagation are validated across repos;
4. Allagma runtime lock can be promoted cleanly;
5. every backend repo has a contract closeout report justifying a score above 9.
