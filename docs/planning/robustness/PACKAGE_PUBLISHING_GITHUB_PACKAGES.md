# Publishing Ontogony packages to GitHub Packages

This repository pushes packed NuGet packages to the **GitHub Packages NuGet registry** when a **version tag** is pushed. The workflow is [.github/workflows/release-packages.yml](../../../.github/workflows/release-packages.yml).

## When packages are published

- **Tag push** matching `v*.*.*` (for example `v0.3.0`) runs restore, build, tests, the same documentation and package-level validation scripts as `ci.yml` (including shipping inventory and AI runtime docs), strict changelog validation, pack, manifest generation, the Conexus package consumer smoke, then `dotnet nuget push` of shipping `.nupkg` files to GitHub Packages.
- **Symbol packages** (`.snupkg`): the workflow attempts to push each file to the same feed; if the registry rejects symbol uploads, the step logs a warning and **continues** so shipping `.nupkg` publish and the GitHub Release are not blocked. Symbols remain in uploaded artifacts and release attachments.
- **Manual `workflow_dispatch`** runs the same build and validation steps and uploads artifacts, but **does not** push to the feed (only tagged releases publish, to avoid accidental overwrites).

## Feed URL

Replace `OWNER` with the GitHub user or organization that owns this repository (in Actions this is `github.repository_owner`):

```text
https://nuget.pkg.github.com/OWNER/index.json
```

## Workflow permissions

The release job uses:

- `contents: read` — checkout and metadata.
- `packages: write` — push packages to GitHub Packages (`GITHUB_TOKEN`).

No extra secrets are required for publish from the same repository.

## Consuming the feed

### Authenticated `dotnet` restore (CI or developer machine)

Create a token with at least **`read:packages`** (and **`repo`** if the packages live in a private repository). Use it as the NuGet password; **username** must be a GitHub account that has access to the packages (often your GitHub username or the organization name).

```bash
dotnet nuget add source \
  --name ontogony-github \
  --username YOUR_GITHUB_USERNAME \
  --password YOUR_GITHUB_TOKEN_WITH_READ_PACKAGES \
  --store-password-in-clear-text \
  "https://nuget.pkg.github.com/OWNER/index.json"
```

In GitHub Actions in **another** repository, store the PAT as a secret (for example `NUGET_AUTH_TOKEN`) and the feed URL as `NUGET_FEED_URL`; see [.github/workflows/samples/consume-internal-feed.yml](../../../.github/workflows/samples/consume-internal-feed.yml).

### `nuget.config` in a consumer repo (no secrets in source control)

Keep credentials out of git: use environment variables or CI to inject the password, or use `dotnet nuget add source` in the pipeline as in the sample workflow.

## Local pack without publishing

From the repo root (see also [private-nuget-feed.md](../../adoption/private-nuget-feed.md)):

```powershell
$env:PACKAGE_VERSION = "0.3.0-local.1"
./scripts/pack-all.ps1
```

Outputs land under `artifacts/packages/`. Push manually only when you intend to override a feed (prefer tag-driven releases). `generate-package-manifest.ps1` may also write `PACKAGE_MANIFEST.json` at the repo root for local inspection; that file is gitignored and should not be committed.

## First tag publish (operational proof)

Automation alone does not prove the feed until a tag has completed successfully. After pushing a version tag (for example `v0.3.0-alpha.2`):

1. Open the **Actions** run for `release-packages` and confirm all steps are green, including **Publish to GitHub Packages**.
2. In GitHub **Packages** (or `https://github.com/OWNER?tab=packages`), confirm the expected `Ontogony.*` versions appear on the NuGet feed `https://nuget.pkg.github.com/OWNER/index.json`.
3. Compare `PACKAGE_MANIFEST.json` from the run artifacts (or the Release) to the published `.nupkg` list and checksums.
4. Confirm the **GitHub Release** for that tag lists `.nupkg`, `.snupkg` (if present), and `PACKAGE_MANIFEST.json`.

## Related docs

- [Private NuGet feed adoption](../../adoption/private-nuget-feed.md)
- [Consumer package migration](../../adoption/consumer-package-migration.md)
- [Installing released packages](../../operations/index.md#installing-released-packages)
