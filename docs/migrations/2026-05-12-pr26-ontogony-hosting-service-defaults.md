# PR26 — Ontogony.Hosting service defaults

## Summary

Introduces `Ontogony.Hosting`, a new infrastructure package that composes shared ASP.NET host mechanics for:

- observability registration
- error middleware registration
- consistent middleware ordering
- optional service-identity body-hash preload middleware
- health/readiness endpoint mapping
- host-overridable JSON defaults

## Consumer actions

### New package

- Add a package/project reference to `Ontogony.Hosting`.
- Use `AddOntogonyServiceDefaults(...)` during service registration.
- Use `UseOntogonyServiceDefaults()` and `MapOntogonyHealthEndpoints()` in startup.

### Health endpoints

- `/health` and `/ready` are mapped by default when `MapOntogonyHealthEndpoints()` is called.
- Override paths through `OntogonyServiceDefaultsOptions` when needed.

### Security preload

- `UseServiceIdentityBodyHashPreload` remains opt-in (`false` by default).
- Enabling preload does not configure service identity verification itself; services still own security policy wiring.

## Repos unchanged

No Athanor, Agentor, or Conexus domain behaviors were introduced in this PR.
