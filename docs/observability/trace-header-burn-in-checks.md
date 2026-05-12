# Trace Header Migration Burn-In Checks

Use this checklist while migrating clients from legacy trace headers to canonical Ontogony headers.

## Goals

- Accept canonical and legacy incoming trace identifiers.
- Prefer canonical header value when both are present.
- Emit canonical response trace header always.
- Emit legacy response aliases only when explicitly enabled.

## Checks

1. Send request with canonical `X-Ontogony-Trace-Id` only.
: expected: response contains same canonical trace value.

2. Send request with one legacy trace header only (`X-Athanor-Trace-Id`, `X-Agentor-Trace-Id`, or `X-Conexus-Request-Id`).
: expected: service accepts it as incoming trace source.

3. Send request with canonical and legacy trace headers with different values.
: expected: canonical value wins.

4. Send request with `traceparent` only.
: expected: service derives Ontogony trace ID from W3C traceparent trace-id.

5. With `EchoLegacyHeaders = false`.
: expected: response includes canonical trace header and no legacy aliases.

6. With `EchoLegacyHeaders = true`.
: expected: response includes canonical trace header and all legacy aliases.

## Suggested burn-in telemetry checks

- Validate stable trace continuity by querying traces where `ontogony.trace_id` is present.
- Validate logs include `traceId` and `operationId` scope properties.
- Validate no dashboards still depend only on legacy response headers before disabling alias echo.
