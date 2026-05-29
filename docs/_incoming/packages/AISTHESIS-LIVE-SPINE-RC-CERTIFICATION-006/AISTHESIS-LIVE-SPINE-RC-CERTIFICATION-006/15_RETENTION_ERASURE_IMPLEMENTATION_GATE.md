# Retention / erasure implementation gate

## Why this is special for Aisthesis

Aisthesis is an evidence spine. Deleting evidence changes reconstructability. Therefore destructive deletion must be modeled, audited, and reflected in trace bundles.

## Minimum semantics

- Envelope retention.
- Edge retention.
- Evaluation-run retention.
- Bundle export retention.
- PayloadRef handling.
- Tombstone behavior.
- Erasure audit evidence.
- Legal hold override.
- Reconstructability after erasure.

## Preferred first implementation

Do not start with physical delete. Start with tombstone/redaction:

```text
envelope retained as tombstone
edge retained but marks redacted endpoint if needed
payloadRef removed/redacted
bundle export shows redaction marker
reconstructability emits retention/erasure diagnostic
retention action emits Aisthesis-owned evidence
```

## Contract fields

```yaml
retentionPolicyId:
retentionClass:
expiresAtUtc:
legalHold: true|false
erasureRequestId:
tombstone: true|false
redactionReason:
auditEvidenceId:
```

## Closeout classification

```yaml
implemented: true|false
tombstoneSupported: true|false
physicalDeleteSupported: true|false
productionBlocker: true|false
```
