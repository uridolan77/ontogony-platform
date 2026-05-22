# Allagma.NET starter — platform lock (PLAT-ALLAGMA-001)

This note is a **checkpoint** before work moves out of `ontogony-platform` into the Allagma.NET starter repository. It does **not** add product behavior here.

## What is frozen for Allagma.NET v0

Treat the following as the **Allagma.NET v0 substrate** unless a deliberate platform release changes it:

- **Version line:** `0.3.0-alpha.1` (see [`CURRENT_STATE.md`](../CURRENT_STATE.md)).
- **Shipping library count:** **23** `Ontogony.*` packages under `src/` (inventory + pack smoke in CI).
- **Required direct references** for the first execution-host slice: as listed in [`allagma-dotnet-platform-readiness.md`](allagma-dotnet-platform-readiness.md).
- **Compile-only reference app in this repo:** [`examples/AllagmaDotNetSkeleton/`](../../examples/AllagmaDotNetSkeleton/) — not a product; mirrors required wiring (tracing → logging scope → exception handling; integration metrics; idempotency; execution journal; architecture-test compile smoke).

**Rule of thumb:** new Allagma-only code lives in the Allagma.NET repo; new **mechanical** cross-cutting primitives belong in `ontogony-platform` only when multiple services need them.

## Recorded validation (local)

Run before cutting a platform tag that Allagma will consume:

| Step | Command |
| --- | --- |
| Restore / build / test | `dotnet restore Ontogony.Platform.sln`; `dotnet build Ontogony.Platform.sln --no-restore`; `dotnet test Ontogony.Platform.sln --no-build` |
| Allagma consumer alignment | `./scripts/validate-allagma-consumer-baseline-alignment.ps1` |
| Doc validators | `./scripts/validate-docs-links.ps1`; `./scripts/validate-docs-api-names.ps1` |
| Skeleton Release build | `dotnet build examples/AllagmaDotNetSkeleton/AllagmaDotNetSkeleton.csproj -c Release` |

GitHub Actions should run the same validators as Conexus (see `.github/workflows/ci.yml`).

## Next step (outside this repo)

Create or open the **Allagma.NET** starter solution: project references or `PackageReference` entries for the frozen set, then implement governed execution, Kanon/Conexus HTTP clients, and policies **only** in that repo.

## Related

- [`allagma-dotnet-platform-readiness.md`](allagma-dotnet-platform-readiness.md) — package roles and reference integration flow.
- [`ALLAGMA_ONTOGONY_PACKAGE_MODE_CONTRACT.md`](ALLAGMA_ONTOGONY_PACKAGE_MODE_CONTRACT.md) — package-mode inputs and future CI proof.
- Kanon boundary docs (external): `kanon-dotnet` → `docs/integrations/ALLAGMA_BOUNDARY.md`.
