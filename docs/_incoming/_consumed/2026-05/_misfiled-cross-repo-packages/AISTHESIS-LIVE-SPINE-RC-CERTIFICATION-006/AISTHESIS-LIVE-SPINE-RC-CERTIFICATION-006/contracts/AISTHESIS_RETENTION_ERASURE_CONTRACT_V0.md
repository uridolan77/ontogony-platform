# Contract — Aisthesis retention/erasure v0

## Entities

```yaml
retentionPolicyId:
erasureRequestId:
tombstoneId:
auditEvidenceId:
```

## Required behavior

- Redaction/tombstone before physical delete.
- Erasure actions emit Aisthesis-owned evidence.
- Trace bundle export includes redaction markers.
- Reconstructability reflects retention gaps distinctly from missing producer evidence.
- Legal hold prevents deletion.
