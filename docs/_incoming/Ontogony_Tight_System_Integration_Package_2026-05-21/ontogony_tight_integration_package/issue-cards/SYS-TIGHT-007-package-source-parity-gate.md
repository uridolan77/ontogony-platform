# SYS-TIGHT-007 — Package-mode and sibling-source parity gate

**Repo:** allagma-dotnet  
**Type:** CI/release validation  
**Priority:** P0

## Goal

Ensure release candidates work both in sibling-source mode and package mode.

## Scope

- Run full Allagma package mode with pinned package versions.
- Fail on Kanon/Conexus package mismatch.
- Include streaming contract compile check.

## Acceptance

- Package-mode build/test passes before lock promotion.
- Runtime lock package versions match build inputs.
- Evidence copied into closeout artifacts.
