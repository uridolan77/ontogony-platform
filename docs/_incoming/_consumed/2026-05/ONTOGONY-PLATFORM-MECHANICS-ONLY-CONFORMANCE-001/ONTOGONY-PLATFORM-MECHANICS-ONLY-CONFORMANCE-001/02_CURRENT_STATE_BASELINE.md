# Current state baseline

## Existing platform truth

Ontogony.Platform already has a strong mechanical spine:

- shared mechanics only;
- 27 shipping `Ontogony.*` packages;
- `.NET 9`;
- trace/correlation mechanics;
- error envelopes;
- frozen header propagation;
- system compatibility gate;
- protocol registry;
- per-package contracts;
- CI validation;
- HTTP resilience;
- conformance harnesses for outbox/idempotency/artifacts;
- observability mechanics;
- six-repo compatibility gate;
- consumer conformance;
- no-meaning guard.

## Important existing non-claims

- Platform is not production-ready as a system.
- Platform does not own semantic authority.
- Platform does not own model routing.
- Platform does not own governed execution.
- Platform does not own product semantics.
- Platform has replay contracts only, not a replay runtime.
- Runtime lock/system status authority remains outside Platform, primarily in Allagma/system evidence.

## This package extends, not replaces

This package should not rewrite the architecture. It should make existing mechanical principles more enforceable:

1. Add a proposal gate so future work cannot sneak product meaning into Platform.
2. Add product-repo conformance harnesses that produce machine-readable evidence.
3. Add versioned JSON schemas for neutral cross-service contracts.
4. Add consumer adoption checklists and required evidence rows.
