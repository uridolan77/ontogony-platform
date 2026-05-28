# Contract — Aisthesis live spine summary v3

## Required top-level fields

```json
{
  "schemaVersion": "aisthesis.live-spine.summary.v3",
  "package": "AISTHESIS-LIVE-SPINE-RC-CERTIFICATION-006",
  "mode": "Preflight|Fixture|Live|LiveOrExplain",
  "status": "PASS|FAIL|NOT_RUN|PARTIAL",
  "classification": "RC-certification candidate|RC-certification partial|Blocked|Not evaluated",
  "traceId": null,
  "workflowProfile": null,
  "services": {},
  "producersObserved": [],
  "requiredEdges": {
    "v1": { "present": 0, "missing": 0, "ambiguous": 0, "notApplicable": 0 },
    "v2": { "present": 0, "missing": 0, "ambiguous": 0, "notApplicable": 0 }
  },
  "reconstructability": {
    "grade": null,
    "score": null,
    "blockingFindings": 0,
    "blockingCodes": [],
    "diagnostics": []
  },
  "bundle": {
    "bundleFingerprintPresent": false,
    "bundleFingerprint": null,
    "exportedAtUtc": null
  },
  "redaction": {
    "tokensIncluded": false,
    "rawPayloadsIncluded": false,
    "payloadRefsDereferenced": false
  },
  "gates": {},
  "deferrals": [],
  "reason": null,
  "artifacts": [],
  "recordedAtUtc": "..."
}
```

## Truth rules

- `status = PASS` in Live mode requires live services and live producer evidence.
- Fixture evidence cannot produce Live PASS.
- `NOT_RUN` must include reason.
- `PARTIAL` must include missing/accepted-partial rationale.
- Redaction flags must never be omitted.
