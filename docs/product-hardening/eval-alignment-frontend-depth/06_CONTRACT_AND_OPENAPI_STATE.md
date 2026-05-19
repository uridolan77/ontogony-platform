# 06 — Contract and OpenAPI State

## Contract-first rule

Any new product backend capability must land in this order:

```text
Route/DTO semantics → OpenAPI snapshot → generated client → product wrapper/hook → adapter contract → UI → tests/evidence
```

## OpenAPI surfaces to audit

| Service | Expected relevance |
|---|---|
| Allagma | eval routes, baseline comparison, run events, replay evidence, topology summary |
| Kanon | decision records, by-trace lookup, ontology/provenance/topology evidence |
| Conexus | model-call evidence, route decisions, journal metadata, provider/alias bootstrap status |

## Missing route policy

If a product capability is desired but no backend route exists, the PR must choose one:

- add backend route and OpenAPI snapshot
- explicitly mark as missing capability and expose limitation state
- defer the capability and remove UI affordance

No hidden placeholder success states.
