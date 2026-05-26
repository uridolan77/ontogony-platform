# SYSTEM-COH-001 — Conexus alignment

## Conexus role

Conexus is the model gateway and routing authority for the Ontogony governed runtime.

Conexus participates in SYSTEM-COH-001 through:

- OpenAI-compatible chat completions;
- model aliases;
- provider fallback;
- model-call detail and execution journal;
- route decision detail where available;
- usage/cost/telemetry;
- optional streaming model purposes;
- Kanon assistance calls.

## Boundary

Allagma and Kanon may request a semantic model alias/purpose. Conexus owns provider/model/fallback/pricing resolution.

Allagma must not reference provider SDKs or own provider routing policy.

## SYSTEM-COH scenarios

| Scenario | Conexus obligation |
|---|---|
| governed_run_complete | complete model call and expose model call id |
| kanon_conexus_assistance | accept redacted assistance request and return advisory output |
| conexus_fallback | record fallback chain and final selected provider/model |
| correlation_chain | persist correlation/run metadata in execution journal where supplied |
| evidence_spine_operator_visibility | expose model call / route decision evidence for operator graph |
| error compatibility | classify OpenAI-compatible and admin errors for Allagma translation |

## Streaming

Streaming idempotency boundary must be explicit: Conexus may reject `Idempotency-Key` on streaming calls, and clients must handle this contract.

## Acceptance

This doc should be covered by provider contract tests or docs tests if available.
