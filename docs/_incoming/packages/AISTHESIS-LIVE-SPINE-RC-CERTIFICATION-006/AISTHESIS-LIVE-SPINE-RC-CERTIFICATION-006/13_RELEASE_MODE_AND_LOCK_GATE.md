# ReleaseMode and lock gate

## ReleaseMode expectations

ReleaseMode is not production readiness. It is the minimum controlled runtime posture for RC review.

Expected:

```yaml
store: postgres preferred
idempotency: postgres preferred
authMode: producer-token
sharedTokenMode: allowed only for local transitional use
rawPayloadStorage: prohibited by default
payloadRefs: allowed if redacted and non-secret
producerTokens: configured per producer
logRedaction: enabled
fixtureEvidence: disallowed in live PASS claims
traceContext: propagated when available
```

## Lock decision fields

```yaml
package: AISTHESIS-LIVE-SPINE-RC-CERTIFICATION-006
lockRequired_current: false
recommended_next: false | candidate | true
classification: RC-certification candidate | RC-certification partial | blocked
evidence:
  releaseBuild:
  tests:
  fixtureSmoke:
  liveCertification:
  les001:
  les002:
  requiredEdgeV2:
  clientCoverage:
  edgeAuth:
  evaluationDurability:
  frontend:
  iam:
  retention:
  otel:
blockers: []
deferrals: []
rationale:
```

## Recommendation rules

- Recommend `true` only if live certification PASS and all blocking gates resolved.
- Recommend `candidate` if core gates pass but frontend/IAM/retention/OTel are accepted RC-scope deferrals.
- Recommend `false` if live evidence remains fixture-only or required edges regress.
