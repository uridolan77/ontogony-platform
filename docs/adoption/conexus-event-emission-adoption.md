# Conexus Event Emission Adoption

Broader Conexus mechanics (HTTP correlation, optional .NET stack): [`conexus-platform-adoption.md`](./conexus-platform-adoption.md).

This guide describes emitting Ontogony envelope-compatible events from Conexus (Python or other non-.NET service).

## Scope

Emit protocol-neutral envelopes for:

- llm.request.created
- llm.response.completed
- llm.provider.failed
- llm.cost.recorded

## Required Envelope Fields

- eventId
- eventType
- source
- occurredAt
- traceId
- protocol
- payload

## Mapping Notes

- Map Conexus request id to X-Conexus-Request-Id for inbound/outbound HTTP compatibility.
- Carry traceId consistently across emitted events and downstream calls.
- payloadHash may be precomputed with shared canonical-json policy or deferred with explicit TODO.

## Verification Checklist

1. Events conform to schemas/ontogony-envelope.schema.json.
2. Event type naming follows shared conventions.
3. Trace id and source are stable and non-empty.

## Do Not Do This

- Do not emit Athanor/Agentor domain types in shared envelopes.
- Do not encode provider routing policy semantics in Ontogony.Platform.
