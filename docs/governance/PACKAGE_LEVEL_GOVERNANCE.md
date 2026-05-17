# Package-level governance (Ontogony.Platform)

**Status:** Active (PLATFORM-GOV-002)  
**Package line:** `0.3.0-alpha.1` and later until the golden map changes

Mechanical rules for **which Ontogony library may reference which** under `src/`. This is infrastructure layering — not product semantics (Kanon meaning, Allagma orchestration, Conexus routing).

## Authoritative sources

| Artifact | Path | Role |
| --- | --- | --- |
| Golden map (CI) | [`scripts/validate-package-levels.ps1`](../../scripts/validate-package-levels.ps1) | Machine-enforced allowed `ProjectReference` sets per `PackageId` |
| Human matrix | [`docs/architecture/package-levels.md`](../architecture/package-levels.md) | Levels, forbidden edges, dependency table |
| Pre-tag checklist | [`PACKAGE_COMPATIBILITY_CHECKLIST_0.3.0-alpha.1.md`](./PACKAGE_COMPATIBILITY_CHECKLIST_0.3.0-alpha.1.md) | Section A — inventory + package-level pass |

CI runs `./scripts/validate-package-levels.ps1` in [`.github/workflows/ci.yml`](../../.github/workflows/ci.yml) and [`.github/workflows/release-packages.yml`](../../.github/workflows/release-packages.yml).

## When you change a package edge

1. Edit the `ProjectReference` in the consumer `.csproj` under `src/`.
2. Update the **golden map** in `validate-package-levels.ps1` (`$expectedRefs`).
3. Update the **matrix table** in `docs/architecture/package-levels.md` (and level narrative if the grouping changes).
4. Run locally:

   ```powershell
   ./scripts/validate-package-levels.ps1
   ```

5. If the change affects shipped public API, follow [`PUBLIC_API_COMPATIBILITY.md`](../planning/robustness/PUBLIC_API_COMPATIBILITY.md) and refresh snapshots as required.

## Forbidden edges (always)

These are enforced in addition to the golden map:

| Rule | Enforcement |
| --- | --- |
| `Ontogony.Execution` → `Ontogony.Artifacts` | `$forbiddenEdges` in `validate-package-levels.ps1` |
| Shipping library → `Ontogony.Testing` | Script scan (Testing is test-only aggregate) |
| PackageId in `src/` but missing from golden map | Script fails with add-to-map instructions |
| Golden map entry with no `src/*.csproj` | Script fails (orphan map entry) |

## Intentional cross-level edges

Documentation **levels** (0, 0.5, 1–3) are a mental model, not a strict topological sort. The matrix is authoritative.

**Example (PLATFORM-GOV-001A):** `Ontogony.Security` → `Ontogony.Http` for outbound actor/tenant propagation (`CurrentActorOutboundPropagator`). That edge is recorded in the golden map and matrix; it does **not** move routing or provider policy into Platform.

## Consumer guidance

Phase 1 consumers (Conexus, Kanon, Allagma) should treat [`package-levels.md`](../architecture/package-levels.md) as the allowed **platform-internal** graph. Consumer repos have their own forbidden-dependency tests; they must not add illegal edges by referencing implementation assemblies across product boundaries.

See [`PHASE1_CONSUMER_COMPATIBILITY.md`](./PHASE1_CONSUMER_COMPATIBILITY.md) for how each consumer consumes Platform packages.

## Validation evidence

Local closure for package-level governance is recorded in [`docs/reviews/PLATFORM_GOV_001_VALIDATION_REPORT.md`](../reviews/PLATFORM_GOV_001_VALIDATION_REPORT.md) (PLATFORM-GOV-002 section).
