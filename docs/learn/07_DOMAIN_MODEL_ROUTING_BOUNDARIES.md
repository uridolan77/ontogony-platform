# Domain, model purpose, routing boundaries

> **Audience:** backend developer, cross-repo integrator  
> **Applies to:** Kanon, Conexus, Allagma, domain packs  
> **Source of truth:** Kanon `docs/SEMANTIC_AUTHORITY_BOUNDARY.md`, Conexus route docs, Allagma run lifecycle  
> **Last verified:** 2026-05-25

## Four concepts (often confused)

| Concept | Owner | What it is | Example |
| --- | --- | --- | --- |
| **Domain / ontology** | Kanon | Published semantic world model | `gaming-core@0.1.0` |
| **Model purpose** | Allagma (+ Kanon policy context) | Why a model is invoked in a run | `summarize-player-risk` on start-run request |
| **Conexus model alias** | Conexus | Stable product name → provider model | `risk-summary-v0` → `gpt-4o-mini` via fake provider |
| **Provider route** | Conexus | Resolved provider + deployment + fallback | OpenAI-compatible fake provider in dev bootstrap |

```text
Domain pack answers: "What entities and policies exist?"
Model purpose answers: "Why are we calling a model in this run step?"
Alias answers: "Which logical model name does the gateway expose?"
Route answers: "Which provider endpoint actually executes?"
```

## Kanon (meaning)

- Domain packs: validate → promote → **load** (`accepted` ≠ `active`)
- Decisions and provenance are immutable authority for replay
- No live LLM calls in Kanon

Start: `kanon-dotnet/docs/CURRENT_STATE.md`, `docs/integrations/`

## Conexus (model access)

- Project API keys, provider registry, aliases, routing strategy
- OpenAI-compatible surface for callers
- Fake provider for local proof ([03_GOVERNED_FAKE_E2E.md](./03_GOVERNED_FAKE_E2E.md))

Start: `conexus-dotnet/docs/CURRENT_STATE.md`

## Allagma (governed execution)

- Binds ontology version + actor + **model purpose** on run start
- Calls Conexus with alias; records `modelCallId`, events, audit bundle
- Does not choose provider routes directly

Start: `allagma-dotnet/docs/RUN_LIFECYCLE.md`

## Console (operator view)

- **Domain Switcher** — active domain pack / ontology context
- Run detail — shows purpose, alias, decision ids
- Conexus observability — aliases, routes, usage

## Anti-patterns

| Do not… | Because… |
| --- | --- |
| Put routing policy in Kanon | Belongs in Conexus |
| Put ontology rules in Conexus | Belongs in Kanon |
| Call OpenAI SDK from Allagma core | Use Conexus HTTP client |
| Treat vector/LLM output as canonical facts | Kanon provenance required |

## Next

- Add domain: [09_ADD_A_DOMAIN.md](./09_ADD_A_DOMAIN.md)
- Add alias: [10_ADD_A_PROVIDER_OR_MODEL_ALIAS.md](./10_ADD_A_PROVIDER_OR_MODEL_ALIAS.md)
- Glossary: [GLOSSARY.md](./GLOSSARY.md)
