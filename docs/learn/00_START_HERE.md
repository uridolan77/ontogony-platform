# Start here

> **Audience:** new developer  
> **Applies to:** full Ontogony workspace  
> **Source of truth:** [INDEX.md](./INDEX.md), sibling repo `AGENTS.md` files, [`../ARCHITECTURE.md`](../ARCHITECTURE.md)  
> **Last verified:** 2026-05-25

## What Ontogony is (one paragraph)

Ontogony is a **governed AI operations stack**: **Kanon** decides what is semantically allowed, **Allagma** runs governed agent workflows and records evidence, **Conexus** routes model calls to providers, and **Ontogony.Platform** supplies shared mechanics (trace, errors, hashing, idempotency). The **operator console** (`ontogony-frontend` + `ontogony-ui`) is how humans inspect runs, evidence, and configuration.

## The boundary rule (memorize this)

```text
Kanon owns meaning.
Allagma owns governed execution.
Conexus owns model access.
Ontogony.Platform owns mechanics.
```

Do not put product semantics in `ontogony-platform`. Do not put provider SDKs in Kanon or Allagma core.

## Workspace layout

| Repo | You go here when… |
| --- | --- |
| `ontogony-platform` | shared packages, cross-service contracts, Docker local system, **this learning index** |
| `kanon-dotnet` | ontology, domain packs, decisions, semantic query, action policy |
| `conexus-dotnet` | model aliases, providers, routing, gateway API |
| `allagma-dotnet` | runs, events, human gates, replay, MAF adapter, evaluations |
| `ontogony-frontend` | operator console, generated OpenAPI clients, Evidence Spine UI |
| `ontogony-ui` | reusable AppShell and operator primitives |

Typical sibling checkout root: `C:\dev\` with all six repos.

## First hour checklist

1. Read [01_ARCHITECTURE_MAP.md](./01_ARCHITECTURE_MAP.md) (10 min).
2. Start the stack: [02_RUN_LOCAL_SYSTEM.md](./02_RUN_LOCAL_SYSTEM.md) (20 min).
3. Run proof smoke: [03_GOVERNED_FAKE_E2E.md](./03_GOVERNED_FAKE_E2E.md) (15 min).
4. Skim [GLOSSARY.md](./GLOSSARY.md) (5 min).

## Where truth lives (avoid parallel docs)

| Need | Canonical location |
| --- | --- |
| How to run locally | [02_RUN_LOCAL_SYSTEM.md](./02_RUN_LOCAL_SYSTEM.md) → platform Docker README |
| API contracts | Owning repo `docs/api/*` snapshots + OpenAPI sync (see [08](./08_CONTRACT_DISCIPLINE.md)) |
| Generated inventories | `docs/generated/*` in each repo — **link, do not copy** |
| Runtime reproducibility | `allagma-dotnet/docs/system/ontogony-runtime.lock.json` |
| Stale / historical docs | Marked in [DOCS_AUDIT_MATRIX.md](./DOCS_AUDIT_MATRIX.md) |

## Production readiness

Closed Docker-local and operator milestones are **development proofs**, not production IAM or multi-tenant readiness. See [`../KNOWN_LIMITATIONS.md`](../KNOWN_LIMITATIONS.md).

## Next

- Operators → [04_SYSTEM_TRUTH_AND_RELEASE_READINESS.md](./04_SYSTEM_TRUTH_AND_RELEASE_READINESS.md)
- Backend → [07_DOMAIN_MODEL_ROUTING_BOUNDARIES.md](./07_DOMAIN_MODEL_ROUTING_BOUNDARIES.md)
- Frontend → [15_UI_CANONICALIZATION_AND_CONSOLE_UX.md](./15_UI_CANONICALIZATION_AND_CONSOLE_UX.md)
