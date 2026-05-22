# Ontogony.Platform — 0.4 alpha-RC substrate contract

**Contract id:** `ONTOGONY-PLATFORM-0.4-ALPHA-RC`  
**PR:** `PLATFORM-RC-001-substrate-contract-freeze`  
**Runtime baseline:** `SYSTEM-RC-001A` ([`allagma-dotnet/docs/system/ontogony-runtime.lock.json`](../../../allagma-dotnet/docs/system/ontogony-runtime.lock.json))  
**Date:** 2026-05-22

This document freezes the **shared mechanical substrate** for the backend alpha-RC. It does **not** certify production readiness, enterprise IAM, or semantic authority outside Kanon.

---

## package_version_line

| Field | Value |
| --- | --- |
| NuGet / assembly line | **`0.3.0-alpha.1`** (`Directory.Build.props`) |
| Contract name | **0.4 alpha-RC** — RC certification label for the substrate contract family; not a package semver bump |
| Runtime lock packages | Ontogony `0.3.0-alpha.1`; Kanon `0.1.0-alpha.0`; Conexus `0.1.0-alpha.1` |
| Shipping inventory | **23** libraries — [`scripts/validate-shipping-inventory.ps1`](../../scripts/validate-shipping-inventory.ps1) |

A future **`0.4.x`** NuGet line is reserved for event-substrate or breaking packaging moves per [`README.md`](../../README.md) and [`FRAMEWORK_BASELINE.md`](../FRAMEWORK_BASELINE.md). This RC freeze keeps **`0.3.0-alpha.1`** as the consumed line until an explicit platform release PR bumps `<Version>`.

---

## public_api_freeze_scope

**In scope (frozen at RC):**

- All shipping assemblies under `src/Ontogony.*` listed in the shipping inventory.
- Public surface baselines in [`tests/Ontogony.PublicApi.Tests`](../../tests/Ontogony.PublicApi.Tests) (`*.verified.txt` per assembly).
- Cross-service mechanical DTOs: error envelope, evidence-spine bundle schema, agent-interaction event/session schemas, operator failure taxonomy adapter types.
- Protocol-neutral contracts in `Ontogony.Contracts`, `Ontogony.AI.Contracts`, `Ontogony.Replay.Contracts`, `Ontogony.Topology.Contracts`, `Ontogony.Evaluation.Contracts`.

**Out of scope (not platform-owned):**

- HTTP route catalogs for Allagma, Kanon, Conexus (owned by product repos).
- OpenAPI document names and ontology `/ontology/v0` semantics (Kanon).
- Gateway routing, quota policy, provider adapters (Conexus).

**RC rule:** No new **product** semantics in Platform for this baseline. Additive **mechanical** APIs require public API snapshot review and changelog entry per [`PUBLIC_API_COMPATIBILITY.md`](../planning/robustness/PUBLIC_API_COMPATIBILITY.md).

---

## allowed_breaking_change_process

1. Classify the change (breaking vs additive-only) using the public API snapshot diff.
2. Add [`CHANGELOG.md`](../../CHANGELOG.md) entry; add [`docs/migrations/`](../migrations/) when the break is non-obvious.
3. Refresh `*.verified.txt` only after prose is drafted.
4. Coordinate downstream repos (Conexus, Kanon, Allagma) and **runtime lock** promotion in `allagma-dotnet` — silent package bumps are forbidden for Phase 1.
5. Re-run the [consumer validation commands](#consumer_validation_commands) before merging.

Pre-1.0 breaks are allowed but must never be silent. See [`PACKAGE_COMPATIBILITY_CHECKLIST_0.3.0-alpha.1.md`](../governance/PACKAGE_COMPATIBILITY_CHECKLIST_0.3.0-alpha.1.md).

---

## migration_and_changelog_policy

| Change type | Changelog | Migration note |
| --- | --- | --- |
| Additive public API | Required for release notes; migration optional | — |
| Breaking public API | Required | Required when not trivially discoverable |
| Docs-only / registry refresh | Required under Unreleased | Optional pointer doc |
| Behavior change in mechanics (hashing, envelope mapping) | Required | Required when consumers must react |

Registry baseline promotion (`SYSTEM-ALPHA-006` → `SYSTEM-RC-001A`): [`docs/migrations/2026-05-22-platform-rc-001-substrate-contract-freeze.md`](../migrations/2026-05-22-platform-rc-001-substrate-contract-freeze.md).

---

## consumer_validation_commands

Evidence for RC consumers must be **PASS** at lock SHAs before this contract is treated as closed.

### Platform (substrate owner)

```powershell
cd C:\dev\ontogony-platform
dotnet test Ontogony.Platform.sln -c Release
.\scripts\validate-system-protocol-registry.ps1
.\scripts\validate-system-evidence-spine-contract.ps1
.\scripts\validate-system-operator-failure-taxonomy.ps1
.\scripts\validate-package-levels.ps1
dotnet test tests\Ontogony.PublicApi.Tests -c Release --no-build
```

### Allagma (package-mode certification)

```powershell
cd C:\dev\allagma-dotnet
.\scripts\run-package-mode-build.ps1 -WriteCiEvidenceSummary
```

Evidence: [`SYSTEM_RC_001C_PACKAGE_MODE_CERTIFICATION.md`](../../../allagma-dotnet/docs/evidence/SYSTEM_RC_001C_PACKAGE_MODE_CERTIFICATION.md).

### Kanon (source or package mode)

```powershell
cd C:\dev\kanon-dotnet
.\scripts\validate-semantic-authority-certification-matrix.ps1
.\scripts\run-semantic-authority-certification-matrix.ps1
```

Evidence: [`KANON_RC_001_SEMANTIC_AUTHORITY_CERTIFICATION_EVIDENCE.md`](../../../kanon-dotnet/docs/evidence/KANON_RC_001_SEMANTIC_AUTHORITY_CERTIFICATION_EVIDENCE.md).

### Conexus (source or package mode)

```powershell
cd C:\dev\conexus-dotnet
.\scripts\validate-gateway-certification-matrix.ps1
.\scripts\run-gateway-certification-matrix.ps1
```

Evidence: [`CONEXUS_RC_001_GATEWAY_CERTIFICATION_EVIDENCE.md`](../../../conexus-dotnet/docs/evidence/CONEXUS_RC_001_GATEWAY_CERTIFICATION_EVIDENCE.md).

### System cohesion (cross-repo, optional local)

```powershell
cd C:\dev\allagma-dotnet
.\scripts\run-system-cohesion-smoke.ps1 -UseExistingServices `
  -IncludeKanonAssistance -IncludeConexusFallback -IncludeStreamingEvidence
```

---

## package_level_matrix

Authoritative allowed `ProjectReference` edges:

- Human matrix: [`docs/architecture/package-levels.md`](../architecture/package-levels.md)
- CI golden map: [`scripts/validate-package-levels.ps1`](../../scripts/validate-package-levels.ps1)
- Governance: [`docs/governance/PACKAGE_LEVEL_GOVERNANCE.md`](../governance/PACKAGE_LEVEL_GOVERNANCE.md)

**RC requirement:** `validate-package-levels.ps1` **PASS** with no new forbidden edges.

---

## protocol_registry_refresh_policy

| Rule | Detail |
| --- | --- |
| Owner | `ontogony-platform` — [`docs/system/system-protocol-registry.json`](../system/system-protocol-registry.json) |
| Lock sync | `repos[].commit` for `ontogony-platform`, `allagma-dotnet`, `kanon-dotnet`, `conexus-dotnet` must match [`ontogony-runtime.lock.json`](../../../allagma-dotnet/docs/system/ontogony-runtime.lock.json) `lockedCommits` |
| Baseline field | Registry `baseline` must equal lock `baseline` (`SYSTEM-RC-001A` for this RC) |
| Evidence paths | Every `protocols[].paths` and `evidence[].path` entry must resolve under `DevRoot` sibling layout |
| Validator | [`scripts/validate-system-protocol-registry.ps1`](../../scripts/validate-system-protocol-registry.ps1) |
| When to refresh | After runtime lock promotion, RC certification PRs, or new cross-repo protocol surfaces — never silently on unrelated PRs |

**This RC refresh:** baseline `SYSTEM-RC-001A`; cohesion/observability/evidence-spine artifact timestamps updated to 2026-05-22 RC runs; RC certification evidence indexed.

---

## evidence_spine_contract_status

| Artifact | Status |
| --- | --- |
| [`SYSTEM_EVIDENCE_SPINE_CONTRACT.md`](../operators/SYSTEM_EVIDENCE_SPINE_CONTRACT.md) | **Frozen** — observational resolver contract |
| [`system-evidence-spine-resolution.matrix.json`](../system/system-evidence-spine-resolution.matrix.json) | **PASS** — `validate-system-evidence-spine-contract.ps1` |
| Golden run (SYSTEM-RC-001E) | **PASS** — [`SYSTEM_RC_001E_EVIDENCE_SPINE_GOLDEN_RUN.md`](../../../allagma-dotnet/docs/evidence/SYSTEM_RC_001E_EVIDENCE_SPINE_GOLDEN_RUN.md) |

Matrix `baseline` may remain `SYSTEM-ALPHA-006` until a dedicated matrix promotion PR; mechanical contract and validators are authoritative for RC.

---

## operator_failure_taxonomy_status

| Artifact | Status |
| --- | --- |
| [`SYSTEM_OPERATOR_FAILURE_TAXONOMY_CONTRACT.md`](../operators/SYSTEM_OPERATOR_FAILURE_TAXONOMY_CONTRACT.md) | **Frozen** |
| [`operator-failure-taxonomy.matrix.json`](../system/operator-failure-taxonomy.matrix.json) | **PASS** — `validate-system-operator-failure-taxonomy.ps1` |
| [`OperatorFailureTaxonomyAdapter.cs`](../../src/Ontogony.Errors/OperatorFailureTaxonomyAdapter.cs) | Shipped; maps `CrossServiceErrorEnvelope` without changing service error contracts |

---

## non_semantic_boundary_statement

```text
Platform owns mechanics only.
Kanon owns meaning.
Allagma owns governed execution.
Conexus owns model access.
```

**Allowed in Platform (RC freeze):** trace propagation, correlation, idempotency primitives, error envelope mechanics, HTTP resilience, hashing/fingerprints, redaction/secrets abstractions, execution journal facts, protocol registry, operator taxonomy adapter, evidence-spine and agent-interaction **schemas**.

**Forbidden in Platform:** canonical truth, ontology meaning, agent planning semantics, provider routing policy, business approval rules, iGaming/product logic.

See [`AGENTS.md`](../../AGENTS.md) and [`docs/governance/PHASE1_CONSUMER_COMPATIBILITY.md`](../governance/PHASE1_CONSUMER_COMPATIBILITY.md).

---

## RC acceptance (PLATFORM-RC-001)

| Criterion | Status |
| --- | --- |
| Protocol registry reflects runtime lock | **Required** — see evidence |
| No product semantics added | **Required** |
| Public API diff reviewed (snapshots unchanged unless intentional) | **Required** |
| Evidence spine + operator taxonomy validators pass | **Required** |
| Package levels validator passes | **Required** |
| Consumer RC evidence linked | **Required** — Allagma 001C, Kanon RC-001, Conexus RC-001 |

Evidence: [`PLATFORM_RC_001_SUBSTRATE_CONTRACT_FREEZE_EVIDENCE.md`](../evidence/PLATFORM_RC_001_SUBSTRATE_CONTRACT_FREEZE_EVIDENCE.md).
