# Consumer package migration (index)

Use this page as the **single entry** for moving a service from `ProjectReference` to **versioned Ontogony NuGet packages** (private feed or public when available).

## Steps

1. **Choose a restore source** — internal Azure Artifacts / GitHub Packages / file feed. See [private-nuget-feed.md](./private-nuget-feed.md).
2. **Pin versions** — align `PackageReference` versions with your platform adoption PR; see [package-versioning.md](./package-versioning.md).
3. **Remove sibling `ProjectReference` entries** — replace each `..\ontogony-platform\src\...` reference with a `PackageReference` to the same `PackageId`.
4. **CI restore** — configure `NUGET_AUTH_TOKEN` (or feed-specific credential) for restore; optional cache of `~/.nuget/packages`. Copy a template from [local-repo-layout-and-ci.md](./local-repo-layout-and-ci.md) ([multi-checkout sample](../../.github/workflows/samples/multi-checkout.yml), [internal feed sample](../../.github/workflows/samples/consume-internal-feed.yml)).
5. **Verify** — `dotnet restore`, `dotnet test`, and smoke the integration surfaces you adopted (tracing, errors, HTTP clients, hashing).

## Layout vs feed

| Approach | When |
|----------|------|
| Sibling checkout + `ProjectReference` | Fast iteration; pilot PRs. |
| Packed packages + internal feed | CI and production-like builds; preferred before wide rollout. |

Details: [local-repo-layout-and-ci.md](./local-repo-layout-and-ci.md).

## Related docs

- [athanor-platform-adoption.md](./athanor-platform-adoption.md) — phased package order for Athanor.
- [agentor-observability-adoption.md](./agentor-observability-adoption.md) — Agentor HTTP/tracing/errors.
- [observability-error-ordering.md](./observability-error-ordering.md) — middleware ordering.

## Hashing note (Athanor)

Stage-1 adoption delegates SHA-256 to `Ontogony.Hashing` while keeping Athanor canonical JSON until JSON parity is proven. See `docs/migrations/2026-05-11-pr21-athanor-hashing-stage1.md` in this repository.
