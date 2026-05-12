# Conexus.NET starter — platform lock (PR54)

This note is a **checkpoint** before work moves out of `ontogony-platform` into the Conexus.NET starter repository or archive. It does **not** add product behavior here.

## What is frozen for Conexus.NET v0

Treat the following as the **Conexus.NET v0 substrate** unless a deliberate platform release changes it:

- **Version line:** `0.3.0-alpha.1` (see [`docs/FRAMEWORK_BASELINE.md`](../FRAMEWORK_BASELINE.md)).
- **Shipping library count:** **23** `Ontogony.*` packages under `src/` (inventory + pack smoke in CI).
- **Required direct references** for the first gateway slice: as listed in [`conexus-dotnet-platform-readiness.md`](conexus-dotnet-platform-readiness.md) (Hosting, Observability, Logging, Redaction, Secrets, Quotas, Errors, Http, Security, Idempotency, Hashing, Contracts, AI.Contracts, Artifacts, Execution).
- **Optional / later:** `Ontogony.Replay.Contracts` and the other “optional later” rows in that blueprint.
- **Compile-only reference app in this repo:** [`examples/ConexusDotNetSkeleton/`](../../examples/ConexusDotNetSkeleton/) — not a product; mirrors required wiring (including tracing → logging scope → exception handling).

**Rule of thumb:** new Conexus-only code lives in the Conexus.NET repo; new **mechanical** cross-cutting primitives belong in `ontogony-platform` only when multiple services need them (not for Conexus-only convenience).

## Recorded validation (local)

The following was run on **2026-05-12** against commit **`3bf0788`** (short SHA from `git rev-parse --short HEAD` at run time).

| Step | Result |
| --- | --- |
| `dotnet restore Ontogony.Platform.sln` | Success |
| `dotnet build Ontogony.Platform.sln -c Release --no-restore` | Success (0 warnings, 0 errors) |
| `dotnet test Ontogony.Platform.sln -c Release --no-build` | All test assemblies passed (0 failed) |
| `scripts/validate-docs-links.ps1` | `OK: all relative links under docs resolve` |
| `scripts/validate-docs-api-names.ps1` | `OK: no banned stale API strings` |
| `scripts/validate-ai-runtime-boundaries.ps1` | `OK: AI-runtime substrate boundary scan passed` |
| `scripts/validate-shipping-inventory.ps1` | `OK: 23 shipping packages` + README each |
| `scripts/validate-ai-runtime-docs.ps1` | `OK: AI runtime doc files pass banned-term scan` |
| `scripts/validate-package-levels.ps1` | `OK: package-level ProjectReference validation passed` |
| `scripts/validate-pr48-pr52-overlay.ps1` | `OK: PR48-PR52 overlay packages and tests are present` |
| `scripts/pack-all.ps1 -NoBuild` with `PACKAGE_VERSION=0.3.0-alpha.1` | **23** shipping `.nupkg` listed (smoke gate) |
| `dotnet build examples/ConexusDotNetSkeleton/ConexusDotNetSkeleton.csproj -c Release` | Success |

GitHub Actions was not verified from this workspace; re-run the same commands in CI or locally before cutting a release tag.

## Next step (outside this repo)

Create or open the **Conexus.NET** starter solution: project references or `PackageReference` entries for the frozen set, then implement gateway routes, provider integration, and policy **only** in that repo.

## Related

- [`conexus-dotnet-platform-readiness.md`](conexus-dotnet-platform-readiness.md) — package roles and reference request flow.
- [`../migrations/`](../migrations/) — breaking changes (e.g. PR53 redaction/replay notes).
