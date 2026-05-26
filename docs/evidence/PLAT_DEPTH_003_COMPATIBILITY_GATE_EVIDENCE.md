# PLAT-DEPTH-003 — Compatibility gate expansion

**Date:** 2026-05-26  
**Package:** ONTOGONY-IMPLEMENTATION-DEPTH-OVER9-001

## Scope

Machine-readable system compatibility summary plus runtime lock / post-lock delta shape validation.

## Checks added

| Check id | Description |
| --- | --- |
| `runtime-lock-shape` | Required `ontogony-runtime.lock.json` fields and 40-char SHAs |
| `post-lock-deltas` | `ontogony-post-lock-deltas-v1` schema in Allagma register |

Existing gate still covers: registry vs lock alignment, Kanon/Conexus manifests, feature matrix, propagation headers, error envelope matrix.

## Artifacts

```text
artifacts/system-compat/system-compatibility-summary.json
artifacts/system-compat/system-compatibility-summary.md
```

## Validation

```powershell
pwsh ./scripts/run-system-compatibility-gate.ps1 -DevRoot C:\dev
```

Allagma `validate-runtime-lock.ps1 -ReleaseMode` optionally requires a green compatibility summary when present under the Platform repo.
