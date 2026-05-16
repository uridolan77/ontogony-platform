# 2026-05-16 — PLAT-QUALITY-001 (public API docs policy + coverage HTML)

## Consumer impact

**None** for NuGet package APIs or runtime behavior.

## Operator / maintainer impact

- CI publishes an additional artifact **`coverage-report-html`** (ReportGenerator output beside existing Cobertura/TRX under **`xplat-coverage`**).
- Staged **`CS1591`** rules are written down in [`docs/quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md`](../quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md) (no new compiler enforcement beyond the existing Conexus baseline list in `src/Directory.Build.targets`).

## References

- Policy: [`docs/quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md`](../quality/PLAT-QUALITY-001-public-api-docs-and-coverage.md)
