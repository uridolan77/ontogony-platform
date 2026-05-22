# PLATFORM-RC-001 — Substrate contract freeze evidence

**Date:** 2026-05-22  
**PR:** `PLATFORM-RC-001-substrate-contract-freeze`  
**Runtime baseline:** `SYSTEM-RC-001A`  
**Verdict:** **PASS** — substrate RC contract published; protocol registry refreshed to runtime lock; validators green.

**Non-claims:** Not production readiness. Does not bump the NuGet line from `0.3.0-alpha.1`. Does not promote semantic authority, gateway policy, or Allagma orchestration into Platform.

---

## Deliverables

| Artifact | Path |
| --- | --- |
| Substrate RC contract | [`docs/governance/ONTOGONY_PLATFORM_0_4_ALPHA_RC_CONTRACT.md`](../governance/ONTOGONY_PLATFORM_0_4_ALPHA_RC_CONTRACT.md) |
| Protocol registry | [`docs/system/system-protocol-registry.json`](../system/system-protocol-registry.json) |
| Migration note | [`docs/migrations/2026-05-22-platform-rc-001-substrate-contract-freeze.md`](../migrations/2026-05-22-platform-rc-001-substrate-contract-freeze.md) |

---

## Commands (platform)

```powershell
cd C:\dev\ontogony-platform
dotnet test Ontogony.Platform.sln -c Release
.\scripts\validate-system-protocol-registry.ps1
.\scripts\validate-system-evidence-spine-contract.ps1
.\scripts\validate-system-operator-failure-taxonomy.ps1
.\scripts\validate-package-levels.ps1
dotnet test tests\Ontogony.PublicApi.Tests\Ontogony.PublicApi.Tests.csproj -c Release
```

---

## Validation results

| Gate | Result |
| --- | --- |
| `Ontogony.Platform.sln` Release tests | **PASS** (616 tests) |
| `validate-system-protocol-registry.ps1` | **PASS** (`baseline=SYSTEM-RC-001A`, `DevRoot=C:\dev`) |
| `validate-system-evidence-spine-contract.ps1` | **PASS** |
| `validate-system-operator-failure-taxonomy.ps1` | **PASS** (17 mappings) |
| `validate-package-levels.ps1` | **PASS** (includes `Ontogony.Secrets.AzureKeyVault` golden-map entry) |
| `Ontogony.PublicApi.Tests` | **PASS** — no intentional snapshot change in this PR |

---

## Protocol registry refresh

| Field | Before (ALPHA-006 era) | After (RC-001A) |
| --- | --- | --- |
| `baseline` | `SYSTEM-ALPHA-006` | `SYSTEM-RC-001A` |
| `generatedAt` | `2026-05-20` | `2026-05-22` |
| `ontogony-platform` commit | `23ba850…` | `25703a438c54f93d145ca4a8e1899aa13f3dce2f` |
| `allagma-dotnet` commit | `6dbf087…` | `5787e4ab39f6284f9bb8ab2c27ad8458c5bbbc78` |
| `kanon-dotnet` commit | `515c7aa…` | `816a09f257b0a77912acbdfabffcc23e90f6caf0` |
| `conexus-dotnet` commit | `100c33d…` | `2ad88c6ea5121bf5c3ad501242065235dfbfd83d` |

Cohesion / observability / evidence-spine artifact paths updated to 2026-05-22 RC timestamps (see lock [`evidence`](../../../allagma-dotnet/docs/system/ontogony-runtime.lock.json)).

New registry protocol: `platform-substrate-rc-contract`. RC certification evidence rows added for SYSTEM-RC-001A–E, CONEXUS-RC-001, KANON-RC-001, PLATFORM-RC-001.

---

## Consumer compatibility (lock-pinned)

| Consumer | Evidence | Mode |
| --- | --- | --- |
| Allagma | [`SYSTEM_RC_001C_PACKAGE_MODE_CERTIFICATION.md`](../../../allagma-dotnet/docs/evidence/SYSTEM_RC_001C_PACKAGE_MODE_CERTIFICATION.md) | Package-mode build at lock versions |
| Kanon | [`KANON_RC_001_SEMANTIC_AUTHORITY_CERTIFICATION_EVIDENCE.md`](../../../kanon-dotnet/docs/evidence/KANON_RC_001_SEMANTIC_AUTHORITY_CERTIFICATION_EVIDENCE.md) | Certification matrix (20 rows) |
| Conexus | [`CONEXUS_RC_001_GATEWAY_CERTIFICATION_EVIDENCE.md`](../../../conexus-dotnet/docs/evidence/CONEXUS_RC_001_GATEWAY_CERTIFICATION_EVIDENCE.md) | Gateway matrix (19 rows) |

System gates (Allagma-owned evidence, referenced by registry):

| Gate | Evidence |
| --- | --- |
| Lock promotion | [`SYSTEM_RC_001A_RUNTIME_LOCK_PROMOTION_EVIDENCE.md`](../../../allagma-dotnet/docs/evidence/SYSTEM_RC_001A_RUNTIME_LOCK_PROMOTION_EVIDENCE.md) |
| Cohesion | [`SYSTEM_RC_001B_FULL_COHESION_EVIDENCE.md`](../../../allagma-dotnet/docs/evidence/SYSTEM_RC_001B_FULL_COHESION_EVIDENCE.md) |
| Observability | [`SYSTEM_RC_001D_OBSERVABILITY_PASS.md`](../../../allagma-dotnet/docs/evidence/SYSTEM_RC_001D_OBSERVABILITY_PASS.md) |
| Evidence spine golden run | [`SYSTEM_RC_001E_EVIDENCE_SPINE_GOLDEN_RUN.md`](../../../allagma-dotnet/docs/evidence/SYSTEM_RC_001E_EVIDENCE_SPINE_GOLDEN_RUN.md) |

---

## Acceptance mapping

| Criterion | Result |
| --- | --- |
| Protocol registry reflects runtime lock | **PASS** |
| No product semantics added to Platform | **PASS** (docs + registry only) |
| Public API diff reviewed | **PASS** (no snapshot changes) |
| Evidence spine + operator taxonomy validators | **PASS** (see commands) |
| Package levels validator | **PASS** |
| Consumer RC evidence linked | **PASS** |

---

## Follow-up

- **SYSTEM-RC-001F** / final closeout: refresh registry evidence for AG-UI spine certification when that PR lands.
- Promote auxiliary matrix `baseline` fields (`operator-failure-taxonomy`, `system-evidence-spine-resolution`) in a follow-up if strict baseline parity is required across all JSON artifacts.
