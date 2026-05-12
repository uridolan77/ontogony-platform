# Public API approval tests

## Goal

Prevent accidental API churn.

## Acceptance criteria

API baselines checked in CI.

## Implementation (this repo)

- Project: [`tests/Ontogony.PublicApi.Tests`](../../../../tests/Ontogony.PublicApi.Tests) — Verify + PublicApiGenerator snapshots per shipping assembly (`*.verified.txt`).
- Documentation: [`PUBLIC_API_COMPATIBILITY.md`](../PUBLIC_API_COMPATIBILITY.md) — mechanism, snapshot refresh, **breaking-change checklist** (changelog + migrations, not snapshots alone).

## Boundary checklist

- [ ] Reusable platform mechanics only.
- [ ] No Conexus routing/provider/model semantics.
- [ ] CI/build/package validation updated where relevant.
