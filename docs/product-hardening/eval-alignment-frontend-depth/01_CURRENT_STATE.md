# 01 — Current State

## Closed prerequisites

```text
First Dockerized local working system      CLOSED / PASS
Post-Docker-local hardening                CLOSED / PASS
CI-COST-001                                DONE across six repos
Production readiness                       NOT STARTED
```

The system now has a stable Docker-local foundation, operator evidence, trace/correlation proof, Conexus persistence validation, Kanon topology diagnostics, frontend fixture/live/replay/config checks, UI packaging documentation, and CI cost controls.

## Product-level problem now

The infrastructure is no longer the bottleneck. The bottleneck is product depth and semantic alignment:

- eval surfaces need stronger query/list semantics
- dashboard data model needs to stop relying on sampling limitations where possible
- comparison/dataset/scoring concepts need clearer user-facing structure
- backend OpenAPI, generated clients, adapters, and UI pages need a refreshed contract matrix
- run detail should become an evidence workbench rather than a collection of panels
- replay needs a limitation-aware workbench
- evidence should align across run/eval/comparison/dataset/trace/correlation/decision IDs

## Boundary

Do not mix this product-hardening package with production readiness. Production readiness is a separate program.
