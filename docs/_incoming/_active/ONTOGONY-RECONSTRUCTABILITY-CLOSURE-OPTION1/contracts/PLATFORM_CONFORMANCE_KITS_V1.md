# Platform Conformance Kits v1

## Purpose

Platform should prove that consumers use shared mechanics consistently.

## Required kits

### Correlation/header propagation

Consumers:

```text
allagma-dotnet
kanon-dotnet
conexus-dotnet
```

Checks:

```text
trace id accepted/created
correlation id propagated
known headers preserved
forbidden headers not propagated
```

### Error envelope

Checks:

```text
CrossServiceErrorEnvelope used for operator-visible cross-service errors
stage/system/retryable fields set
trace id included where available
```

### Idempotency

Checks:

```text
same key + same payload -> replay/consistent response
same key + different payload -> conflict
missing key rejected where mutation contract requires it
```

### Observability naming

Checks:

```text
meters use repo-owned meter name
counter/histogram/gauge naming conventions followed
no high-cardinality labels
```

### Artifact/export safety

Checks:

```text
exports declare schema
exports declare generatedAtUtc
secrets and raw provider payloads are redacted
```

## Platform must not know product semantics

The kits should validate shape and mechanics, not event-kind meaning.
