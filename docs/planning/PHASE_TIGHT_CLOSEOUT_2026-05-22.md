# Phase Tight closeout — completed PRs (2026-05-22)

Tracks **done** work from [`PHASE_TIGHT.md`](./PHASE_TIGHT.md) and cross-repo propagation / acceptance follow-ups. Planning IDs map to implementation; not every item had a separate GitHub PR number.

## Score snapshot (after closeout)

| Repo | Prior (PHASE_TIGHT) | Current | Blockers to 9.2+ |
| --- | ---: | ---: | --- |
| `ontogony-platform` | 8.5 | **9.12** | Consumer repos still adding domain gates (aliases, lifecycle, evidence graph) |
| `kanon-dotnet` | 8.6 | **9.18** | KANON-9-001 lifecycle manifest; KANON-9-002 replay **9/9 PASS** |
| `conexus-dotnet` | 8.8 | **9.08** | Consumed by Allagma alias validation + streaming cohesion PASS |
| `ontogony-frontend` | — | **8.78** | SYSTEM-9B-005 + live posture docker E2E |
| `allagma-dotnet` | 8.7 | **9.25** | Streaming + restart cohesion PASS (2026-05-22 operator slice) |
| `ontogony-ui` | — | **8.05** | Unchanged this slice |

## Platform (`ontogony-platform`)

| ID | Status | Deliverables | Evidence |
| --- | --- | --- | --- |
| PLATFORM-9-001 | **Done** | `Ontogony.SystemCompatibility`, gate script, `SYSTEM_COMPATIBILITY_GATE.md` | `scripts/run-system-compatibility-gate.ps1`; `artifacts/system-compat/` |
| PLATFORM-9-002 | **Done** | Error envelope matrix, gate checks, taxonomy adapter | `docs/contracts/CROSS_SERVICE_ERROR_ENVELOPE_GATE.md`; `CrossServiceErrorEnvelopeConformance` |
| PLATFORM-9-003 | **Done** | Frozen headers, matrix, test helpers, gate checks | `docs/contracts/HEADER_PROPAGATION_CONTRACT.md`; `Ontogony.Testing.HeaderPropagationConformanceAssertions` |

**Local gate (2026-05-22):** `Category=SystemCompatGate` PASS — 19 checks, 0 failures (`artifacts/system-compat/` under platform or allagma acceptance run).

## Allagma (`allagma-dotnet`)

| ID | Status | Deliverables | Evidence |
| --- | --- | --- | --- |
| ALLAGMA-9-001 | **Done** | `scripts/system/run-system-cohesion-acceptance.ps1` | `docs/e2e/SYSTEM_COHESION_ACCEPTANCE.md`; `artifacts/system-cohesion/summary.{json,md}` |
| ALLAGMA-PROP-001 | **Done** | Outbound propagation conformance tests | `tests/Allagma.Tests/AllagmaOutboundPropagationConformanceTests.cs` |
| ALLAGMA-9-002 | **Done** | Executable evidence graph acceptance | `docs/e2e/EVIDENCE_GRAPH_ACCEPTANCE.md`; scenario `evidence_graph_acceptance` |
| ALLAGMA-9-003 | **Done** | Real execution trust acceptance (still disabled) | [`allagma-dotnet/docs/e2e/REAL_EXECUTION_TRUST_ACCEPTANCE.md`](../../allagma-dotnet/docs/e2e/REAL_EXECUTION_TRUST_ACCEPTANCE.md); `Allagma9003RealExecutionTrustModelTests` |
| ALLAGMA-9-004 | **Done** | Operator-grade runtime posture v2 | [`allagma-dotnet/docs/e2e/RUNTIME_POSTURE_ACCEPTANCE.md`](../../allagma-dotnet/docs/e2e/RUNTIME_POSTURE_ACCEPTANCE.md) |

**Acceptance run (2026-05-22, Docker `local-working-system` + `-UseExistingServices`):**

| Artifact | Verdict |
| --- | --- |
| `artifacts/system-cohesion/summary.json` | PASS |
| `artifacts/system-cohesion/run-20260522T185400Z/summary.json` | PASS (10 scenarios PASS; `restart_survival` DEFERRED) |
| `artifacts/system-cohesion/run-20260522T185400Z/streaming-summary.json` | PASS |
| `artifacts/system-cohesion/platform-compat/system-compatibility-summary.json` | PASS (22 checks) |
| `artifacts/system-cohesion/streaming-acceptance-20260522/` | PASS — streaming cohesion (evening) |
| `artifacts/system-cohesion/restart-streaming-20260522/` | PASS — post-restart cohesion + streaming |
| `artifacts/restart-e2e/2026-05-22T19-55-17/summary.json` | PASS — `docker-compose-restart-allagma-api` |

Repo SHAs at morning run: platform `45c7603`, allagma `ed5ec7c`, kanon `6f369ad`, conexus `76f10c4`. Detail: [`ALLAGMA_COH_FULL_ACCEPTANCE_2026-05-22_EVIDENCE.md`](../../allagma-dotnet/docs/evidence/ALLAGMA_COH_FULL_ACCEPTANCE_2026-05-22_EVIDENCE.md); evening slice: [`ALLAGMA_RESTART_STREAMING_COH_2026-05-22_EVIDENCE.md`](../../allagma-dotnet/docs/evidence/ALLAGMA_RESTART_STREAMING_COH_2026-05-22_EVIDENCE.md).

Prior Quick path: `run-20260522T175112Z`.

## Kanon (`kanon-dotnet`)

| ID | Status | Deliverables | Evidence |
| --- | --- | --- | --- |
| KANON-PROP-001 | **Done** | Conexus assistance outbound propagation proof | `tests/Kanon.Tests/KanonConexusAssistancePropagationConformanceTests.cs` |
| KANON-9-004 | **Partial** | Assistance in system cohesion smoke | `kanon_conexus_assistance` scenario in ALLAGMA-9-001 / SYSTEM-COH-003 |
| KANON-9-001 | **Done** | Domain pack lifecycle manifest + acceptance | `KANON_DOMAIN_PACK_LIFECYCLE_MANIFEST.json`; `DomainPackLifecycleGovernanceTests` |
| KANON-9-002 | **Done** | Semantic decision replay acceptance | `KanonSemanticDecisionReplayAcceptanceTests`; `run-semantic-decision-replay-acceptance.ps1`; operator **6/6 PASS** (2026-05-22) |
| KANON-9-003 | **Partial** | Error envelope via platform gate + integration docs | PLATFORM-9-002 sibling doc check |

## Conexus (`conexus-dotnet`)

| ID | Status | Deliverables | Evidence |
| --- | --- | --- | --- |
| CONEXUS-PROP-001 | **Done** | Provider outbound propagation + ingress/outbound split doc | `tests/Conexus.Providers.OpenAI.Tests/ConexusOutboundPropagationConformanceTests.cs`; `BOUNDARIES.md` |
| CONEXUS-9-001 | **Done** | Model alias manifest + Allagma startup validation | `docs/generated/CONEXUS_MODEL_ALIAS_MANIFEST.json`; `allagma-dotnet/docs/system/conexus-model-alias-manifest.snapshot.json` |
| CONEXUS-9-002 | **Done** | Streaming evidence acceptance pack + Allagma cohesion | `ConexusStreamingEvidenceAcceptanceTests`; [`ALLAGMA_RESTART_STREAMING_COH_2026-05-22_EVIDENCE.md`](../../allagma-dotnet/docs/evidence/ALLAGMA_RESTART_STREAMING_COH_2026-05-22_EVIDENCE.md) |

## Sprint mapping (PHASE_TIGHT roadmap)

| Sprint ID | Maps to | Status |
| --- | --- | --- |
| SYSTEM-9A-001 | PLATFORM-9-001 | Done |
| SYSTEM-9A-002 | ALLAGMA-9-001 | Done |
| SYSTEM-9C-002 | PLATFORM-9-002 | Done |
| SYSTEM-9C-003 | PLATFORM-9-003 | Done |
| SYSTEM-9A-003 | CONEXUS-9-001 | Done |
| SYSTEM-9B-002 | KANON-9-002 | Done |
| SYSTEM-9B-003 | CONEXUS-9-002 | Done — Conexus pack + Allagma streaming cohesion + Docker restart |
| SYSTEM-9B-005 | FE live-artifact E2E | Done |
| SYSTEM-9A-004 | KANON-9-001 | Done |
| SYSTEM-9A-005 | FE route-client drift gate | Done — `SYSTEM_9A_005_ROUTE_CLIENT_DRIFT_GATE_EVIDENCE.md`; `ontogony-frontend` `route-client-drift:check` |
| SYSTEM-9B-001 | ALLAGMA-9-002 | Done |

**Operator slice (2026-05-22 evening):** CONEXUS-9-002 script PASS; Allagma streaming cohesion (`streaming-acceptance-20260522`, `restart-streaming-20260522`); canonical `docker compose restart allagma-api` + `artifacts/restart-e2e/2026-05-22T19-55-17/summary.json`. Record: [`allagma-dotnet/docs/evidence/ALLAGMA_RESTART_STREAMING_COH_2026-05-22_EVIDENCE.md`](../../allagma-dotnet/docs/evidence/ALLAGMA_RESTART_STREAMING_COH_2026-05-22_EVIDENCE.md).

## Next recommended work

1. **ontogony-ui** — UI-HARDEN-001–004 consolidation  
2. **Allagma** — install **PowerShell 7** (`pwsh`) so `run-system-cohesion-acceptance.ps1` bundles platform gate + streaming + trust in one artifact dir  
3. Optional: wire `runtime-posture-docker-live.spec.ts` into `live-artifact-evidence-journey:check` catalog  
4. **AG UI interaction spine** — next cross-repo slice per incoming package (`ontogony_ag_ui_interaction_spine_package_2026-05-22`)
