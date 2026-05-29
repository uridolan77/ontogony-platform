# Implementation plan

## Slice 0 — Preflight and truth sync

1. Stop local APIs if they may interfere with clean Release tests.
2. Inspect current `aisthesis-dotnet` main.
3. Read current README, status, closeouts, generated route inventory, client coverage, required-edge matrix v1, and current scripts.
4. Confirm whether RC-readiness-005 was already applied. Do not duplicate files; upgrade them.
5. Record baseline in `AISTHESIS_LIVE_SPINE_RC_CERTIFICATION_006_RELEASE_GATES.md`.

## Slice 1 — Preserve 005 gates

Keep the 005 gates intact:

- restore/build/test;
- fixture smoke;
- LES-001/LES-002 proof discipline;
- ReleaseMode expectations;
- CI-compatible smoke script;
- lock decision framework;
- IAM/retention/OTel gates.

Rename or supersede docs only if needed. Never delete useful 005 evidence.

## Slice 2 — Required-edge matrix v2

Create `AISTHESIS_REQUIRED_EDGE_MATRIX_V2.md`.

Add edge families for:

- Allagma run → human gate;
- Allagma run → tool intent;
- Allagma tool intent → tool execution;
- Allagma tool execution → side effect;
- Allagma side effect → output artifact;
- Kanon decision → canonical fact;
- Kanon decision → source binding;
- Kanon decision → replay bundle;
- Kanon human gate opened → human gate resolved;
- Conexus model call → provider fallback;
- Conexus model call → provider error;
- Conexus model call → usage/cost record;
- Conexus streaming session → completion summary;
- Metabole pipeline → schema extract;
- Metabole data profile → mapping candidate;
- Metabole mapping candidate → review;
- Metabole review → artifact;
- Metabole transformation plan → output artifact;
- Metabole mapping candidate/source artifact → Kanon source-binding proposal where applicable.

Add profile semantics:

```text
core-run-profile
metabole-first-profile
human-gated-run-profile
tool-side-effect-profile
model-fallback-profile
streaming-model-profile
```

Do not require every v2 edge for every trace. Use `requiredWhen` and `notApplicableReason`.

## Slice 3 — V2 fixtures and evaluator tests

Add golden and missing-edge fixtures for matrix v2.

At minimum:

- a complete core-run v2 fixture;
- a Metabole-first v2 fixture;
- a missing-edge v2 fixture that proves diagnostics include `suggestedProducerFix`.

Acceptance:

```text
requiredEdges.v2.missing = 0 for complete profile
requiredEdges.v2.missing > 0 for missing fixture
diagnostics identify producer owner and native ID fix
```

## Slice 4 — Live five-service certification

Upgrade the 005 CI smoke into `run-five-service-live-certification.ps1`.

Modes:

```powershell
-Mode Preflight
-Mode Fixture
-Mode Live
-Mode LiveOrExplain
```

Rules:

- Preflight may PASS without services.
- Fixture may PASS against Aisthesis only.
- Live may PASS only if all five services are ready and a configured trigger produces evidence.
- LiveOrExplain may return NOT_RUN with a precise reason.
- Never fake PASS.

Summary schema must match `AISTHESIS_LIVE_SPINE_SUMMARY_V3.md`.

## Slice 5 — LES-001 / LES-002 proof discipline

Create/update live proof doc.

For LES-002, either:

1. fix missing producer evidence or scoring profile until complete; or
2. document accepted partial with exact dimensions, required-edge status, blockers=0, and owner/date for future remediation.

No vague partial acceptance.

## Slice 6 — Aisthesis.Client coverage completion

Compare route inventory to client coverage.

If evaluation routes are public, add client methods for:

```text
POST /aisthesis/v0/evaluation/traces/{traceId}/runs
GET /aisthesis/v0/evaluation/runs/{evaluationRunId}
GET /aisthesis/v0/evaluation/traces/{traceId}/runs
GET /aisthesis/v0/evaluation/traces/{traceId}/runs/latest
```

If they are server-only, update generated coverage truth to classify them explicitly.

Acceptance:

```text
no public route exists outside client coverage unless marked serverOnly/internal
```

## Slice 7 — Durable evaluation/Krisis

Inspect current evaluation flow.

If evaluation is still fire-and-forget after envelope ingestion, implement or plan a durable job layer:

- `EvaluationJob`;
- status: pending/running/succeeded/failed/retry_exhausted;
- in-memory store;
- Postgres store/migration if within scope;
- bounded retry;
- failure evidence;
- latest evaluation remains queryable.

If too large for this package, produce a blocker/deferral doc with exact risk.

## Slice 8 — Producer and edge authorization hardening

Direct edge writes must not be weaker than envelope/batch writes.

Options:

1. require edge writes through batch with `producerSystem`;
2. add producerSystem to direct edge request;
3. add relation-scoped ACLs;
4. only wildcard/system tokens may direct-write edges.

Implement one option or record a blocker.

## Slice 9 — ReleaseMode and lock decision

Define ReleaseMode expectations:

- Postgres mode preferred;
- producer-token auth enabled;
- no fake/demo evidence;
- payload redaction defaults;
- no in-memory-only lock claims;
- exact env vars documented;
- failure semantics documented.

Update lock decision:

```yaml
lockRequired_current: false
recommended_next: false | candidate | true
classification: RC-certification candidate | RC-certification partial | blocked
```

## Slice 10 — IAM, retention, OTel gates

For each gate, produce one of:

```text
implemented
accepted non-blocking deferral
production blocker
```

Do not leave as ambiguous docs-only gap.

## Slice 11 — Frontend validation handoff

Create a route-compatible frontend contract:

- timeline;
- graph;
- reconstructability v2 diagnostics;
- required edge status;
- producer/native-ID lookup;
- bundle export;
- auth handling;
- missing-edge producer fix display.

If frontend is not modified, produce a strict prompt and close as handoff-only.

## Slice 12 — Producer rechecks

Run producer rechecks only if live certification fails or matrix v2 identifies missing evidence.

Expected owner mapping:

```text
allagma.* -> allagma-dotnet
kanon.* -> kanon-dotnet
conexus.* -> conexus-dotnet
metabole.* -> metabole-dotnet
```

## Slice 13 — Platform lock review

In `ontogony-platform`, add system-level RC lock review docs linking:

- Aisthesis closeout;
- live certification summary;
- producer evidence;
- frontend status;
- IAM/retention/OTel decisions;
- lock recommendation.

## Slice 14 — Final closeout

Use `22_CLOSEOUT_TEMPLATE.md`.

Do not close as RC-certification candidate unless evidence supports it.
