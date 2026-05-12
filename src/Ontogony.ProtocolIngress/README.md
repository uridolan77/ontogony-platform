# Ontogony.ProtocolIngress

Mechanical adapters that normalize **external** protocol payloads into `OntogonyEnvelope<RawProtocolPayload>`.

## What this is

- `IProtocolIngressAdapter<TRaw>` and concrete adapters (generic JSON, CloudEvents, MCP, A2A, AG-UI).
- `AddOntogonyProtocolIngress()` — register default adapters and validators.

## What this is not

- Not gRPC ingress (no shipped adapter today).
- Not product routing, authorization, or ingest policy beyond mechanical validation.

## See also

- `docs/packages/index.md` (ProtocolIngress section).
