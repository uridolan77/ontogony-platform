# Live Release Validation Follow-ups

This package does not implement live release validation. It prepares the page to represent it honestly later.

## Future checks

A real release-candidate readiness gate should eventually include:

1. Service health contract validation for Conexus, Kanon, and Allagma.
2. Service readiness contract validation for Conexus, Kanon, and Allagma.
3. Version metadata alignment.
4. Generated OpenAPI/client freshness checks.
5. Route existence probes.
6. Auth/config smoke checks.
7. One completed live Allagma → Kanon → Conexus run.
8. Evidence Spine resolution of the live run.
9. Kanon decision/provenance lookup.
10. Conexus model-call/route-decision lookup.
11. Allagma run/audit lookup.
12. Fixture/demo data exclusion proof.

## Potential future artifact

```json
{
  "schemaVersion": "release-readiness-live.v1",
  "checkedAt": "2026-05-24T00:00:00.000Z",
  "services": {
    "conexus": { "health": "passed", "ready": "failed", "version": "0.1.0-alpha.0" },
    "kanon": { "health": "passed", "ready": "passed", "version": "0.1.0-alpha.0" },
    "allagma": { "health": "passed", "ready": "passed", "version": "0.1.0-alpha.0" }
  },
  "evidence": {
    "latestRunId": "run_...",
    "traceId": "trace_...",
    "kanonDecisionId": "decision_...",
    "conexusModelCallId": "chatcmpl_...",
    "evidenceSpineCompleteness": "complete|partial|failed"
  },
  "posture": "not_ready|candidate|passed"
}
```

## Important

Until such live validation exists, the frontend should show release posture as `Not assessed` or equivalent.
