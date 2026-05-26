# 11 — Test and acceptance plan

## Acceptance criteria

REPLAY-RUNTIME-001 is accepted when:

- current-state audit remains accurate after local repo inspection;
- replay modes are consistently defined and used;
- replay eligibility explains exact/dry-run/deterministic/reconstructed/evidence-only/unavailable status;
- no replay flow silently executes real external providers;
- no replay flow silently executes real external tools;
- existing Allagma replay endpoint remains compatible;
- at least one governed fake end-to-end replay path is designed and preferably implemented;
- replay results connect to Evidence Spine;
- replay artifacts are exportable and redacted;
- frontend replay workbench uses canonical UI patterns;
- all route/API changes pass contract discipline;
- existing governed fake E2E remains valid;
- runtime-lock artifacts are not broken;
- no existing evidence/truth features are removed.

## Backend commands

### Platform

```powershell
cd C:\dev\ontogony-platform
dotnet test
pwsh ./scripts/validate-system-evidence-spine-contract.ps1
pwsh ./scripts/check/check-contract-discipline.ps1
pwsh ./scripts/smoke/run-runtime-lock-governed-fake-e2e.ps1
```

### Allagma

```powershell
cd C:\devllagma-dotnet
dotnet test
pwsh ./scripts/smoke/run-governed-fake-e2e.ps1
pwsh ./scripts/check-governed-fake-e2e-summary.ps1
```

After replay smoke is added:

```powershell
pwsh ./scripts/smoke/run-governed-fake-replay-e2e.ps1
pwsh ./scripts/check-governed-fake-replay-summary.ps1
```

### Kanon

```powershell
cd C:\dev\kanon-dotnet
dotnet test
pwsh ./scripts/run-semantic-decision-replay-acceptance.ps1
pwsh ./scripts/run-semantic-decision-replay-live-acceptance.ps1
```

Run live acceptance only when local services/config are ready.

### Conexus

```powershell
cd C:\dev\conexus-dotnet
dotnet test
pwsh ./scripts/run-idempotency-acceptance.ps1
```

After Conexus replay/dry-run endpoints are added:

```powershell
# names to be added during implementation
pwsh ./scripts/run-replay-dry-run-acceptance.ps1
```

## Frontend commands

```powershell
cd C:\dev\ontogony-frontend
npm install
npm run typecheck
npm run test -- --run
npx playwright test e2e/evidence-spine-docker-live.spec.ts
```

After Replay Workbench E2E is added:

```powershell
npx playwright test e2e/replay-workbench.spec.ts
```

## UI commands

```powershell
cd C:\dev\ontogony-ui
npm install
npm run typecheck
npm run test -- --run
npm run build
node ./scripts/smoke-named-exports.mjs
```

## Contract discipline commands

Run the repo-specific discipline scripts and the frontend generated route/client checks. Exact command names may differ by repo; do not skip the underlying checks.

Minimum:

```powershell
# platform
pwsh C:\dev\ontogony-platform\scripts\check\check-contract-discipline.ps1

# frontend
cd C:\dev\ontogony-frontend
npm run contracts:discipline
```

If `contracts:discipline` has a different current script name, inspect `package.json` and use the active command.

## Test cases to add

### Allagma

- `ReplayEligibilityClassifierTests`
- `ReplayAgentRunServiceEvidenceOnlyTests`
- `ReplayRecordRepositoryTests`
- `ReplayOrchestrationServiceKanonSkippedTests`
- `ReplayOrchestrationServiceConexusSkippedTests`
- `ReplayEvidenceBundleBuilderTests`
- `ReplayDeltaBuilderTests`

### Kanon

- `KanonReplayEligibilityTests`
- `DecisionReplayBundleSummaryTests`
- `SemanticPlanReplayEligibilityTests`
- `ReplayEndpointRouteInventoryTests`

### Conexus

- `ConexusReplayEligibilityTests`
- `RouteDecisionDryRunReplayTests`
- `ModelCallDryRunReplayBlocksRealProviderTests`
- `FakeProviderDeterministicSimulationTests`
- `ProviderAttemptEvidenceOnlyReplayTests`

### Frontend

- `ReplayRootLookup.test.tsx`
- `ReplayEligibilitySummary.test.tsx`
- `ReplayModeSelector.test.tsx`
- `ReplayPreviewPanel.test.tsx`
- `ReplayDeltaComparison.test.tsx`
- `ReplayEvidenceLinksPanel.test.tsx`

## Manual verification checklist

- Paste a valid Allagma run ID into Evidence Spine.
- Confirm graph resolves to Allagma/Kanon/Conexus nodes when evidence exists.
- Open Replay Workbench from the graph.
- Confirm exact replay is unavailable unless proven.
- Confirm real provider/tool execution is blocked.
- Run evidence-only replay.
- Export bundle.
- Open replay result from Evidence Spine link.
- Confirm raw attempts are collapsed by default.
- Confirm existing Evidence Spine and Agent Interaction pages still work.
