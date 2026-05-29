# Contract — Aisthesis RC readiness gate v0

## Gate result

```json
{
  "schemaVersion": "aisthesis.rc-readiness-gate.v0",
  "classification": "RC-ready candidate|RC-readiness partial|Blocked",
  "lockRecommendation": "false|candidate|true",
  "gates": {
    "build": "PASS|FAIL|NOT_RUN",
    "tests": "PASS|FAIL|NOT_RUN",
    "fixtureSmoke": "PASS|FAIL|NOT_RUN",
    "liveProofLes001": "PASS|FAIL|NOT_RUN",
    "liveProofLes002": "PASS|FAIL|NOT_RUN|ACCEPTED_PARTIAL",
    "ciSmoke": "PASS|FAIL|NOT_RUN",
    "releaseMode": "PASS|FAIL|NOT_RUN",
    "frontend": "PASS|FAIL|HANDOFF_ONLY|NOT_RUN",
    "iam": "PASS|DEFERRED|FAIL",
    "retention": "PASS|DEFERRED|FAIL",
    "otel": "PASS|DEFERRED|FAIL"
  },
  "blockers": [],
  "deferredNonBlockers": [],
  "evidence": []
}
```

## Classification rules

- Any failed Aisthesis build/test/fixture gate => `Blocked`.
- LES-001 failure => `Blocked`.
- CI smoke missing => at most `RC-readiness partial`.
- ReleaseMode missing => at most `RC-readiness partial`.
- Production IAM/retention/OTel deferred => may still be `RC-ready candidate` only if explicitly outside RC lock scope.
