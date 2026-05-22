# Integration — consuming Ontogony.Platform

Platform supplies **mechanics**. Consumers own product meaning.

```text
Conexus.NET  → model gateway (routing, providers, gateway policy)
Kanon.NET    → semantic authority
Allagma.NET  → governed execution (runs, gates, eval)
```

Governance index: [`governance/README.md`](./governance/README.md). Phase 1 compatibility: [`governance/PHASE1_CONSUMER_COMPATIBILITY.md`](./governance/PHASE1_CONSUMER_COMPATIBILITY.md) (`0.3.0-alpha.1`).

---

## Consumer blueprints

| Consumer | Readiness | Package-mode contract |
| --- | --- | --- |
| Conexus.NET | [`consumer-blueprints/conexus-dotnet-platform-readiness.md`](./consumer-blueprints/conexus-dotnet-platform-readiness.md) | [`CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md`](./consumer-blueprints/CONEXUS_ONTOGONY_PACKAGE_MODE_CONTRACT.md) |
| Allagma.NET | [`consumer-blueprints/allagma-dotnet-platform-readiness.md`](./consumer-blueprints/allagma-dotnet-platform-readiness.md) | [`ALLAGMA_ONTOGONY_PACKAGE_MODE_CONTRACT.md`](./consumer-blueprints/ALLAGMA_ONTOGONY_PACKAGE_MODE_CONTRACT.md) |

Index: [`consumer-blueprints/README.md`](./consumer-blueprints/README.md).

Compile-only smokes: [`examples/ConexusDotNetSkeleton/`](../examples/ConexusDotNetSkeleton/), [`examples/AllagmaDotNetSkeleton/`](../examples/AllagmaDotNetSkeleton/).

---

## Consumption modes

### Sibling `ProjectReference` (default for active development)

Reference projects under `../ontogony-platform/src/Ontogony.*` from the consumer solution. Fast iteration; no NuGet publish required.

### NuGet package mode

Set `UseOntogonyPackages=true` and pin `OntogonyPackageVersion` to the platform line (`0.3.0-alpha.1`). Restore from a feed that does not collide with a sibling checkout in CI.

Validation:

- Conexus: `scripts/validate-conexus-consumer-baseline-alignment.ps1`
- Allagma: `scripts/validate-allagma-consumer-baseline-alignment.ps1`
- Kanon: union file [`governance/kanon-ontogony-package-union.txt`](./governance/kanon-ontogony-package-union.txt) + `validate-kanon-ontogony-package-union.ps1`

NuGet source mapping: [`governance/NUGET_SOURCE_MAPPING.md`](./governance/NUGET_SOURCE_MAPPING.md).

---

## System compatibility gate

Mechanical drift detection across the six-repo alpha system:

- Doc: [`contracts/SYSTEM_COMPATIBILITY_GATE.md`](./contracts/SYSTEM_COMPATIBILITY_GATE.md)
- Package: `Ontogony.SystemCompatibility`
- Script: `scripts/run-system-compatibility-gate.ps1`

Reads manifests and snapshots **from sibling repos** (Kanon, Conexus, Allagma, frontend). Platform does not author those semantics.

**System-wide compatibility matrix** (feature connections, runtime lock) is owned by **Allagma** and sister repos — not duplicated here.

---

## Package version line

| Field | Value |
| --- | --- |
| Current line | `0.3.0-alpha.1` |
| Shipping count | 27 packages |
| Pre-1.0 policy | Breaking changes allowed with changelog, migrations, consumer validation |

See [`CURRENT_STATE.md`](./CURRENT_STATE.md) and [`../CHANGELOG.md`](../CHANGELOG.md).

---

## Adoption guides

| Topic | Document |
| --- | --- |
| Conexus platform adoption | [`adoption/conexus-platform-adoption.md`](./adoption/conexus-platform-adoption.md) |
| Error middleware | [`adoption/error-middleware-adoption.md`](./adoption/error-middleware-adoption.md) |
| Observability + error ordering | [`adoption/observability-error-ordering.md`](./adoption/observability-error-ordering.md) |
| Hosting service defaults | [`adoption/hosting-service-defaults-adoption.md`](./adoption/hosting-service-defaults-adoption.md) |

---

## Do not

- Reference Kanon, Allagma, or Conexus **implementation** assemblies from Platform.
- Add provider SDKs, orchestration semantics, or ontology logic to Platform packages.
- Treat Platform docs as the system runtime status authority — use Allagma evidence index for runtime baseline.
