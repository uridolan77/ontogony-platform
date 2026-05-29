# Cross-repo validation plan — ONTOGONY-BACKEND-COORDINATION-002

**Dev root:** `C:\dev` (or `-DevRoot` parameter)

---

## Preflight (every slice)

```powershell
cd C:\dev\ontogony-platform
pwsh -File .\docs\_incoming\_active\ONTOGONY-BACKEND-COORDINATION-002\scripts\validate-backend-coordination-002-preflight.ps1 `
  -DevRoot C:\dev -Slice <SLICE-ID>
```

---

## Slice 1 — BACKEND-REPO-DOCS-ORDER-002

Per repo:

```powershell
pwsh -File .\scripts\validate-docs-incoming-hygiene.ps1
pwsh -File .\scripts\validate-docs-links.ps1
```

Allagma additionally:

```powershell
pwsh -File .\scripts\check-doc-freshness.ps1
```

---

## Slice 2 — SYSTEM-COMPATIBILITY-MATRIX-001

```powershell
cd C:\dev\allagma-dotnet
./scripts/validate-runtime-lock.ps1 -DevRoot C:\dev
./scripts/validate-runtime-lock.ps1 -DevRoot C:\dev -ReleaseMode
./scripts/architecture-conformance/run-cross-repo-conformance.ps1 -DevRoot C:\dev
./scripts/validate-feature-connection-matrix.ps1 -DevRoot C:\dev
dotnet test tests/Allagma.ArchitectureConformance.Tests -c Release
```

---

## Slice 3 — SHARED-ERROR-CONTRACT-001

```powershell
cd C:\dev\ontogony-platform
./scripts/validate-cross-service-error-envelope.ps1 -DevRoot C:\dev
dotnet test tests/Ontogony.SystemCompatibility.Tests -c Release --filter "FullyQualifiedName~CrossServiceError"

# Per product repo
dotnet test <solution> -c Release --filter "FullyQualifiedName~ErrorEnvelope|FullyQualifiedName~OntogonyError"
```

---

## Slice 4 — CROSS-REPO-IDENTITY-CORRELATION-001

```powershell
cd C:\dev\allagma-dotnet
./scripts/run-system-coh-001-acceptance.ps1 -DevRoot C:\dev -IncludeCorrelationChain

cd C:\dev\ontogony-platform
dotnet test tests/Ontogony.Http.Tests -c Release --filter "FullyQualifiedName~Propagation"
dotnet test tests/Ontogony.Testing.Tests -c Release --filter "FullyQualifiedName~Propagation"  # if exists
```

---

## Slice 5 — ALLAGMA-CONEXUS-MODEL-ALIAS-001

```powershell
cd C:\dev\allagma-dotnet
# Grep gate (no provider model IDs in src)
rg "gpt-[0-9]" src/ --glob "!*Tests*" && exit 1 || echo "OK"

dotnet test tests/Allagma.Tests -c Release --filter "FullyQualifiedName~ModelPurpose|FullyQualifiedName~ConexusModel"

cd C:\dev\conexus-dotnet
dotnet test tests/Conexus.Tests -c Release --filter "FullyQualifiedName~Alias|FullyQualifiedName~Routing"
```

---

## Slice 6 — BACKEND-SYSTEM-E2E-001

```powershell
cd C:\dev\allagma-dotnet
./scripts/run-local-stack.ps1 -DevRoot C:\dev
./scripts/smoke-first-system.ps1
./scripts/run-system-cohesion-smoke.ps1 -UseExistingServices -DevRoot C:\dev
./scripts/validate-real-tools-block.ps1

dotnet test Allagma.sln -c Release --filter "Category!=CrossRepo&Category!=PersistenceSmoke"
```

Optional package-mode:

```powershell
./scripts/run-package-mode-build.ps1
```

---

## Slice 7 — AISTHESIS-RECONSTRUCTABILITY-SPINE-001

```powershell
cd C:\dev\aisthesis-dotnet
./scripts/system/run-five-service-stack-for-aisthesis.ps1 -WorkspaceRoot C:\dev
./scripts/system/run-five-service-live-certification.ps1 -Mode Live
./scripts/system/run-aisthesis-rc-certification.ps1 -LiveMode Live
```

---

## Slice 8 — METABOLE-DATA-SPINE-HARDENING-001

```powershell
cd C:\dev\metabole-dotnet
./scripts/smoke/run-metabole-five-service-certification.ps1
./scripts/test/run-status-truth.ps1
./scripts/test/run-undeniability.ps1
dotnet test Metabole.sln -c Release
```

---

## Full sprint regression (parent closeout)

Run slices 2–6 validators sequentially, then 7 and 8. Record output in:

```text
allagma-dotnet/docs/evidence/ONTOGONY_BACKEND_COORDINATION_002_VALIDATION_LOG.txt
```

---

## Skip policy

| Category | Document as |
| --- | --- |
| Pre-existing failure unchanged by slice | `DEFERRED_WITH_REASON` + link to issue |
| Environment (SDK, secrets, Postgres) | `SKIP_ENV` in evidence |
| Cleanup-caused failure | **Fix before closeout** |
| Flaky integration | `DEFERRED_WITH_REASON` + rerun count |

Do not mark parent package PASS with silent skips.
