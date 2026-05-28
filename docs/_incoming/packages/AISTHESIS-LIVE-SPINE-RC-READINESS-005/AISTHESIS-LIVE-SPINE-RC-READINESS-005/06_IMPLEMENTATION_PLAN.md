# Implementation plan

## Slice A — Intake and baseline truth

1. Unpack package.
2. Confirm Aisthesis 003A state:
   - tests pass;
   - fixture smoke complete;
   - required-edge evaluator has 10 rules;
   - bundle fingerprint stable.
3. Record baseline in `AISTHESIS_LIVE_SPINE_RC_READINESS_005_RELEASE_GATES.md`.

## Slice B — Full clean local Release gates

Stop all running API processes that may lock `bin/Release`.

Run:

```powershell
dotnet restore
dotnet build Aisthesis.sln -c Release
dotnet test Aisthesis.sln -c Release --no-build
```

Record exact results.

If tests fail, stop the package and fix regressions before touching cross-repo smoke.

## Slice C — Aisthesis fixture smoke

Run:

```powershell
./scripts/run-aisthesis-live-spine-smoke.ps1 -StartApi
./scripts/system/run-five-service-aisthesis-live-smoke.ps1 -Mode Fixture -StartApi
```

Expected:

```json
{
  "status": "PASS",
  "requiredEdges": { "present": 10, "missing": 0 },
  "reconstructabilityGrade": "complete"
}
```

Record artifact paths.

## Slice D — Add RC readiness script

Add `scripts/system/run-aisthesis-rc-readiness.ps1`.

It should orchestrate:

1. build/test;
2. local smoke;
3. fixture five-service smoke;
4. summary JSON write to `artifacts/aisthesis-rc-readiness/<timestamp>/summary.json`.

The script must not fake live proof. It may record live proof as `NOT_RUN`.

## Slice E — Add CI-compatible five-service smoke template

Add `scripts/system/run-five-service-ci-smoke.ps1`.

The script should:

- probe all five services;
- run readiness checks;
- optionally trigger configured live workflow if environment variables are set;
- query Aisthesis reconstructability, lookup, and bundle;
- write a redacted summary;
- exit non-zero only when the CI gate is expected to run and fails.

It must support a dry/preflight mode.

## Slice F — LES-001 / LES-002 live proof discipline

Update `AISTHESIS_LIVE_SPINE_RC_READINESS_005_LIVE_PROOF.md`.

Include:

- LES-001 artifact and status;
- LES-002 artifact and status;
- score, grade, blockers, producersObserved;
- whether LES-002 partial grade is fixed or accepted.

## Slice G — ReleaseMode gate

Define a ReleaseMode runbook:

- auth mode;
- Postgres mode;
- producer tokens;
- no in-memory-only assumptions;
- idempotency behavior;
- no demo/fake evidence;
- log redaction;
- failure behavior.

## Slice H — Lock decision

Create `AISTHESIS_LIVE_SPINE_RC_READINESS_005_LOCK_DECISION.md`.

Decision fields:

```yaml
lockRequired_current: false
recommended_next: false | candidate | true
evidence:
  tests:
  fixtureSmoke:
  liveProof:
  ciSmoke:
  releaseMode:
  frontend:
  iam:
  retention:
  otel:
decision:
rationale:
blockers:
```

## Slice I — Platform closeout

In `ontogony-platform`, add platform closeout and SYSTEM-RC-003 lock review doc.

Do not claim full RC if gates remain partial.

## Slice J — Final closeout

Use `18_CLOSEOUT_TEMPLATE.md`.
