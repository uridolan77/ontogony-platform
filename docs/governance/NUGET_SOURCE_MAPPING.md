# NuGet source mapping (`nuget.config`)

Ontogony.Platform uses a **repo-root** [`nuget.config`](../../nuget.config) so restores are deterministic when developers or CI machines have extra feeds in user-level NuGet config (which otherwise triggers **NU1507** with central package management).

## Platform repository (`ontogony-platform`)

```xml
<packageSources>
  <clear />
  <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
</packageSources>
<packageSourceMapping>
  <packageSource key="nuget.org">
    <package pattern="*" />
  </packageSource>
</packageSourceMapping>
```

| Behavior | Why |
| --- | --- |
| `<clear />` | Drops inherited user/machine sources for this repo |
| Single `nuget.org` feed | All third-party packages resolve from one mapped source |
| `pattern="*"` | Every package id maps to `nuget.org` — no ambiguous multi-feed restore |

**Ontogony.* packages** in this repo are built from source (`ProjectReference`), not restored from a feed, during normal platform development.

### Local pack output

Packed `.nupkg` files land under `artifacts/packages/`. The **Conexus package smoke** uses a **project-local** config:

- [`examples/ConexusDotNetPackageSmoke/nuget.config`](../../examples/ConexusDotNetPackageSmoke/nuget.config) — folder feed at `../../artifacts/packages` with `Ontogony.*` → local and `*` → nuget.org (required when the repo root maps all packages to nuget.org only).

Do not commit credentials into any `nuget.config`.

## Consumer repositories (Conexus, Kanon, Allagma)

Consumers typically need **two** logical sources:

| Source | Packages |
| --- | --- |
| **nuget.org** (or mirror) | `Microsoft.*`, test SDKs, database drivers, etc. |
| **Internal feed** | Packed `Ontogony.*` at pinned `OntogonyPackageVersion` |

### Recommended consumer pattern

1. Keep **secrets out of git** — use `dotnet nuget add source` in CI or environment variables (see [Package publishing (GitHub Packages)](../adoption/private-nuget-feed.md)).
2. Use **package source mapping** when multiple feeds are enabled (same NU1507 avoidance as platform).
3. Example mapping shape (adjust keys/URLs to your feed):

```xml
<packageSources>
  <clear />
  <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
  <add key="ontogony-github" value="https://nuget.pkg.github.com/OWNER/index.json" />
</packageSources>
<packageSourceMapping>
  <packageSource key="nuget.org">
    <package pattern="Microsoft.*" />
    <package pattern="System.*" />
    <package pattern="Npgsql" />
    <!-- other third-party ids -->
  </packageSource>
  <packageSource key="ontogony-github">
    <package pattern="Ontogony.*" />
  </packageSource>
</packageSourceMapping>
```

4. **Package-mode CI:** Conexus guards against a sibling `../ontogony-platform` checkout when `UseOntogonyPackages=true` — pack Ontogony to a non-colliding path or use a published feed ([`CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md`](../consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md)).

### Folder feed (local iteration)

After `./scripts/pack-all.ps1`:

```powershell
dotnet nuget add source (Resolve-Path ./artifacts/packages) -Name ontogony-local
```

Or use `examples/ConexusDotNetPackageSmoke/nuget.config` as a template for folder-feed + nuget.org mapping.

## Related

- [`docs/adoption/private-nuget-feed.md`](../adoption/private-nuget-feed.md)
- [`docs/adoption/consumer-package-migration.md`](../adoption/consumer-package-migration.md)
- [`.github/workflows/samples/consume-internal-feed.yml`](../../.github/workflows/samples/consume-internal-feed.yml)
