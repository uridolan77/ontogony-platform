# Repo task — conexus-dotnet

Conexus owns model gateway participation in SYSTEM-COH-001.

## Required work

Add or update:

- `docs/integrations/SYSTEM_COHESION_CONEXUS_ALIGNMENT.md`

The doc should state:

1. Conexus owns alias → provider/model/fallback/pricing routing.
2. Allagma sends model purpose / model alias, not provider policy.
3. Kanon assistance calls are advisory and must be redacted upstream.
4. Model-call telemetry must expose enough metadata for correlation: trace/correlation/run/modelCall/routeDecision where available.
5. Streaming idempotency boundary: `Idempotency-Key` is not supported on streaming calls; Allagma must omit it for StreamAsync.
6. Fallback chain evidence expectations.
7. Public OpenAI-compatible error shape and admin/operator error shape.

## Tests

Add/extend docs tests or provider-fallback tests if the repo already has provider contract gates.

## Done when

SYSTEM-COH-001 can cite Conexus model alias, fallback, telemetry, and error-compatibility behavior as explicit contract, not convention.
