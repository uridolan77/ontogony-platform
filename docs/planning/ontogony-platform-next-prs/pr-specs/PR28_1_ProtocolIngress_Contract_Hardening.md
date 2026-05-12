# PR28.1 — ProtocolIngress Contract Hardening

## Goal

Harden `Ontogony.ProtocolIngress` to meet production-grade ingress correctness standards:

1. **Deterministic timestamps** via injected `IClock`
2. **Proper trace ID fallback** with correct precedence
3. **Envelope validation** against platform contracts
4. **Absolute source URIs** for all protocols
5. **Event type policy clarity** (preserve raw in payload, use mechanical envelope types)

## Why

PR28 established the right strategic boundary (mechanical normalization, no product semantics). But it lacks correctness guards required when this package becomes the **canonical protocol recorder ingestion layer** for Agentor, Athanor, and Conexus.

Current issues:

- Envelopes bypass `DefaultEnvelopeValidator` → source URIs and event types may violate contracts
- Timestamps fall back to wall-clock `DateTimeOffset.UtcNow` → breaks tests and determinism
- Trace ID fallback treats empty string as "found" instead of cascading to context → loses trace correlation
- Sources like `"mcp-agent"` are not absolute URIs → violate PR15 envelope contract
- CloudEvents model is duplicated and weaker than `Contracts` version

## Scope

### Add

```text
src/Ontogony.ProtocolIngress/
  ProtocolIngressValidationError.cs (new)
  IEnvelopeValidator (injected from Contracts)
  
src/Ontogony.ProtocolIngress/Adapters/
  BaseProtocolIngressAdapter.cs (refactor)
    - Add IClock injection
    - Add FirstNonWhiteSpace helper
    - Add envelope validation
    - Update source normalization
```

### Update

```text
src/Ontogony.ProtocolIngress/Adapters/
  GenericJsonProtocolAdapter.cs
  CloudEventsProtocolAdapter.cs
  McpProtocolAdapter.cs
  A2aProtocolAdapter.cs
  AgUiProtocolAdapter.cs
  
tests/Ontogony.ProtocolIngress.Tests/
  ProtocolIngressAdapterTests.cs
    - Add validation integration tests
    - Add trace fallback edge-case tests
    - Add deterministic timestamp tests (using FakeClock)
    - Add source URI normalization tests
```

### Remove

```text
Ontogony.ProtocolIngress/Adapters/BaseProtocolIngressAdapter.cs
  - Remove dead ApplyContextMetadata method
```

## Phase 1: P0 Fixes (Required)

### 1.1 Inject `IClock`

Change signature:

```csharp
protected BaseProtocolIngressAdapter(
    PayloadHasher payloadHasher,
    IIdGenerator idGenerator,
    IClock clock)
{
    PayloadHasher = payloadHasher ?? throw new ArgumentNullException(nameof(payloadHasher));
    IdGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
    Clock = clock ?? throw new ArgumentNullException(nameof(clock));
}

protected readonly IClock Clock;
```

Update `NormalizeTimestamp`:

```csharp
protected DateTimeOffset NormalizeTimestamp(DateTimeOffset? providedTime, ProtocolIngressContext context)
{
    if (providedTime.HasValue)
        return providedTime.Value;

    if (context.OccurredAt.HasValue)
        return context.OccurredAt.Value;

    return Clock.UtcNow;  // Use injected clock, not wall-clock
}
```

### 1.2 Fix Trace ID Fallback

Add helper:

```csharp
private static string? FirstNonWhiteSpace(params string?[] values)
{
    foreach (var value in values)
    {
        if (!string.IsNullOrWhiteSpace(value))
            return value;
    }
    return null;
}
```

Update trace ID validation:

```csharp
protected string? ValidateOrGenerateTraceId(
    string? providedTraceId,
    ProtocolIngressContext context,
    out ProtocolIngressResult? error)
{
    error = null;
    
    // Proper fallback: raw -> context -> generate
    var traceId = FirstNonWhiteSpace(providedTraceId, context.TraceId);

    if (string.IsNullOrWhiteSpace(traceId))
    {
        if (context.IdGenerationPolicy == TraceIdGenerationPolicy.RequireProvided)
        {
            error = ProtocolIngressResult.Failure(
                nameof(OntogonyEnvelope<object>.TraceId),
                "TraceId must be provided in the protocol event or context.");
            return null;
        }

        traceId = IdGenerator.NewId("trace");
    }

    return traceId;
}
```

### 1.3 Run Envelope Validator

Inject `IEnvelopeValidator` into base adapter:

```csharp
protected BaseProtocolIngressAdapter(
    PayloadHasher payloadHasher,
    IIdGenerator idGenerator,
    IClock clock,
    IEnvelopeValidator envelopeValidator)
{
    PayloadHasher = payloadHasher ?? throw new ArgumentNullException(nameof(payloadHasher));
    IdGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
    Clock = clock ?? throw new ArgumentNullException(nameof(clock));
    EnvelopeValidator = envelopeValidator ?? throw new ArgumentNullException(nameof(envelopeValidator));
}

protected readonly IEnvelopeValidator EnvelopeValidator;
```

Add validation helper:

```csharp
protected ProtocolIngressResult ValidateAndReturnEnvelope(
    OntogonyEnvelope<RawProtocolPayload> envelope)
{
    var validationResult = EnvelopeValidator.Validate(envelope);
    
    if (!validationResult.IsValid)
    {
        var errors = validationResult.Errors
            .Select(e => new ProtocolIngressError(e.Field, e.Message))
            .ToList();
        return ProtocolIngressResult.Failure(errors.Cast<ProtocolIngressError>().ToArray());
    }

    return ProtocolIngressResult.Success(envelope);
}
```

Then all adapters call `ValidateAndReturnEnvelope` instead of `ProtocolIngressResult.Success`.

### 1.4 Normalize Sources to Absolute URIs

Each adapter normalizes its source:

```text
GenericJsonProtocol:    "generic-json://{source}"
CloudEvents:            "cloudevents://{source}"
MCP:                    "mcp://{source}"
A2A:                    "a2a://{senderId}"
AG-UI:                  "ag-ui://session/{sessionId}"
```

Helper in base:

```csharp
protected string NormalizeSourceUri(string protocol, string source)
{
    if (source.StartsWith($"{protocol}://", StringComparison.OrdinalIgnoreCase))
        return source;
    
    return $"{protocol}://{source}";
}
```

## Tests (Phase 1)

Add focused tests:

### Envelope Validator Integration
```csharp
[Fact]
public void Normalize_ProducesValidEnvelope()
{
    // Ensure every adapter produces an envelope that passes DefaultEnvelopeValidator
}

[Fact]
public void Normalize_WithInvalidSourceUri_ReturnsValidationError()
{
    // Test that adapters catch invalid source formats
}
```

### Trace ID Fallback Behavior
```csharp
[Fact]
public void Normalize_WithEmptyRawTraceId_CascadesToContextTraceId()
{
    // Empty raw trace should NOT be treated as "found"
}

[Fact]
public void Normalize_WithoutRawOrContextTraceId_GeneratesIfPolicyAllows()
{
    // Verify policy-driven generation
}
```

### Deterministic Timestamps
```csharp
[Fact]
public void Normalize_UsesFakeClock_ForDeterminism()
{
    // Verify timestamp comes from FakeClock, not wall-clock
}
```

### Source URI Normalization
```csharp
[Fact]
public void Normalize_NormalizesSourceToAbsoluteUri()
{
    // Verify mcp-agent -> mcp://mcp-agent, etc.
}
```

## Acceptance

```powershell
dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln --no-restore
dotnet test Ontogony.Platform.sln --no-build
$env:PACKAGE_VERSION="0.2.1-local"
./scripts/pack-all.ps1 -NoBuild
```

All tests pass. No regressions in other packages.

### Verification Checklist

- [ ] All adapters produce envelopes that pass `DefaultEnvelopeValidator`
- [ ] Timestamps use injected `IClock` (no wall-clock fallback)
- [ ] Empty raw trace ID cascades to context trace ID
- [ ] All source URIs follow `{protocol}://{source}` format
- [ ] Tests prove trace fallback precedence
- [ ] Tests prove deterministic timestamps via FakeClock
- [ ] No product semantics (Agentor/Athanor/Conexus) are imported

## Follow-Up (Phase 2 & 3)

Phase 2 will address P1 issues:
- Decide event type preservation vs. normalization
- Consolidate CloudEvents model
- Document hash semantics

Phase 3 will address P2 + expanded tests:
- Remove dead code
- Add advanced edge-case tests

## Boundary

PR28.1 remains **purely mechanical**. No product semantics are added. The package continues to normalize raw events into validated envelopes suitable for durable recording, without interpreting Agentor runs, Athanor canonization, or Conexus approvals.
