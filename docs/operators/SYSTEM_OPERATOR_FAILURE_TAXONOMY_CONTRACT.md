# System operator failure taxonomy contract

**Sprint:** SYS-TIGHT-006 — Ontogony Tight Integration Baseline  
**Status:** Canonical cross-repo operator failure index (adapter layer only)  
**Baseline:** `SYSTEM-ALPHA-006`

## Purpose

Operators see failures from **Allagma**, **Kanon**, and **Conexus** with different public wire shapes. SYS-TIGHT-006 adds a **shared operator taxonomy** and adapter mapping layer **without** changing Kanon v0 HTTP errors, Conexus OpenAI-shaped gateway errors, or Allagma `CrossServiceErrorEnvelope` public responses.

## Authority boundaries

| Layer | Owns | Must not change |
| --- | --- | --- |
| Kanon | Ontogony `ApiError` + documented typed DTO exceptions | Wire JSON for `/ontology/v0` |
| Conexus | OpenAI-shaped `error` on `/v1/chat/completions` | Allagma envelope on public gateway |
| Allagma | `CrossServiceErrorEnvelope` on `/allagma/v0` | Kanon/Conexus public contracts |
| Platform | Taxonomy kinds, `OperatorFailureView`, `OperatorFailureTaxonomyAdapter` | Product policy semantics |
| Frontend | `operatorFailureTaxonomy.ts`, `OperatorFailureBanner` | Backend error authority |

## Taxonomy kinds (v1)

| Kind | Operator meaning |
| --- | --- |
| `auth_failed` | Missing or invalid credentials |
| `forbidden` | Authenticated but not permitted / policy denied |
| `validation_failed` | Request payload or compile validation failed |
| `not_found` | Target resource does not exist |
| `conflict` | State conflict (non-idempotency) |
| `idempotency_conflict` | Idempotency key reuse or streaming idempotency unsupported |
| `downstream_unavailable` | Dependency temporarily unavailable (Kanon plan, Conexus gateway, …) |
| `provider_failed_retryable` | Model/provider failure that may succeed on retry |
| `provider_failed_terminal` | Model/provider failure requiring input or policy change |
| `quota_exceeded` | Gateway quota exhausted |
| `timeout` | Call timed out or was canceled |
| `unknown` | Unclassified — still show trace/correlation when present |

## Canonical artifacts

| Artifact | Repo | Role |
| --- | --- | --- |
| **This document** | `ontogony-platform` | Contract index and acceptance rules |
| [`operator-failure-taxonomy.matrix.json`](../system/operator-failure-taxonomy.matrix.json) | Platform | Representative code → taxonomy mappings |
| [`OperatorFailureTaxonomyAdapter.cs`](../../src/Ontogony.Errors/OperatorFailureTaxonomyAdapter.cs) | Platform | Cross-service envelope → `OperatorFailureView` |
| [`operatorFailureTaxonomy.ts`](../../../ontogony-frontend/src/system/errors/operatorFailureTaxonomy.ts) | Frontend | TypeScript adapter (parity with platform) |
| [`OperatorFailureBanner.tsx`](../../../ontogony-frontend/src/system/errors/OperatorFailureBanner.tsx) | Frontend | Consistent operator failure banners |
| [`CROSS_SERVICE_ERROR_CONTRACT.md`](../../../allagma-dotnet/docs/integrations/CROSS_SERVICE_ERROR_CONTRACT.md) | Allagma | Downstream mapping into `CrossServiceErrorEnvelope` |

## Mapping flow

```text
Kanon ApiError / Conexus OpenAI error
        ↓ (consumer repo — unchanged public wire)
Allagma CrossServiceErrorEnvelope (+ run failure payload fields)
        ↓ OperatorFailureTaxonomyAdapter
OperatorFailureView (taxonomy, title, actions, trace/correlation)
        ↓ frontend normalizeOperatorFailure
OperatorFailureBanner
```

## Run failure payload (additive)

When Allagma maps a downstream failure, `AllagmaRunFailurePayload` may include:

- `operatorFailureTaxonomy`
- `operatorFailureTitle`
- `operatorRecommendedActions` (string array)

Public `CrossServiceErrorEnvelope` fields are unchanged.

## Acceptance (SYS-TIGHT-006)

- [x] Taxonomy matrix lists all required kinds and representative mappings.
- [x] Platform adapter tests cover matrix rows.
- [x] Frontend adapter tests cover matrix rows.
- [x] Allagma run failure payload includes taxonomy when `CrossService` is present.
- [x] Operator UI shows consistent failure banners with recommended actions.
- [x] No breaking changes to Kanon/Conexus/Allagma public HTTP error contracts (adapter layer only; verified by review and matrix parity tests, not a dedicated CI wire-contract gate).

## Validate locally

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-system-operator-failure-taxonomy.ps1
dotnet test tests/Ontogony.Infrastructure.Tests/Ontogony.Infrastructure.Tests.csproj --filter "FullyQualifiedName~OperatorFailureTaxonomy"
```

```powershell
cd C:\dev\ontogony-frontend
npm test -- src/system/errors
```
