# Contract — ReleaseMode expectations

## ReleaseMode means

- no dev-only fake evidence;
- no demo traces unless explicitly marked;
- token auth enabled or explicitly waived;
- durable storage or explicit in-memory waiver;
- redacted summaries;
- deterministic evidence IDs where producers own identity;
- reconstructability failures are visible.

## Required readiness

```text
GET /health
GET /ready
GET /aisthesis/v0/capabilities
```

## Required read checks

```text
GET /aisthesis/v0/evidence/lookup?traceId=...
GET /aisthesis/v0/evidence/traces/{traceId}/timeline
GET /aisthesis/v0/evidence/traces/{traceId}/graph
GET /aisthesis/v0/evidence/traces/{traceId}/reconstructability/v2
GET /aisthesis/v0/evidence/traces/{traceId}/bundle
```
