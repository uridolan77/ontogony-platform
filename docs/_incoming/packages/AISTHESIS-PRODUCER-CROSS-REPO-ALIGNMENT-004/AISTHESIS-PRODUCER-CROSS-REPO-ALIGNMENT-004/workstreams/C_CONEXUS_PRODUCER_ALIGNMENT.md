# Conexus producer alignment

Conexus owns route decision, model call, provider attempt, fallback, and provider execution telemetry.

## Required

- Emit route decision with `routeDecisionId`.
- Emit model call with `modelCallId`, `routeDecisionId`, `providerAttemptId`.
- Emit:
  - `conexus.model_call_to_route_decision`
  - `conexus.model_call_to_provider_attempt`
- Same-envelope `providerAttemptId` is acceptable if no dedicated provider attempt envelope exists.
- Add tests for route/model/provider evidence and edges.
