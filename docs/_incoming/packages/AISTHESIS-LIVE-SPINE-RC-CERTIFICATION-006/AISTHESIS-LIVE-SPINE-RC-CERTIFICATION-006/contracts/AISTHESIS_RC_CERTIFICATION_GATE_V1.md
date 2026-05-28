# Contract — Aisthesis RC certification gate v1

## Gate states

```text
PASS
FAIL
NOT_RUN
DEFERRED
ACCEPTED_PARTIAL
BLOCKER
```

## Required gate groups

```yaml
local:
  restore:
  build:
  tests:
  fixtureSmoke:
contracts:
  requiredEdgeV1:
  requiredEdgeV2:
  clientCoverage:
live:
  les001:
  les002:
  fiveServiceCertification:
security:
  producerAuth:
  edgeAuth:
  iam:
operations:
  releaseMode:
  retentionErasure:
  otel:
frontend:
  liveSpineUi:
lock:
  decision:
```

## Certification decision

```yaml
classification:
recommendedNext:
blockers:
acceptedDeferrals:
unsupportedClaims:
```
