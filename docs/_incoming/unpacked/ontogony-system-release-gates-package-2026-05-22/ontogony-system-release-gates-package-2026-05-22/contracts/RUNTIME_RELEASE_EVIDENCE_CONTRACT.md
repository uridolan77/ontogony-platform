# Runtime release evidence contract

## Purpose

Defines the JSON and Markdown evidence emitted by SYSTEM-ALPHA-007.

## Required top-level fields

```json
{
  "schema": "ontogony-runtime-release-evidence-v1",
  "releaseId": "SYSTEM-ALPHA-007",
  "mode": "Locked",
  "startedAtUtc": "2026-05-22T00:00:00Z",
  "completedAtUtc": "2026-05-22T00:00:00Z",
  "verdict": "PASS",
  "runtimeLock": {},
  "repositories": {},
  "repoGates": {},
  "packageMode": {},
  "systemCohesion": {},
  "capacityBaseline": {},
  "restartSurvival": {},
  "streamingSmoke": {},
  "evidencePolicy": {},
  "artifacts": []
}
```

## Verdict values

| Value | Meaning |
|---|---|
| `PASS` | All required gates passed |
| `FAIL` | At least one required gate failed |
| `INCONCLUSIVE` | Prerequisite missing or optional environment unavailable |
| `DRIFT_ONLY` | Moving-main run; not release evidence |

## Release evidence policy

A valid release bundle must satisfy:

- `mode == "Locked"`;
- four repo SHAs match runtime lock;
- all required gates have `verdict == "PASS"`;
- no required scenario is `SKIPPED`;
- package-mode summary exists;
- system-cohesion summary exists;
- no secret-like values appear in JSON.

## Secret redaction

Evidence writers must redact values matching:

```text
sk-*
sk-or-*
Bearer *
Password=*
ApiKey=*
ServiceToken=*
CONEXUS_ADMIN_API_KEY
CONEXUS_PROVIDER_*
```
