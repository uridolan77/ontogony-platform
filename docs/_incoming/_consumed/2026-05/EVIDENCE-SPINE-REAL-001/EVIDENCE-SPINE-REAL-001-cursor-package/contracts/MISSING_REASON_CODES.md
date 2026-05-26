# Missing Reason Codes

Use stable reason codes. Operator UI may localize/humanize them, but code and tests should assert the enum.

## Codes

| Code | Meaning | Operator message pattern |
|---|---|---|
| `not_applicable` | The relationship is not expected for this root/context. | This identifier was not produced by a governed flow, so this link is not applicable. |
| `not_recorded` | The system does not record this evidence today. | The source system did not record this relationship. |
| `not_resolved` | Identifier exists but resolver could not expand it. | The relationship is known, but the resolver could not load detail. |
| `backend_missing` | Detail row/resource missing even though ID was emitted. | An ID was recorded, but the detail endpoint did not find it. |
| `authorization_failed` | Caller lacks permission. | The source endpoint denied access. Check actor roles or credentials. |
| `fixture_only` | Evidence came from fixtures, not live backend. | This evidence is fixture/demo-only and does not prove live connectivity. |
| `not_implemented` | Capability is intentionally absent. | This lookup is not implemented yet. |
| `contract_mismatch` | Response shape did not match expected contract. | The endpoint responded, but its payload did not match the expected schema. |
| `route_mismatch` | Resolver used wrong route or route version. | The resolver attempted an unsupported route template. |
| `lookup_failed` | Generic network/API failure after mapping. | The lookup failed; inspect source attempt details. |
| `upstream_unavailable` | Service was down/unreachable. | The source service was unavailable. |
| `timeout` | Request timed out. | The source lookup timed out. |
| `redacted` | Evidence exists but is intentionally withheld. | Evidence exists but was redacted from this view/export. |

## Required behavior

- Do not render raw exception messages as the only explanation.
- Do not use `unknown` as a final reason code unless wrapped by a specific diagnostic warning.
- Prefer `not_applicable` over `missing` when the relationship is not semantically expected.
- Prefer `backend_missing` when an ID exists but detail lookup returns 404.
- Prefer `not_recorded` when the backend intentionally does not persist that class of evidence.
- Prefer `contract_mismatch` when response payload is parseable but incompatible.

## Applicability examples

Direct Conexus model call:

```json
{
  "relationship": "used_kanon_decision",
  "applicability": "not_applicable",
  "reasonCode": "not_applicable",
  "message": "This Conexus model call was resolved directly and is not known to have passed through Allagma/Kanon governance."
}
```

Governed Allagma run missing route-decision detail:

```json
{
  "relationship": "used_route_decision",
  "applicability": "required",
  "reasonCode": "backend_missing",
  "message": "Conexus evidence links recorded a routeDecisionId, but the route-decision detail endpoint did not return a record."
}
```
