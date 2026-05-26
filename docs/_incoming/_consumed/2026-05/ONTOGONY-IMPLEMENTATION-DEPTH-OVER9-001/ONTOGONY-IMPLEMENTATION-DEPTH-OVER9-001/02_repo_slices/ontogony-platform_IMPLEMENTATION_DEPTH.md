# PLAT-DEPTH-001 to PLAT-DEPTH-004 — Platform implementation-depth slice

## Objective

Raise `ontogony-platform` implementation depth from 8.0 to 9.1+ without turning Platform into a product runtime.

## PLAT-DEPTH-001 — HTTP resilience v1 completion

Implement richer reusable HTTP resilience mechanics:

- Retry-After aware retry policy.
- Exponential backoff with jitter.
- Timeout policy with explicit operation-stage metadata.
- Circuit-breaker abstraction or adapter boundary, not hardwired product behavior.
- Conformance harness in `Ontogony.Testing`.

Suggested files:

```text
src/Ontogony.Http/Resilience/*
src/Ontogony.Testing/HttpResilience/*
docs/packages/Ontogony.Http.md
docs/evidence/PLAT_DEPTH_001_HTTP_RESILIENCE_EVIDENCE.md
```

Acceptance:

- Existing HTTP tests pass.
- New conformance tests prove Retry-After, jitter window, timeout classification, and no product semantics.
- Conexus/Allagma can consume the harness without referencing product internals.

## PLAT-DEPTH-002 — Idempotency/outbox/artifact conformance depth

Add reusable conformance harnesses, not runtime products:

- idempotency ledger conformance;
- outbox writer/reader/dispatcher conformance;
- artifact store conformance for hash/provenance semantics;
- fake in-memory implementations for tests only if needed.

Acceptance:

- Consumers can test their EF/in-memory implementations against the same mechanical contract.
- No replay runtime in Platform.
- No product-specific identifiers hard-coded.

## PLAT-DEPTH-003 — Compatibility gate expansion

Extend `Ontogony.SystemCompatibility` to validate runtime lock schema, post-lock delta schema, required repo role names, package version line consistency, propagation header set consistency, and error envelope gate references.

Acceptance:

- `run-system-compatibility-gate.ps1` reports machine-readable JSON.
- Allagma can consume the output during runtime-lock validation.

## PLAT-DEPTH-004 — Public API documentation Tier A closure

Close the highest-value public XML doc gaps for package surfaces consumed by Conexus/Kanon/Allagma.

Acceptance:

- No blanket CS1591 suppression for Tier A packages.
- Snapshot/public API tests stay stable.
- Evidence lists remaining Tier B deferrals.

## Validation commands

```powershell
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln -c Release
dotnet test Ontogony.Platform.sln -c Release --no-build
pwsh ./scripts/run-system-compatibility-gate.ps1
pwsh ./scripts/validate-shipping-inventory.ps1
pwsh ./scripts/validate-package-levels.ps1
pwsh ./scripts/validate-real-tools-block.ps1
```
