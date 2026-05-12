# PR28 — Ontogony.ProtocolIngress Normalization

## Goal

Create a mechanical protocol ingress package that converts protocol-specific events into validated `OntogonyEnvelope<TPayload>` records.

## Why

The platform’s future value is not just tracing/errors. It should become the shared recorder substrate for AG-UI, MCP, A2A, CloudEvents, OpenTelemetry, and future agent protocols.

## Scope

Add package:

```text
src/Ontogony.ProtocolIngress/Ontogony.ProtocolIngress.csproj
```

Add abstractions:

```csharp
public interface IProtocolIngressAdapter<in TRaw>
{
    ProtocolIngressResult Normalize(TRaw raw, ProtocolIngressContext context);
}

public sealed record ProtocolIngressResult(...);
public sealed record ProtocolIngressContext(...);
public sealed record RawProtocolPayload(...);
```

Add adapters:

- CloudEvents adapter wrapping existing conversion.
- MCP event adapter DTOs/minimal normalizer.
- A2A event adapter DTOs/minimal normalizer.
- AG-UI event adapter DTOs/minimal normalizer.
- Generic JSON event adapter.

## Mechanical responsibilities

- Preserve raw payload hash.
- Assign protocol identifier.
- Generate or require trace ID according to options.
- Normalize timestamp.
- Validate source, event type, schema version.
- Emit `OntogonyEnvelope<RawProtocolPayload>` or typed payload where safe.

## Must not do

- Do not decide whether a tool call succeeded semantically.
- Do not create Agentor run records.
- Do not create Athanor canonical facts.
- Do not classify business approvals.

## Tests

- Golden raw payload fixtures.
- Deterministic normalized envelope fields.
- Invalid raw payload errors are structured.
- Missing trace policy works.
- Payload hash is stable.
- Raw payload is preserved.

## Docs

- `docs/protocol-ingress/overview.md`
- `docs/protocol-ingress/ag-ui.md`
- `docs/protocol-ingress/mcp.md`
- `docs/protocol-ingress/a2a.md`
- `docs/protocol-ingress/cloudevents.md`

## Acceptance

A sample JSON protocol event can be normalized, validated, hashed, and optionally written to the outbox without any product semantics.
