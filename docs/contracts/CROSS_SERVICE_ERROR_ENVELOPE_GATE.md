# Cross-service error envelope gate (PLATFORM-9-002) — done

Platform-owned **conformance enforcement** for the neutral `CrossServiceErrorEnvelope` shape and cross-service failure normalization paths. This does not replace per-service HTTP contracts; it proves they map consistently when failures cross service boundaries.

**Contract v1:** [`CROSS_SERVICE_ERROR_ENVELOPE_V1.md`](./CROSS_SERVICE_ERROR_ENVELOPE_V1.md) (`SHARED-ERROR-CONTRACT-001`, promoted 2026-05-29).

Part of Phase Tight closeout (2026-05-22): [`PLATFORM_PHASE_TIGHT_2026_05_22_EVIDENCE.md`](../evidence/PLATFORM_PHASE_TIGHT_2026_05_22_EVIDENCE.md).

## Neutral envelope (Platform)

Type: `Ontogony.Errors.CrossServiceErrorEnvelope`

| Field | Required | Notes |
| --- | --- | --- |
| `code` | yes | Namespaced machine code |
| `message` | yes | Client-safe summary |
| `system` | yes | Originating system (`allagma`, `kanon`, `conexus`, …) |
| `stage` | no | Processing stage |
| `downstreamSystem` | no | Wrapped downstream |
| `traceId` | no | Trace propagation |
| `correlationId` | no | Correlation propagation |
| `retryable` | no | Retry hint |
| `detail` | no | Structured detail |

JSON Schema: [`docs/system/schemas/cross-service-error-envelope-v0.schema.json`](../system/schemas/cross-service-error-envelope-v0.schema.json)

Conformance matrix: [`docs/system/cross-service-error-envelope.matrix.json`](../system/cross-service-error-envelope.matrix.json)

## Wire shapes (by design)

| Service | Public HTTP error | Normalization |
| --- | --- | --- |
| Allagma | `CrossServiceErrorEnvelope` on `/allagma/v0` | Native |
| Metabole | `MetaboleErrorEnvelope` (compatible superset) | Native on `/metabole/v0` |
| Aisthesis | `CrossServiceErrorEnvelope` on `/aisthesis/v0` | Native |
| Kanon | Ontogony `ApiError` (`code`, `message`, optional `traceId`, `detail`) | Allagma mapper sets `system=kanon` |
| Conexus | OpenAI `error` object on `/v1/chat/completions` | `AllagmaDownstreamCrossServiceErrorMapper` |
| Frontend | Parses Allagma envelope + maps taxonomy | `operatorFailureTaxonomy.ts` |

Integration docs:

- Allagma: `allagma-dotnet/docs/integrations/CROSS_SERVICE_ERROR_CONTRACT.md`
- Kanon: `kanon-dotnet/docs/integrations/ERROR_CONTRACTS.md`
- Conexus: `conexus-dotnet/docs/architecture/BOUNDARIES.md`

## Operator taxonomy bridge (SYS-TIGHT-006)

Representative cross-service scenarios are pinned in [`operator-failure-taxonomy.matrix.json`](../system/operator-failure-taxonomy.matrix.json). PLATFORM-9-002 verifies:

1. Each mapping envelope parses as `CrossServiceErrorEnvelope`.
2. `OperatorFailureTaxonomyAdapter` produces the expected `expectedTaxonomy` kind.
3. All `requiredTaxonomyKinds` are covered.

Frontend parity: `ontogony-frontend/src/system/errors/operatorFailureTaxonomy.ts` (matrix test in frontend CI).

## Platform samples

| Sample | Wire shape |
| --- | --- |
| [`allagma-cross-service-error.sample.json`](../system/error-envelope-samples/allagma-cross-service-error.sample.json) | `CrossServiceErrorEnvelope` |
| [`kanon-api-error.sample.json`](../system/error-envelope-samples/kanon-api-error.sample.json) | `Ontogony.ApiError` |
| [`conexus-openai-error.sample.json`](../system/error-envelope-samples/conexus-openai-error.sample.json) | `OpenAI.error` |

## Run

Included in the system compatibility gate:

```powershell
pwsh ./scripts/run-system-compatibility-gate.ps1 -DevRoot C:\dev
```

Standalone structural validation (docs/matrix only):

```powershell
pwsh ./scripts/validate-cross-service-error-envelope.ps1 -DevRoot C:\dev
```

## Checks (mechanical)

| Check id | What it proves |
| --- | --- |
| `error-envelope-matrix` | Conformance matrix + schema + contract doc present |
| `error-envelope-samples` | Platform wire samples parse per shape |
| `error-envelope-taxonomy` | Taxonomy matrix mappings match `OperatorFailureTaxonomyAdapter` |
| `error-envelope-openapi` | Allagma + frontend OpenAPI expose `CrossServiceErrorEnvelope` with required fields |
| `error-envelope-sibling-docs` | Allagma/Kanon/Conexus integration contracts exist |
| `error-envelope-frontend` | Frontend taxonomy module present |

## Non-goals

- Does not require Kanon or Conexus to emit `CrossServiceErrorEnvelope` on the public wire.
- Does not prove live HTTP error responses (per-repo integration tests own that).
- Does not replace `validate-system-operator-failure-taxonomy.ps1` (still run in CI for registry protocol).

## Related

- [`SYSTEM_COMPATIBILITY_GATE.md`](./SYSTEM_COMPATIBILITY_GATE.md) (PLATFORM-9-001)
- [`SYSTEM_OPERATOR_FAILURE_TAXONOMY_CONTRACT.md`](../operators/SYSTEM_OPERATOR_FAILURE_TAXONOMY_CONTRACT.md)
