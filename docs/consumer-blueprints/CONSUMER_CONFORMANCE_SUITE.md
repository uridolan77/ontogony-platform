# Consumer conformance suite (PLAT-9-003)

**Status (2026-05-26):** **Done** for platform scope — `scripts/run-consumer-conformance.ps1`, `ConsumerConformanceGate`, and `*PlatformConformanceTests` on Allagma/Kanon/Conexus. Evidence: [`../evidence/PLATFORM_RECONSTRUCTABILITY_CONFORMANCE_EVIDENCE.md`](../evidence/PLATFORM_RECONSTRUCTABILITY_CONFORMANCE_EVIDENCE.md). Optional: full-matrix `artifacts/consumer-conformance/<timestamp>/summary.json` runner.

Formalizes mechanical proof that each alpha consumer can adopt Ontogony platform packages without semantic drift.

## Consumers and required proof

| Consumer | Required proof |
| --- | --- |
| Conexus | Package-mode build, error/trace/idempotency compatibility |
| Kanon | Package-mode build, route/error/decision-record compatibility |
| Allagma | Package-mode build, cross-service propagation, runtime lock compatibility |
| Frontend | Generated client snapshot compatibility, route inventory compatibility |
| UI | Packed package import/export compatibility |

Proof is grouped from `SystemCompatibilityGate` and `SixRepoCompatibilityGate` checks. See `src/Ontogony.SystemCompatibility/ConsumerConformanceGate.cs` for the check-id mapping.

## Run locally

```powershell
./scripts/run-consumer-conformance.ps1
```

With sibling repos under `c:\dev`:

```powershell
./scripts/run-consumer-conformance.ps1 -DevRoot c:\dev
```

Release/strict mode (Warn proofs fail):

```powershell
./scripts/run-consumer-conformance.ps1 -ReleaseMode
```

Full six-repo workspace integration (fails until sibling propagation/error docs are green):

```powershell
./scripts/run-consumer-conformance.ps1 -DevRoot C:\dev -FullWorkspace
```

Faster iteration (skip package smoke builds):

```powershell
./scripts/run-consumer-conformance.ps1 -SkipPackageSmoke
```

## Artifacts

```text
artifacts/consumer-conformance/<timestamp>/summary.json
artifacts/consumer-conformance/<timestamp>/summary.md
```

## PLAT-9-004 acceptance bundle

**Status (2026-05-26):** **Done** — see [`../evidence/PLAT_9_004_PUBLIC_API_HARDENING_EVIDENCE.md`](../evidence/PLAT_9_004_PUBLIC_API_HARDENING_EVIDENCE.md).

```powershell
./scripts/validate-public-api-baseline.ps1
./scripts/validate-package-levels.ps1
./scripts/validate-shipping-inventory.ps1
./scripts/run-consumer-conformance.ps1
```

Compliance with the mechanical protocol registry is documented in [MECHANICAL_PROTOCOL_REGISTRY.md](../contracts/MECHANICAL_PROTOCOL_REGISTRY.md).
