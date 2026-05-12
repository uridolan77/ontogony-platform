# PR33 — Ontogony.Testing Conformance Kits

## Goal

Let consumer services prove they adopted platform mechanics correctly.

## Scope

Expand `Ontogony.Testing` with conformance fixtures:

```csharp
TracingConformanceAssertions
ErrorShapeConformanceAssertions
EnvelopeConformanceAssertions
HmacConformanceAssertions
OutboxConformanceHarness
HttpResilienceConformanceHarness
```

## Examples

A service should be able to write:

```csharp
await TracingConformanceAssertions.AssertCanonicalTraceHeaderAsync(app);
await ErrorShapeConformanceAssertions.AssertNoUnmappedExceptionLeakAsync(app);
```

## Must not do

- Do not require services to use identical domain exception classes.
- Do not require a specific database provider.
- Do not couple to Agentor/Athanor.

## Tests

- Self-tests for each conformance helper.
- Negative tests proving helper fails on wrong behavior.

## Docs

- `docs/testing/conformance-kits.md`
- service adoption snippets

## Acceptance

Agentor/Athanor can later import these helpers without changing platform code.
