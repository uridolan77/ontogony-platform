# Protocol Ingress Overview

## Purpose

`Ontogony.ProtocolIngress` provides mechanical protocol normalization adapters that convert protocol-specific events into validated `OntogonyEnvelope<RawProtocolPayload>` records. It is the shared recording substrate for diverse event protocols: CloudEvents, MCP, A2A, AG-UI, and generic JSON.

## Key Principles

**Share mechanics, not meaning.**

- Protocol adapters preserve raw payloads and hashes for reproducibility.
- No product semantics (canonization, run orchestration, approval logic) leak into ingress.
- Each service can opt out or replace adapter implementations.
- Normalized envelopes carry trace ID, timestamp, and source for distributed tracing and audit.

## Core Abstractions

### `IProtocolIngressAdapter<TRaw>`

Converts protocol-specific events into `OntogonyEnvelope<RawProtocolPayload>`.

```csharp
public interface IProtocolIngressAdapter<in TRaw>
{
    ProtocolIngressResult Normalize(TRaw raw, ProtocolIngressContext context);
}
```

### `ProtocolIngressResult`

Result of normalization containing either a validated envelope or a list of errors.

```csharp
public sealed record ProtocolIngressResult
{
    public bool IsSuccess { get; }
    public OntogonyEnvelope<RawProtocolPayload>? Envelope { get; init; }
    public IReadOnlyList<ProtocolIngressError> Errors { get; init; }
}
```

### `ProtocolIngressContext`

Options and metadata for normalization.

```csharp
public sealed record ProtocolIngressContext
{
    public string? TraceId { get; init; }
    public string? SpanId { get; init; }
    public string? ParentSpanId { get; init; }
    public TraceIdGenerationPolicy IdGenerationPolicy { get; init; }
    public ProtocolIngressContextMetadata? Metadata { get; init; }
    public DateTimeOffset? OccurredAt { get; init; }
}
```

### `RawProtocolPayload`

Preserves the original protocol event as JSON and parsed object.

```csharp
public sealed record RawProtocolPayload
{
    public required string Protocol { get; init; }
    public required string RawJson { get; init; }
    public object? ParsedObject { get; init; }
    public string? PayloadHash { get; init; }
}
```

## Mechanical Responsibilities

Each adapter:

1. **Preserves the raw payload** as JSON and parsed object for reproducibility.
2. **Assigns or requires a protocol identifier** (e.g., "cloudevents", "mcp", "a2a", "ag-ui", "generic-json").
3. **Generates or validates a trace ID** according to `TraceIdGenerationPolicy`.
4. **Normalizes timestamp** from the raw event or uses context/current time as fallback.
5. **Validates required fields** (source, event type, schema version) and emits structured errors.
6. **Computes deterministic payload hash** for deduplication and integrity.
7. **Emits OntogonyEnvelope<RawProtocolPayload>** with stable metadata.

## Protocols Supported

- **CloudEvents**: Full CloudEvents 1.0 compatibility. Extracts trace ID from `traceid` extension.
- **MCP**: Model Context Protocol events with tool calls and responses.
- **A2A**: Agent-to-Agent messages for choreography without orchestration.
- **AG-UI**: Agent UI state and user interaction events.
- **Generic JSON**: Fallback for untyped or custom JSON events.

## Usage Example

```csharp
var payloadHasher = new PayloadHasher(new Sha256ContentHashService());
var idGenerator = new GuidIdGenerator();
var clock = new SystemClock();
var envelopeValidator = new DefaultEnvelopeValidator();

var adapter = new GenericJsonProtocolAdapter(payloadHasher, idGenerator, clock, envelopeValidator);

var rawJson = @"
{
    ""eventType"": ""user.created"",
    ""source"": ""user-service"",
    ""data"": { ""userId"": ""123"" }
}
";

var context = new ProtocolIngressContext 
{
    TraceId = "trace-xyz",
    IdGenerationPolicy = TraceIdGenerationPolicy.RequireProvided,
    Metadata = new ProtocolIngressContextMetadata
    {
        TenantId = "tenant-123",
        ActorId = "actor-456"
    }
};

var result = adapter.Normalize(rawJson, context);

if (result.IsSuccess)
{
    var envelope = result.Envelope!;
    // envelope.TraceId: "trace-xyz"
    // envelope.EventType: "generic-json.ingress.normalized"
    // envelope.Payload.RawEventType: "user.created"
    // envelope.Source: "generic-json://user-service"
    // envelope.Protocol: "generic-json"
    // envelope.PayloadHash: deterministic hash of rawJson
    // envelope.TenantId: "tenant-123"
}
else
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"{error.Field}: {error.Message}");
    }
}
```

## Validation

Adapters validate at ingress:

- **Required fields**: protocol-specific raw event type and source must be present (EventId is generated if missing in some protocols).
- **TraceId policy**: RequireProvided (fail) or GenerateIfMissing.
- **Timestamp**: Normalized to UTC or from context.
- **Envelope event type policy**: uses mechanical `{protocol}.ingress.normalized`; raw protocol event type is preserved in payload.
- **Source normalization**: preserves already-absolute URIs; prefixes only non-URI identifiers with `{protocol}://`.
- **Payload hash**:
    - `RawPayloadHash`: SHA-256 of exact raw payload bytes (forensic/replay identity)
    - `CanonicalPayloadHash`: SHA-256 of canonical JSON (semantic/dedup identity)
    - `Envelope.PayloadHash`: canonical identity value

Invalid payloads return `ProtocolIngressResult` with structured `ProtocolIngressError` entries.

## Payload Hash Stability

Payload hashes are deterministic across repeated normalizations of the same raw JSON:

```csharp
// Hash1 and Hash2 are always identical for the same rawJson
var result1 = adapter.Normalize(rawJson, context1);
var result2 = adapter.Normalize(rawJson, context2);

Assert.Equal(result1.Envelope!.PayloadHash, result2.Envelope!.PayloadHash);
```

This enables:
- Deduplication across retries.
- Integrity verification.
- Fingerprinting for idempotency keys.

## DI Registration

For default dependency and adapter registration:

```csharp
services.AddOntogonyProtocolIngress();
```

This registers shared dependencies (`PayloadHasher`, `IIdGenerator`, `IClock`, `IEnvelopeValidator`) and all protocol adapters.

## Boundary

**This package does not:**

- Decide whether a tool call succeeded semantically (Agentor responsibility).
- Create Agentor run records or plan snapshots.
- Create Athanor canonical facts or contradiction semantics.
- Classify business approvals (Conexus responsibility).
- Perform RAG or knowledge graph extraction.

Each service (Agentor, Athanor, Conexus) consumes normalized envelopes and applies product semantics independently.

## Next Steps

See protocol-specific documentation:
- [`ag-ui.md`](ag-ui.md)
- [`mcp.md`](mcp.md)
- [`a2a.md`](a2a.md)
- [`cloudevents.md`](cloudevents.md)
