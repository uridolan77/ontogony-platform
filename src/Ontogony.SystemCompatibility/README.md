# Ontogony.SystemCompatibility

Mechanical cross-repo compatibility gate (PLATFORM-9-001). Validates pinned manifests, OpenAPI snapshots, feature matrices, package versions, and propagation headers against the Allagma-owned runtime lock.

See [`docs/contracts/SYSTEM_COMPATIBILITY_GATE.md`](../../docs/contracts/SYSTEM_COMPATIBILITY_GATE.md) (PLATFORM-9-001) and [`docs/contracts/CROSS_SERVICE_ERROR_ENVELOPE_GATE.md`](../../docs/contracts/CROSS_SERVICE_ERROR_ENVELOPE_GATE.md) (PLATFORM-9-002).

```powershell
pwsh ./scripts/run-system-compatibility-gate.ps1 -DevRoot C:\dev
```
