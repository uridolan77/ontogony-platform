# Retention / erasure gate

## Why this matters

Aisthesis is an evidence spine. Retention changes reconstructability, auditability, compliance, and replay behavior.

## Required policy dimensions

1. Envelope retention.
2. Edge retention.
3. Bundle export retention.
4. Evaluation-run retention.
5. Payload reference retention.
6. Redaction / tombstone behavior.
7. Erasure audit trail.
8. Reconstructability after erasure.
9. Legal hold override.
10. Environment-specific defaults.

## Minimum RC gate

Add a document with:

```yaml
defaultRetentionDays:
erasureSupported:
tombstoneSupported:
payloadRefsDereferenced:
legalHold:
reconstructabilityAfterErasure:
productionBlocker:
```

Do not implement destructive deletion APIs in this package unless tombstone semantics, audit trail, tests, and recovery are defined.
