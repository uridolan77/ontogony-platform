# Decisions to Lock Before PR26–PR35

## D1 — Package granularity

Decision: keep provider packages separate.

Examples:

- `Ontogony.Persistence` remains contracts/reference.
- `Ontogony.Persistence.Postgres` owns Postgres implementation.
- `Ontogony.ProtocolIngress` owns protocol normalization, not product ingestion.

## D2 — Hosting defaults must be opt-in and composable

`UseOntogonyServiceDefaults()` should be convenient, but every underlying piece should remain separately usable.

Do not force every service into one monolithic startup style.

## D3 — No cross-repo semantic extraction during this phase

Agentor and Athanor can provide examples and conformance tests, but their domain semantics must not move into `ontogony-platform`.

## D4 — Docs and tests are first-class artifacts

Every PR must include:

- package docs or migration note
- tests proving the mechanical contract
- explicit non-goals
- example or adoption snippet when relevant

## D5 — Release workflow must support internal packages before public packages

Treat internal NuGet publishing as the near-term target. Public release can come later.

## D6 — Protocol ingress stores raw and normalized views

Protocol ingestion must preserve raw protocol payloads plus normalized envelope metadata where feasible. Do not discard raw protocol context.

## D7 — Security defaults should become stricter over time

During 0.x, keep compatibility switches. Before 1.0, prefer stricter defaults:

- no disabled auth in production
- HMAC body-bearing requests require preload middleware
- static shared-secret mode clearly discouraged
- distributed nonce replay store required for multi-node production
