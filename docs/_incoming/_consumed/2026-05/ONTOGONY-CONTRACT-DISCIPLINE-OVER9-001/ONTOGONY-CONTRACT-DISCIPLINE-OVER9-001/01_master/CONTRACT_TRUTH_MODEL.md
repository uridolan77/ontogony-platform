# Contract truth model

## Truth hierarchy

1. Source code route mappings / contracts.
2. Generated route inventory / client coverage / OpenAPI snapshot.
3. Compatibility manifest.
4. Runtime lock / system compatibility matrix.
5. Narrative docs.

Narrative docs must never be treated as more authoritative than generated artifacts.

## Required generated artifacts

| Repo | Required artifacts |
| --- | --- |
| `ontogony-platform` | package inventory, system compatibility gate output, protocol registry, header/error contract docs |
| `conexus-dotnet` | OpenAPI snapshots, admin/gateway DTO snapshots, compatibility manifest, provider capability matrix |
| `kanon-dotnet` | route inventory, client coverage, OpenAPI baseline, compatibility manifest, auth matrix |
| `allagma-dotnet` | route inventory, OpenAPI snapshot, runtime lock, feature connection matrix, post-lock delta register |

## Stale literal prevention

Do not allow hard-coded route counts in narrative docs unless they are generated fragments or checked by tests.
