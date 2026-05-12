# Publishing Ontogony packages to GitHub Packages

This repository pushes packed NuGet packages to the **GitHub Packages NuGet registry** when a **version tag** is pushed. The workflow is [.github/workflows/release-packages.yml](../../../.github/workflows/release-packages.yml).

## When packages are published

- **Tag push** matching `v*.*.*` (for example `v0.3.0`) runs restore, build, tests, validation scripts, pack, manifest generation, then `dotnet nuget push` to GitHub Packages.
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

Outputs land under `artifacts/packages/`. Push manually only when you intend to override a feed (prefer tag-driven releases).

## Related docs

- [Private NuGet feed adoption](../../adoption/private-nuget-feed.md)
- [Consumer package migration](../../adoption/consumer-package-migration.md)
- [Installing released packages](../../operations/index.md#installing-released-packages)
