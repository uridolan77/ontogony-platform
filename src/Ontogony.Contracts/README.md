# Ontogony.Contracts

Canonical event envelope, header names, and protocol contracts for Ontogony services.

## Overview

`Ontogony.Contracts` defines the mechanical DTOs and constants that enable cross-service communication:

- **`OntogonyEnvelope<TPayload>`** — Protocol-neutral event wrapper with trace correlation, tenant context, and payload hash
- **`OntogonyEventHeaders`** — Canonical header names for trace ID, actor, tenant, and workspace context
- **Protocol names and constants** — Enumerated protocol types and standard event type conventions

## Usage

```csharp
using Ontogony.Contracts.Events;

// Publish an event
var envelope = new OntogonyEnvelope<MyPayload>
{
    EventId = Guid.NewGuid().ToString("n"),
    EventType = "ontogony.myservice.operation.started",
    Source = "ontogony://myservice/domain",
    TraceId = traceId,
    Protocol = "internal",
    Payload = myPayload
};

await publisher.PublishAsync(envelope);
```

## Key Types

- `OntogonyEnvelope<TPayload>` — Required fields: `EventId`, `EventType`, `Source`, `OccurredAt`, `TraceId`, `Protocol`, `Payload`
- `OntogonyEventHeaders` — Constants like `TraceId`, `TenantId`, `ActorId`, `WorkspaceId`, `ProjectId`
- `ProtocolNames` — Well-known protocol identifiers (`internal`, `cloudevents`, etc.)

## Design

- **No business logic.** This package is pure mechanics: DTOs, enums, and constants.
- **Protocol-neutral.** Envelopes work with any message transport (in-process, message bus, HTTP, etc.).
- **Deterministic.** Envelope fields enable reproducible hashing and idempotency keys.

## License

MIT — see LICENSE in the repository root.
