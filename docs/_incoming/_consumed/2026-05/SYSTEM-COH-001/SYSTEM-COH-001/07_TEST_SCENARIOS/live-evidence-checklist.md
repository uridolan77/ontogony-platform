# Live evidence checklist

For every live run, capture:

- timestamp UTC;
- repo refs;
- service base URLs;
- health/readiness snapshots;
- request body with sensitive fields redacted;
- `runId`;
- `traceId`;
- `correlationId`;
- Kanon decision ids;
- Conexus model call ids;
- route decision ids where available;
- Allagma event sequence;
- downstream audit references;
- Evidence Spine graph/export if available;
- command output;
- failure/deferral reason.

Save under:

```text
artifacts/system-coh-001/<timestamp>/
```

and summarize in:

```text
artifacts/system-coh-001/<timestamp>/summary.json
```
