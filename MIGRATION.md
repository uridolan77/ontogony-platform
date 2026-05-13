# Migration Guidance

Ontogony.Platform is currently in alpha (`0.3.0-alpha.1`). Public APIs are becoming more stable, but breaking changes can still happen while packages are pre-1.0.

## Current Policy

- `CHANGELOG.md` is the primary source of truth for released and unreleased changes.
- Breaking or consumer-visible contract changes should also be reflected in the public API review process documented in `docs/public-api-review.md`.
- For alpha releases, migration guidance may be lightweight when the affected surface is small, but it must still be explicit.

## How Breaking Changes Are Recorded

Use this order of precedence:

1. Add the change to `CHANGELOG.md` under `Unreleased`.
2. If the change is breaking or requires consumer action, add or update a document under `docs/migrations/`.
3. If the change also updates public API snapshots, follow `docs/public-api-review.md` and ensure the snapshot diff is reviewed intentionally.

## Future Convention

As Ontogony approaches a stable release line, migration notes should follow a per-version convention such as:

- `docs/migrations/MIGRATION_0.3_to_0.4.md`
- `docs/migrations/MIGRATION_1.0_to_1.1.md`

Each migration note should include:

- affected package(s)
- the old behavior or API
- the new behavior or API
- concrete consumer update steps
- whether the change is additive, behavioral, or breaking

## Related Documents

- `CHANGELOG.md`
- `docs/public-api-review.md`
- `docs/migrations/`
