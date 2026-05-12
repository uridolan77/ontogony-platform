# Local layout and CI when consuming `ontogony-platform` via project reference

Some consumer repos (Athanor, Agentor) use **sibling project references** to `ontogony-platform` during pilot adoption. That keeps iteration fast but is **not** a long-term release strategy.

## Recommended local directory layout

```text
{workspace}/
  ontogony-platform/
  athanor/
  agentor/
```

From `athanor/backend/src/Athanor.Application` the relative path to packages is typically `..\..\..\..\ontogony-platform\src\...`. From `agentor/src/Agentor.Infrastructure` use `..\..\..\ontogony-platform\src\...`. Adjust if your clone depth differs.

## CI options

1. **Multi-checkout** — In GitHub Actions (or equivalent), check out `ontogony-platform` and the consumer into sibling directories before `dotnet build`.
2. **Pack + feed** — Build and pack `ontogony-platform` in CI, push artifacts to an internal feed, and reference **versioned NuGet packages** from consumers (preferred before wide adoption).

See [private-nuget-feed.md](./private-nuget-feed.md), the adoption index [consumer-package-migration.md](./consumer-package-migration.md), and the repo script `scripts/pack-all.ps1`.

### Reference workflow templates (copy to consumer repos)

These samples use **`workflow_dispatch` only** so they do not run as automatic gates on this repository. Copy and adjust repository names, paths, and secrets.

- [`.github/workflows/samples/multi-checkout.yml`](../../.github/workflows/samples/multi-checkout.yml) — sibling checkout of `ontogony-platform` + consumer for `ProjectReference` builds.
- [`.github/workflows/samples/consume-internal-feed.yml`](../../.github/workflows/samples/consume-internal-feed.yml) — authenticated restore from an internal feed (`NUGET_AUTH_TOKEN` + feed URL secret).

## Guard scripts

Consumer repos may ship a small script that fails fast if the sibling path is missing (optional). The platform does not enforce layout; teams should document it in each consumer’s engineering docs.
