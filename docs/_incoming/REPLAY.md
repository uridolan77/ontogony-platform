Below is the plan to take replay from “implemented and useful” to **fully integrated, scored >9 across backend, frontend, contracts, smoke, and operator UX**.

Current state is strong but incomplete in three places: backend already has merged eligibility and downstream attempts, but Conexus route-decision dry-run is not inside Allagma orchestration yet; frontend still does not surface `serviceEligibilities`; and the governed-fake replay smoke exists but needs live PASS/runtime-lock/CI integration. The current backend already delegates replay eligibility to `CrossServiceReplayEligibilityService`, and replay creation collects downstream attempts after Allagma replay succeeds.  The coordinator currently handles Kanon replay bundles and Conexus model-call dry-run, while explicitly leaving Conexus route-decision dry-run outside Allagma orchestration.  The smoke path for governed fake replay exists and validates resolve → eligibility → create replay → bundle → delta, with safety checks.  

# Target score

```text
Replay integration target:
  Cross-service eligibility merge:      9.5/10
  Cross-service replay attempts:        9.2/10
  Kanon replay integration:             9.3/10
  Conexus replay integration:           9.2/10
  Replay artifact discipline:           9.5/10
  Runtime-lock / CI proof maturity:      9.0/10
  Frontend service-eligibility display:  9.3/10
  Overall:                              9.2+/10
```

---

# Package name

```text
REPLAY-RUNTIME-FULL-INTEGRATION-001
```

This should be one ambitious package with six slices. The slices can be implemented sequentially but committed as one coherent replay-integration closeout.

---

# Slice 1 — Make Allagma the canonical replay orchestrator

## Goal

All replay should have one canonical operator-facing path:

```text
/allagma/replay
  → Allagma replay resolve
  → Allagma merged eligibility
  → Allagma replay request
  → Allagma evidence bundle
  → Allagma replay delta
  → downstream service attempts:
       allagma
       kanon
       conexus.model_call
       conexus.route_decision where resolvable
```

Conexus direct replay remains available as diagnostic detail, but the canonical chain should be Allagma-owned.

## Backend work — `allagma-dotnet`

### 1.1 Add route-decision downstream attempt to `CrossServiceReplayCoordinator`

Current coordinator calls Conexus model-call dry-run only. Extend it to:

```text
modelCallId
  → read/derive routeDecisionId if available
  → call Conexus route-decision dry-run
  → add separate ReplayServiceAttempt:
       service: conexus
       operation: route_decision_dry_run
       target: conexus.route_decision:{routeDecisionId}
```

Current comment says route-decision dry-run is not invoked here. That should be updated once implemented. 

### 1.2 Extend `IConexusReplayRuntimeClient`

Add:

```csharp
Task<ConexusRouteDecisionReplayDryRunResponse?> TryDryRunRouteDecisionAsync(
    string routeDecisionId,
    ConexusRouteDecisionReplayDryRunRequest request,
    CancellationToken cancellationToken);
```

Existing `ConexusReplayRuntimeClient` already has eligibility and model-call dry-run. Add route-decision dry-run using:

```text
POST /admin/v0/replay/route-decisions/{id}/dry-run
```

### 1.3 Resolve route decision robustly

Route-decision id can come from:

```text
1. AgentRun.ModelCallId → Conexus model-call detail → routeDecisionId
2. run.Context routeDecisionId
3. AgentRunEvent payload routeDecisionId
4. extracted audit refs
```

Add a small extractor:

```text
AgentRunAuditReferenceExtractor:
  DecisionIds
  ModelCallIds
  RouteDecisionIds
```

### 1.4 Normalize replay target kind vocabulary

Today Conexus frontend uses `conexus.model_call` / `conexus.route_decision`, while Evidence Spine tends to use labels like `conexus.modelCall` / `conexus.routeDecision`. Define one replay contract vocabulary in `Ontogony.Replay.Contracts`:

```text
allagma.run
kanon.decision
kanon.provenance
kanon.semantic_plan
conexus.model_call
conexus.route_decision
conexus.provider_attempt
```

Then document mapping to Evidence Spine display kinds, instead of mixing the two.

## Acceptance

```text
- Allagma replay result can contain:
    allagma.run_replay_manifest
    kanon.decision_replay_bundle
    conexus.model_call_dry_run
    conexus.route_decision_dry_run
- Conexus route-decision dry-run is optional but represented honestly:
    succeeded / skipped / failed
- No real provider execution.
- Safety posture remains forbid_real_providers / forbid_real_tools.
```

---

# Slice 2 — Promote merged service eligibility into the frontend

## Goal

The backend now returns:

```csharp
ReplayEligibilityResponse(
  ReplayEligibility Eligibility,
  IReadOnlyList<ReplayServiceEligibilityEntry>? ServiceEligibilities
)
```

but the frontend currently maps only root `eligibility`. The UI must show the service-level breakdown. The backend contract already exposes `ServiceEligibilities`.  

## Frontend work — `ontogony-frontend`

### 2.1 Extend adapters

Update:

```text
src/allagma/adapters/allagmaReplayOrchestrationAdapters.ts
```

Add:

```ts
type ReplayServiceEligibilityViewModel = {
  ownerService: "allagma" | "kanon" | "conexus" | string;
  targetKind: string;
  targetIdentifier: string;
  status: "resolved" | "skipped" | "failed" | string;
  detail?: string | null;
  recommendedModeLabel: string;
  eligibleModeLabels: string[];
  unavailableReasonLabels: string[];
  missingSourceData: string[];
  safety: {
    realProvidersBlocked: boolean;
    realToolsBlocked: boolean;
  };
};
```

Update the root orchestration VM:

```ts
type ReplayOrchestrationViewModel = {
  eligibility: ReplayEligibilityViewModel | null;
  serviceEligibilities: ReplayServiceEligibilityViewModel[];
  result: ReplayRuntimeResultViewModel | null;
  bundle: ReplayRuntimeEvidenceBundleDto | null;
  delta: ReplayDeltaDto | null;
};
```

### 2.2 Update hooks

`useAllagmaReplayEligibility` should return:

```text
root eligibility
service eligibility breakdown
isMerged: true when serviceEligibilities exist
```

### 2.3 Update `AllagmaReplayOrchestrationPanel`

Default visible:

```text
Merged replay eligibility:
  Root: eligible / partial / unavailable
  Mode: Evidence only / Reconstructed / Dry run / unavailable
  Safety: real providers blocked / real tools blocked
```

Disclosure:

```text
Service eligibility breakdown
  Allagma
  Kanon
  Conexus model-call
  Conexus route-decision
```

Each service row should show:

```text
owner service
target
status
recommended mode
eligible modes
missing source data
detail reason
```

### 2.4 Avoid duplicate Conexus diagnostic panel confusion

Keep `ConexusReplayPosturePanel`, but label it as:

```text
Direct Conexus diagnostic replay
```

The merged Allagma replay panel should be primary. Conexus direct panel should be secondary/diagnostic.

## Acceptance

```text
- /allagma/replay displays merged eligibility and per-service eligibility.
- Operators can see why a replay is unavailable.
- Operators can see whether Conexus was skipped because not configured, target not found, or unreachable.
- Root eligibility and service eligibility cannot contradict silently.
```

---

# Slice 3 — Improve replay artifacts and runtime lock

## Goal

REPLAY-RUNTIME-005 has smoke infrastructure and artifact writer. Now make the replay proof part of the system’s canonical acceptance evidence.

The artifact writer already creates `governed-fake-replay-summary.json`, replay request/result/bundle/delta, and markdown summary. 

## Work

### 3.1 Add platform baseline artifact

In `ontogony-platform`:

```text
docs/evidence/artifacts/governed-fake-replay-e2e/<timestamp>/
  governed-fake-replay-summary.json
  replay-request.json
  replay-result.json
  replay-evidence-bundle.json
  replay-delta.json
  replay-summary.md
```

### 3.2 Extend runtime lock

In `allagma-dotnet/ontogony-runtime.lock.json` or the platform runtime lock, add:

```json
{
  "evidence": {
    "governedFakeReplaySummary": {
      "schema": "ontogony-governed-fake-replay-summary-v1",
      "path": "docs/evidence/artifacts/governed-fake-replay-e2e/<timestamp>/governed-fake-replay-summary.json",
      "required": false,
      "stage": "manual-ci"
    }
  }
}
```

### 3.3 Add validation flag

Extend:

```text
scripts/validate-runtime-lock.ps1
```

with:

```powershell
-RequireGovernedFakeReplayEvidence
```

Validate:

```text
schema
verdict PASS
sourceRun.runId
replay.replayId
replay.mode == evidence_only
safety.realProvidersBlocked == true
safety.realToolsBlocked == true
replay.serviceAttemptStatuses.allagma == succeeded
replay.serviceAttemptStatuses.kanon == succeeded
optional conexus status check
artifact files exist
```

### 3.4 Platform smoke wrapper

Add:

```text
ontogony-platform/scripts/smoke/run-runtime-lock-governed-fake-replay-e2e.ps1
```

It should:

```text
- start/wait docker stack if requested
- run system truth smoke
- run governed fake replay smoke
- copy artifacts into platform evidence folder
- optionally validate runtime lock
```

## Acceptance

```text
- A fresh local run produces canonical replay artifacts.
- Platform can validate the artifact.
- Runtime lock has opt-in replay evidence gate.
- Summary schema is stable.
```

---

# Slice 4 — Contract discipline and OpenAPI closure

## Goal

Remove transitional ambiguity around replay routes and DTOs.

## Work

### 4.1 Allagma OpenAPI

Ensure these are generated and snapshot-aligned:

```text
POST /allagma/v0/replay/resolve
POST /allagma/v0/replay/eligibility
POST /allagma/v0/replay/requests
GET  /allagma/v0/replay/requests/{id}
GET  /allagma/v0/replay/requests/{id}/bundle
GET  /allagma/v0/replay/requests/{id}/delta
```

Ensure schemas include:

```text
ReplayServiceEligibilityEntry
ReplayEligibilityResponse.ServiceEligibilities
ReplayRuntimeResult.ServiceAttempts
ReplayRuntimeEvidenceBundle.ServiceAttempts
ReplayDelta
```

### 4.2 Conexus OpenAPI

Eliminate the current frontend manual DTO shim for Conexus admin replay if possible.

Add Conexus OpenAPI coverage for:

```text
POST /admin/v0/replay/eligibility
POST /admin/v0/replay/model-calls/{id}/dry-run
POST /admin/v0/replay/route-decisions/{id}/dry-run
GET  /admin/v0/replay/model-calls/{id}/evidence
```

Current frontend Conexus replay types are explicitly handwritten until Conexus OpenAPI includes admin replay schemas. 

### 4.3 Regenerate frontend schemas and route usage

Run:

```text
npm run contracts:discipline
npm run client-routes:check
```

Target outcome:

```text
Conexus replay routes:
  usesGeneratedSchema: true
  manualDto: false
```

or, if intentionally deferred:

```text
manualDto: true
exception documented with expiry package
```

### 4.4 Route workflow catalog

Update:

```text
route-workflow-catalog.json
API_CLIENT_ROUTE_USAGE.json
MANUAL_DTO_SHIM_REGISTER
```

## Acceptance

```text
- No hidden replay route drift.
- All Allagma replay contracts are generated.
- Conexus replay manual DTOs are either removed or explicitly timeboxed.
- contracts:discipline passes.
```

---

# Slice 5 — Evidence Spine and Agent Interaction integration

## Goal

Replay should appear as part of the evidence graph, not only as JSON blobs.

## Work

### 5.1 Evidence Spine node kinds

Add/confirm:

```text
replay.request
replay.result
replay.evidence_bundle
replay.delta
replay.service_attempt
```

Edges:

```text
allagma.run → replay.request
replay.request → replay.result
replay.result → replay.evidence_bundle
replay.result → replay.delta
replay.result → replay.service_attempt
replay.service_attempt → kanon.decision
replay.service_attempt → conexus.modelCall
replay.service_attempt → conexus.routeDecision
```

### 5.2 Evidence resolver

If user enters:

```text
replay_...
replay-result:...
replay-evidence-bundle:...
```

Evidence Spine should resolve:

```text
replay request
service attempts
source run
Kanon decision
Conexus model call
route decision
bundle/delta
```

### 5.3 Agent Interaction

Replay should add an event/timeline marker:

```text
Allagma.RunReplayRequested
Replay request created
Replay completed
Service attempts:
  Allagma
  Kanon
  Conexus
```

The smoke already checks for `Allagma.RunReplayRequested` on the run.  Make this visible in Agent Interaction.

## Acceptance

```text
- Evidence Spine can start from replayId.
- /allagma/replay has “Open replay in Evidence Spine.”
- Agent Interaction shows replay event timeline.
- Replay artifacts and service attempts are navigable.
```

---

# Slice 6 — Full proof suite: unit, integration, smoke, Playwright

## Goal

Get replay to “I trust this” maturity.

## Backend tests

Add/confirm:

```text
ReplayEligibilityMergerTests
CrossServiceReplayEligibilityServiceTests
CrossServiceReplayCoordinatorTests
ReplayRuntimeServiceTests
ReplayTargetResolverTests
ConexusReplayRuntimeClientTests with fake HttpMessageHandler
```

Existing merger and coordinator tests are present.  

Add tests for:

```text
- Allagma run with Kanon + Conexus model call + route decision
- Conexus not configured → skipped, not failed
- Conexus unreachable → failed with reason
- Kanon unavailable → merged unavailable or partial according to policy
- route-decision dry-run skipped if routeDecisionId missing
- safety policy forbids real provider/tool
```

## Frontend tests

Add:

```text
AllagmaReplayOrchestrationPanel.test.tsx
Replay service eligibility breakdown tests
ConexusReplayPosturePanel route decision tests
Evidence Spine replay resolver tests
Agent Interaction replay event tests
```

## E2E

Add Playwright:

```text
e2e/replay-runtime-full-integration.spec.ts
```

Flow:

```text
1. open /allagma/replay?runId=<fixture>
2. verify merged eligibility
3. expand service eligibility disclosure
4. create evidence-only replay
5. verify result status
6. expand bundle/delta disclosure
7. open Evidence Spine from replay
8. verify replay nodes
```

## Smoke

Run:

```powershell
powershell -NoProfile -File .\scripts\smoke\run-governed-fake-replay-e2e.ps1
```

Strict variant:

```powershell
powershell -NoProfile -File .\scripts\smoke\run-governed-fake-replay-e2e.ps1 -RequireConexusReplayAttempt
```

## Acceptance

```text
- backend unit tests pass
- frontend typecheck passes
- contracts:discipline passes
- console-ui:check passes
- governed fake replay smoke PASS
- optional strict Conexus replay smoke PASS when admin key configured
- artifact copied to platform evidence
- runtime lock replay validation passes
```

---

# Final acceptance definition

Replay is “fully integrated” only when all of these are true:

```text
1. /allagma/replay is the canonical operator replay surface.
2. Allagma resolves replay targets authoritatively.
3. Allagma returns merged root eligibility.
4. Allagma returns per-service eligibility.
5. Allagma replay request creates a replay result with service attempts.
6. Kanon replay eligibility and bundle preparation are included.
7. Conexus model-call dry-run is included.
8. Conexus route-decision dry-run is included when routeDecisionId resolves.
9. No real provider/tool execution is possible in replay smoke.
10. Evidence bundle and delta are retrievable and linked.
11. Evidence Spine can resolve replay artifacts.
12. Agent Interaction shows replay events.
13. Governed fake replay smoke produces PASS artifact.
14. Runtime lock can validate that artifact.
15. Contract discipline covers every replay route.
```

---

# Recommended package sequence inside the work

```text
REPLAY-RUNTIME-FULL-INTEGRATION-001A
  Backend route-decision attempt + target vocabulary + tests.

REPLAY-RUNTIME-FULL-INTEGRATION-001B
  Frontend serviceEligibilities + merged replay UI.

REPLAY-RUNTIME-FULL-INTEGRATION-001C
  Evidence Spine + Agent Interaction replay nodes/events.

REPLAY-RUNTIME-FULL-INTEGRATION-001D
  OpenAPI / generated schemas / contract discipline cleanup.

REPLAY-RUNTIME-FULL-INTEGRATION-001E
  Governed fake replay PASS artifact + runtime lock + platform wrapper.
```

Do not start `RUNTIME-CONFIG-001` until this is done. Replay is now close enough to become a flagship system proof; finishing it will give you a much stronger foundation than moving sideways.
