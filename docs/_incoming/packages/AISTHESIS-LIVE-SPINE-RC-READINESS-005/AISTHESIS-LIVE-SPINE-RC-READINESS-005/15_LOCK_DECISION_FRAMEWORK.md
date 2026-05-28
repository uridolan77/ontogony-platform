# Aisthesis lock decision framework

## Purpose

Decide whether Aisthesis can move toward `lockRequired: true` or remain unlocked.

## Decision schema

```yaml
package: AISTHESIS-LIVE-SPINE-RC-READINESS-005
date:
currentLockRequired: false
recommendedLockRequired: false | candidate | true
classification: RC-ready candidate | RC-readiness partial | blocked
evidence:
  aisthesisBuild:
  aisthesisTests:
  fixtureSmoke:
  les001:
  les002:
  ciSmoke:
  releaseMode:
  producerRepoGates:
  frontend:
  iam:
  retention:
  otel:
decision:
rationale:
blockers:
deferredNonBlockers:
nextPackage:
```

## Recommended default

Do not set `lockRequired: true` in this package unless:

- full clean Release gates pass;
- five-service CI smoke passes;
- ReleaseMode smoke passes;
- live evidence proof is repeatable;
- no high-severity deferrals remain.

A safer outcome is `recommendedLockRequired: candidate` or `recommendedLockRequired: false` with exact blockers.
