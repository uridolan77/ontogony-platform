# PR-005 — Platform consumer conformance kits

## Repo focus

Primary:

```text
C:\dev\ontogony-platform
```

Consumers:

```text
C:\dev\allagma-dotnet
C:\dev\kanon-dotnet
C:\dev\conexus-dotnet
```

## Goal

Convert shared mechanics from "available packages" into consumer-proven contracts.

## Kits

Implement or strengthen conformance kits for:

```text
correlation/header propagation
CrossServiceErrorEnvelope
idempotency
observability naming
artifact/export redaction/safety
runtime compatibility/adoption manifest
```

## Platform deliverables

```text
src/Ontogony.Testing/Conformance/*
docs/adoption/reconstructability-conformance-kits.md
docs/evidence/PLATFORM_RECONSTRUCTABILITY_CONFORMANCE_EVIDENCE.md
```

## Consumer deliverables

Each backend repo should have focused tests proving relevant kits pass.

Examples:

```text
AllagmaPlatformConformanceTests
KanonPlatformConformanceTests
ConexusPlatformConformanceTests
```

## Acceptance criteria

```text
Platform remains mechanics-only.
Each consumer runs at least correlation + error envelope conformance.
Allagma and Conexus run idempotency conformance where mutations require keys.
Observability naming conformance runs where metrics were added.
Artifact/export safety conformance runs for reconstructability exports.
```
