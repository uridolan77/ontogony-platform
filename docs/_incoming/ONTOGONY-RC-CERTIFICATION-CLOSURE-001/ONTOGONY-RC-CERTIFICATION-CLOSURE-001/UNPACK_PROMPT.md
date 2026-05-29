# Unpack Prompt — ONTOGONY-RC-CERTIFICATION-CLOSURE-001

Apply this cross-repo package across the Ontogony workspace.

## Copy rules

- `repo-overlays/allagma-dotnet/*` goes into `C:\dev\allagma-dotnet\`
- `repo-overlays/kanon-dotnet/*` goes into `C:\dev\kanon-dotnet\`
- `repo-overlays/conexus-dotnet/*` goes into `C:\dev\conexus-dotnet\`
- `repo-overlays/metabole-dotnet/*` goes into `C:\dev\metabole-dotnet\`
- `repo-overlays/aisthesis-dotnet/*` goes into `C:\dev\aisthesis-dotnet\`
- `repo-overlays/ontogony-platform/*` goes into `C:\dev\ontogony-platform\`
- `scripts/*` should be copied to a stable workspace scripts folder, or into `ontogony-platform/scripts/system/` with call sites updated.

## Order

1. Runtime port lock alignment.
2. Metabole producer trigger.
3. Allagma producer trigger.
4. Allagma phenomenological bridge sync.
5. Kanon ReplayTarget package-mode fix.
6. Conexus cache metrics fix.
7. Aisthesis diagnostics.
8. Full rerun.

Do not claim RC-candidate until package-mode, five-service live cert, and system cohesion acceptance all pass. Production readiness remains not claimed.
