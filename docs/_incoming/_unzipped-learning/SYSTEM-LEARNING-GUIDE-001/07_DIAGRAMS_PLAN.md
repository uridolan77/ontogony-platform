# 07 — Diagrams plan

Include Mermaid diagrams in the target guides:

## Repo responsibility map

```mermaid
graph LR
  Platform[Ontogony Platform] --> Frontend[ontogony-frontend]
  UI[ontogony-ui] --> Frontend
  Allagma --> Kanon
  Allagma --> Conexus
  Kanon --> Conexus
```

## Governed fake E2E sequence

```mermaid
sequenceDiagram
  participant User
  participant Frontend
  participant Allagma
  participant Kanon
  participant Conexus
  User->>Frontend: Start governed fake flow
  Frontend->>Allagma: create run
  Allagma->>Kanon: compile/evaluate
  Allagma->>Conexus: model completion via fake provider
  Allagma-->>Frontend: events/evidence identifiers
```

## Evidence Spine resolution flow

```mermaid
flowchart TD
  A[Raw identifier] --> B[Parse kind/candidates]
  B --> C[Try owning API resolvers]
  C --> D[Build nodes/edges]
  D --> E[Record source attempts]
  E --> F[Return graph + completeness]
```

## Domain/model/provider boundary

```mermaid
flowchart LR
  Domain[Domain / ontology context] --> Kanon[Kanon semantic plan]
  Purpose[Allagma model purpose] --> Alias[Conexus alias]
  Alias --> Route[Provider route]
  Route --> Provider[Provider model/key]
```

## Contract discipline ladder

```mermaid
flowchart TD
  Backend[Backend route] --> Inventory[Route inventory]
  Inventory --> OpenAPI[OpenAPI snapshot]
  OpenAPI --> TS[Generated TS schema/client]
  TS --> Catalog[route-workflow catalog]
  Catalog --> UI[Frontend page/tests]
```
