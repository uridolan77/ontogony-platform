# Kanon console status taxonomy

Use consistent status language across Kanon surfaces.

## Authority

| Status | Meaning | Example |
|---|---|---|
| `authoritative` | Kanon-owned semantic state or decision record | accepted decision, active ontology version |
| `draft_only` | Model-generated assistance that cannot mutate semantic authority | Conexus assistance output |
| `simulation_only` | Result from diff/impact/migration simulation | domain-pack impact analysis |
| `operator_local` | Browser-local settings or tokens | local actor defaults |
| `fixture_only` | Static/demo data, not live backend evidence | CI/demo dashboard fixture |
| `generated_artifact` | Generated file snapshot, not live backend | release readiness artifact |

## Connectivity

| Status | Meaning |
|---|---|
| `live` | Backend request succeeded and provided the displayed data. |
| `live_with_warning` | Backend request succeeded but payload/schema/readiness warning exists. |
| `live_with_fallback` | Backend request succeeded partially and fallback filled non-critical display fields. |
| `offline` | Backend unavailable. |
| `unknown` | UI cannot determine current connectivity. Avoid unless truly unknown. |

## Readiness

| Status | Meaning |
|---|---|
| `ready` | Contracted checks passed. |
| `not_ready` | One or more contracted checks failed. Must show reason. |
| `partial` | Some checks/data are missing. Must show reason. |
| `not_applicable` | Feature does not apply in this state. |

## Evidence completeness

| Status | Meaning |
|---|---|
| `resolved` | Identifier has a live resolved evidence target. |
| `partial` | Some evidence exists but expected links are missing. Must show missing reason. |
| `unresolved` | Lookup attempted and failed. Must show attempted source and reason. |
| `not_recorded` | Data was not emitted by this run/version. |
| `not_applicable` | No evidence expected for this state. |

## Reason codes

Use explicit reason codes internally and readable copy in the UI.

```ts
export type KanonConsoleReasonCode =
  | 'no_items'
  | 'not_configured'
  | 'insufficient_role'
  | 'backend_route_missing'
  | 'generated_client_missing'
  | 'fixture_only'
  | 'live_fallback'
  | 'health_contract_warning'
  | 'version_metadata_missing'
  | 'identifier_missing'
  | 'lookup_failed'
  | 'action_not_allowed_in_state'
  | 'already_active'
  | 'already_accepted_or_active'
  | 'cannot_deprecate_active_version'
  | 'not_in_scope'
  | 'unknown';
```

`unknown` should be rare and should trigger a follow-up if it appears in normal local-stack operation.
