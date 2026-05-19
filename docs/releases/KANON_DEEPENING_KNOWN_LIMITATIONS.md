# Kanon deepening — known limitations

Consolidated from KANON-DEEPEN-001 through 006 evidence. Not a production readiness statement.

## Semantic substrate

- **No durable canonical-fact list/history API** in v0 — `/kanon/facts` is a resolve-result workbench.
- **No durable semantic-plan list/history API** in v0 — `/kanon/plans` is a compile-result workbench.
- Do not fabricate history from browser session state.

## Domain packs

- Validate/load/promote/deprecate require **Admin** or **System**; Auditor is read-only.
- HTTP load may fail at runtime (`DomainPackHttpLoadDisabled`) without a dedicated pre-flight UI flag.
- Lifecycle history quality depends on Postgres lifecycle rows.

## Provenance

- By-trace returns a **list**; UI must not imply a single decision per trace.
- Verify and replay export depend on backend contracts; prepare-replay is manual POST.
- Cross-links to bindings/facts are route-level until deep-link filters exist.

## Cross-service links (005)

- **Not** the Cross-Service Evidence Spine package.
- `actionEvaluationDecisionId` and `domainPackId` are often absent on Allagma run GET.
- Links are best-effort from DTO/correlation fields present at render time.
- Domain pack `packId` URL selection is best-effort.

## Operator environment

- Local `local-operator` + Auditor/ProvenanceReader defaults are **Docker-local posture** only.
- Browser verification requires **rebuilt** `ontogony-frontend` image with provenance env args.
- CORS/403 must remain distinguishable (authorization vs service down).

## UI-HARDEN overlap

Shared feedback/navigation primitives from `@ontogony/ui` may appear on Kanon pages; that does not expand Kanon API scope.
