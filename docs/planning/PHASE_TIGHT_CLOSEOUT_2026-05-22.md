# Phase Tight closeout — completed PRs (2026-05-22)

Tracks **done** work from [`PHASE_TIGHT.md`](./PHASE_TIGHT.md) and cross-repo propagation / acceptance follow-ups. Planning IDs map to implementation; not every item had a separate GitHub PR number.

## Score snapshot (after closeout)

| Repo | Prior (PHASE_TIGHT) | Current | Blockers to 9.2+ |
| --- | ---: | ---: | --- |
| `ontogony-platform` | 8.5 | **9.12** | Consumer repos still adding domain gates (aliases, lifecycle, evidence graph) |
| `allagma-dotnet` | 8.7 | **9.05** | ALLAGMA-9-002 evidence graph; full acceptance with streaming+restart on demand |
| `kanon-dotnet` | 8.6 | **8.85** | KANON-9-001 lifecycle, KANON-9-002 replay acceptance |
| `conexus-dotnet` | 8.8 | **8.92** | CONEXUS-9-001 alias manifest, CONEXUS-9-002 streaming acceptance |
| `ontogony-frontend` | — | **8.35** | Unchanged this slice |
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
| ALLAGMA-9-002 | Open | Executable evidence graph acceptance | — |
| ALLAGMA-9-003 | Open | Real execution trust model (still disabled) | `docs/security/REAL_TOOL_EXECUTION_TRUST_MODEL.md` (design exists) |

**Acceptance run (2026-05-22, Quick + existing services):**

| Artifact | Verdict |
| --- | --- |
| `artifacts/system-cohesion/summary.json` | PASS |
| `artifacts/system-cohesion/run-20260522T175112Z/summary.json` | PASS (8 scenarios; streaming/restart deferred) |
| `artifacts/system-cohesion/platform-compat/system-compatibility-summary.json` | PASS |

Repo SHAs at run: platform `4b601fd`, allagma `891126c`, kanon `d985087`, conexus `c2e5813`.

## Kanon (`kanon-dotnet`)

| ID | Status | Deliverables | Evidence |
| --- | --- | --- | --- |
| KANON-PROP-001 | **Done** | Conexus assistance outbound propagation proof | `tests/Kanon.Tests/KanonConexusAssistancePropagationConformanceTests.cs` |
| KANON-9-004 | **Partial** | Assistance in system cohesion smoke | `kanon_conexus_assistance` scenario in ALLAGMA-9-001 / SYSTEM-COH-003 |
| KANON-9-001 | Open | Domain pack lifecycle hardening | DPACK-GOV exists; PHASE_TIGHT states still required |
| KANON-9-002 | Open | Semantic decision replay acceptance | REPLAY-001/002 done; full acceptance matrix open |
| KANON-9-003 | **Partial** | Error envelope via platform gate + integration docs | PLATFORM-9-002 sibling doc check |

## Conexus (`conexus-dotnet`)

| ID | Status | Deliverables | Evidence |
| --- | --- | --- | --- |
| CONEXUS-PROP-001 | **Done** | Provider outbound propagation + ingress/outbound split doc | `tests/Conexus.Providers.OpenAI.Tests/ConexusOutboundPropagationConformanceTests.cs`; `BOUNDARIES.md` |
| CONEXUS-9-001 | Open | Model alias manifest | — |
| CONEXUS-9-002 | Open | Streaming evidence acceptance pack | Streaming exists; formal acceptance gate open |

## Sprint mapping (PHASE_TIGHT roadmap)

| Sprint ID | Maps to | Status |
| --- | --- | --- |
| SYSTEM-9A-001 | PLATFORM-9-001 | Done |
| SYSTEM-9A-002 | ALLAGMA-9-001 | Done |
| SYSTEM-9C-002 | PLATFORM-9-002 | Done |
| SYSTEM-9C-003 | PLATFORM-9-003 | Done |
| SYSTEM-9A-003 | CONEXUS-9-001 | Open |
| SYSTEM-9A-004 | KANON-9-001 | Open |
| SYSTEM-9B-001 | ALLAGMA-9-002 | Open |

## Next recommended work

1. ALLAGMA-9-002 — executable evidence graph acceptance  
2. CONEXUS-9-001 — model alias manifest consumed by Allagma  
3. KANON-9-001 / KANON-9-002 — lifecycle + replay acceptance matrices  
4. Full ALLAGMA-9-001 with `-IncludeStreamingEvidence` and `-IncludeRestart` (PowerShell 7+)
