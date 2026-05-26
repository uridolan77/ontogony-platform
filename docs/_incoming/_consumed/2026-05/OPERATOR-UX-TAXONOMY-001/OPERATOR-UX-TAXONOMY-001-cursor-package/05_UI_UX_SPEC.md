# UI/UX spec

## Badge strategy

Use compact badges for independent dimensions. Do not combine all states into one ambiguous badge.

Example service card:

```text
Conexus
Connectivity: live
Readiness: not ready — provider route check failed
Contract: warning — /health missing version
Usability: degraded — route preview works, governed model-call readiness cannot be verified
Data source: live
```

## Severity strategy

Severity is derived from dimension + state + context. For example:

- `connectivity.live` is good.
- `readiness.not_ready` is warning/error depending on workflow.
- `contract.warning` is warning, not failure by itself.
- `dataSource.fixture` is neutral for demo mode but warning if used in release readiness.
- `authority.advisory` is expected for Conexus assistance, but warning if shown as semantic truth.

## Layout strategy

Primary card:

- operator outcome;
- state badges;
- reason summary;
- next action.

Developer details:

- endpoint names;
- raw request/response previews;
- route templates;
- stack-specific debug copy;
- payload contract mismatch details.

## Prohibited headline patterns

Do not use these as page titles, card titles, or top-level identity labels:

- `Live with fixture fallback`
- `Fixture replay` without `Demo fixture — not live evidence`
- `healthy` when readiness/contract is not clean
- `unknown` without subject
- `sample` without scope

## Unknown label pattern

Use:

```text
{Subject}: unknown — {reason}
```

Examples:

```text
Provider: unknown — model-call record did not include provider metadata
Compatibility: unknown — no compatibility summary artifact was found
Readiness: unknown — /ready has not been queried
Task type: unknown — task classification event is absent
```

## Topology link pattern

Each topology edge should show:

```text
Allagma → Kanon
State: validated / degraded / missing / planned / blocked
Expected route(s): ...
Last attempt: ...
Last success: ...
Trace/correlation evidence: ...
Reason: ...
```
