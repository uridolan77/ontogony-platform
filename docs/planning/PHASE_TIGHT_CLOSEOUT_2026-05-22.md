# Phase Tight closeout — completed PRs (2026-05-22)

Tracks **done** work from [`PHASE_TIGHT.md`](./PHASE_TIGHT.md) and cross-repo propagation / acceptance follow-ups. Planning IDs map to implementation; not every item had a separate GitHub PR number.

## Score snapshot (after closeout)

| Repo | Prior (PHASE_TIGHT) | Current | Blockers to 9.2+ |
| --- | ---: | ---: | --- |
| `ontogony-platform` | 8.5 | **9.12** | Consumer repos still adding domain gates (aliases, lifecycle, evidence graph) |
| `allagma-dotnet` | 8.7 | **9.22** | `-IncludeStreamingEvidence` / `-IncludeRestart` cohesion paths still deferred |
| `kanon-dotnet` | 8.6 | **9.02** | KANON-9-001 lifecycle; KANON-9-002 replay **9/9 PASS** |
| `conexus-dotnet` | 8.8 | **9.08** | CONEXUS-9-001 manifest; CONEXUS-9-002 pack in repo; Allagma streaming cohesion rerun |
| `ontogony-frontend` | — | **8.68** | SYSTEM-9B-005 live-artifact E2E; live posture E2E still open |
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

Repo SHAs at run: platform `45c7603`, allagma `ed5ec7c`, kanon `6f369ad`, conexus `76f10c4`. Detail: [`allagma-dotnet/docs/evidence/ALLAGMA_COH_FULL_ACCEPTANCE_2026-05-22_EVIDENCE.md`](../../allagma-dotnet/docs/evidence/ALLAGMA_COH_FULL_ACCEPTANCE_2026-05-22_EVIDENCE.md).

Prior Quick path: `run-20260522T175112Z`.

## Kanon (`kanon-dotnet`)

| ID | Status | Deliverables | Evidence |
| --- | --- | --- | --- |
| KANON-PROP-001 | **Done** | Conexus assistance outbound propagation proof | `tests/Kanon.Tests/KanonConexusAssistancePropagationConformanceTests.cs` |
| KANON-9-004 | **Partial** | Assistance in system cohesion smoke | `kanon_conexus_assistance` scenario in ALLAGMA-9-001 / SYSTEM-COH-003 |
| KANON-9-001 | Open | Domain pack lifecycle hardening | DPACK-GOV exists; PHASE_TIGHT states still required |
| KANON-9-002 | **Done** | Semantic decision replay acceptance | `KanonSemanticDecisionReplayAcceptanceTests`; `run-semantic-decision-replay-acceptance.ps1`; operator **6/6 PASS** (2026-05-22) |
| KANON-9-003 | **Partial** | Error envelope via platform gate + integration docs | PLATFORM-9-002 sibling doc check |

## Conexus (`conexus-dotnet`)

| ID | Status | Deliverables | Evidence |
| --- | --- | --- | --- |
| CONEXUS-PROP-001 | **Done** | Provider outbound propagation + ingress/outbound split doc | `tests/Conexus.Providers.OpenAI.Tests/ConexusOutboundPropagationConformanceTests.cs`; `BOUNDARIES.md` |
| CONEXUS-9-001 | **Done** | Model alias manifest + Allagma startup validation | `docs/generated/CONEXUS_MODEL_ALIAS_MANIFEST.json`; `allagma-dotnet/docs/system/conexus-model-alias-manifest.snapshot.json` |
| CONEXUS-9-002 | **Done** (Conexus) | Streaming evidence acceptance pack | `ConexusStreamingEvidenceAcceptanceTests`; `CONEXUS_9_002_STREAMING_EVIDENCE_ACCEPTANCE_EVIDENCE.md`; Allagma `-IncludeStreamingEvidence` cohesion rerun open |

## Sprint mapping (PHASE_TIGHT roadmap)

| Sprint ID | Maps to | Status |
| --- | --- | --- |
| SYSTEM-9A-001 | PLATFORM-9-001 | Done |
| SYSTEM-9A-002 | ALLAGMA-9-001 | Done |
| SYSTEM-9C-002 | PLATFORM-9-002 | Done |
| SYSTEM-9C-003 | PLATFORM-9-003 | Done |
| SYSTEM-9A-003 | CONEXUS-9-001 | Done |
| SYSTEM-9B-002 | KANON-9-002 | Done |
| SYSTEM-9B-003 | CONEXUS-9-002 | Done (Conexus pack); Allagma cohesion streaming flag open |
| SYSTEM-9B-005 | FE live-artifact E2E | Done |
| SYSTEM-9A-004 | KANON-9-001 | Open |
| SYSTEM-9A-005 | FE route-client drift gate | Done — `SYSTEM_9A_005_ROUTE_CLIENT_DRIFT_GATE_EVIDENCE.md`; `ontogony-frontend` `route-client-drift:check` |
| SYSTEM-9B-001 | ALLAGMA-9-002 | Done |

## Next recommended work

1. **Allagma** `run-system-cohesion-acceptance.ps1 -IncludeStreamingEvidence` (CONEXUS-9-002 already packaged in Conexus)  
2. **Allagma** `-IncludeRestart` (restart survival / idempotency sibling gate)  
3. **KANON-9-001** — domain pack lifecycle hardening manifest  
4. **Frontend** — live runtime posture E2E against Docker stack (SYSTEM-9B-005 evidence journey done)  
5. **ontogony-ui** — UI-HARDEN-001–004 consolidation
