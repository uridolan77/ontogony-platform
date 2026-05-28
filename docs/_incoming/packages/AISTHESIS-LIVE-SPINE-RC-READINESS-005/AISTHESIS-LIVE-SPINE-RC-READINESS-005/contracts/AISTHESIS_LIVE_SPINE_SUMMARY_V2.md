# Contract — Aisthesis live spine summary v2

## Schema version

```text
aisthesis.live-spine.summary.v2
```

## Required fields

```json
{
  "schemaVersion": "aisthesis.live-spine.summary.v2",
  "package": "string",
  "mode": "Fixture|Live|CI|ReleaseMode",
  "status": "PASS|FAIL|NOT_RUN",
  "recordedAtUtc": "ISO-8601",
  "traceId": "string|null",
  "correlationId": "string|null",
  "workflow": "string|null",
  "repoRefs": {},
  "services": [],
  "producersObserved": [],
  "lookupEvidenceCount": 0,
  "bundleFingerprintPresent": true,
  "reconstructability": {
    "grade": "complete|partial|weak|failed|null",
    "score": 0.0,
    "blockingFindings": 0,
    "blockingCodes": []
  },
  "requiredEdges": {
    "present": 0,
    "missing": 0,
    "ambiguous": 0,
    "notApplicable": 0
  },
  "httpChecks": {},
  "redaction": {
    "tokensIncluded": false,
    "rawPayloadsIncluded": false,
    "payloadRefsDereferenced": false
  },
  "acceptedPartial": false,
  "reason": "string|null",
  "artifactPaths": []
}
```

## Rules

- `PASS` requires no tokens or raw payloads in summary.
- `PASS` in live mode requires real producer evidence.
- Fixture mode cannot establish live proof.
- `acceptedPartial` may be true only when `blockingFindings` is 0 and a rationale exists.
