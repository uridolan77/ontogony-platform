# Decision reconstructability protocol (v0)

**Protocol family:** cross-service decision evidence and governance classification.  
**Semantic authority:** Kanon (`kanon-dotnet`) — classifier, governance rules, and ontology HTTP surfaces.  
**Export owners:** Allagma, Conexus, and Kanon each project `DecisionEvent` fragments; the operator console assembles and displays reports.

This document is the platform contract index. Product repos implement exporters and HTTP; platform owns the **shape**, redaction expectations, and cross-repo acceptance references.

---

## Contract documents

| Document | Purpose |
| --- | --- |
| [DECISION_EVENT_SCHEMA_V0.md](DECISION_EVENT_SCHEMA_V0.md) | Normalized `DecisionEvent` cross-service view |
| [RECONSTRUCTABILITY_REPORT_V0.md](RECONSTRUCTABILITY_REPORT_V0.md) | `ReconstructabilityReport`, F/P/S/O, strict score, PASS/WARN/FAIL |
| [MISSING_EVIDENCE_DIAGNOSTIC_V0.md](MISSING_EVIDENCE_DIAGNOSTIC_V0.md) | Actionable missing-evidence diagnostics |
| [DECISION_RECONSTRUCTABILITY_HTTP_V0.md](DECISION_RECONSTRUCTABILITY_HTTP_V0.md) | Implemented HTTP surfaces (2026-05-26) |

Related product contracts:

| Repo | Document |
| --- | --- |
| Conexus | `conexus-dotnet/docs/contracts/CONEXUS_DECISION_EVENTS_V1.md` |
| Closure program | [`docs/evidence/ONTOGONY_RECONSTRUCTABILITY_CLOSURE_OPTION1_CLOSEOUT.md`](../evidence/ONTOGONY_RECONSTRUCTABILITY_CLOSURE_OPTION1_CLOSEOUT.md) |
| Conformance kits | [`docs/adoption/reconstructability-conformance-kits.md`](../adoption/reconstructability-conformance-kits.md) |

---

## F/P/S/O classification (summary)

| Code | Meaning | Score |
| --- | --- | --- |
| `F` | fully_fillable | 1.0 |
| `P` | partially_fillable | 0.5 |
| `S` | structurally_unfillable | 0.0 |
| `O` | opaque | 0.0 |

`desStrictCompletenessPct = round(100 * sum(property scores) / 7, 1)` over the seven governed properties. High/critical severities enforce stricter required-`F` rules — see [RECONSTRUCTABILITY_REPORT_V0.md](RECONSTRUCTABILITY_REPORT_V0.md).

---

## Golden fixtures and local validation

| Artifact | Location |
| --- | --- |
| Golden decision events | [`fixtures/decision-reconstructability/`](../fixtures/decision-reconstructability/) |
| DEC-RECON-004 API smoke wrapper | [`scripts/validate-decision-reconstructability-local.ps1`](../../scripts/validate-decision-reconstructability-local.ps1) |
| Cross-service golden trace (orchestrator) | `allagma-dotnet/docs/evidence/CROSS_SERVICE_RECONSTRUCTABILITY_GOLDEN_TRACE.md` |

---

## Operator console (frontend)

Deep-link routes (PR-006) render backend classification only — no client-side F/P/S/O grading:

```text
/allagma/runs/:runId/reconstructability
/conexus/model-calls/:modelCallId/reconstructability
/kanon/reconstructability/:decisionEventId
/system/evidence-spine/:traceId/reconstructability
```

Evidence Spine graph integration (DEC-RECON-006) uses classify-batch during Allagma run resolution — see archived intake `ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-006`.

---

## Intake archive

Parent program package: [`docs/_incoming/_consumed/2026-05/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001/`](../_incoming/_consumed/2026-05/ONTOGONY-DECISION-RECONSTRUCTABILITY-SPINE-001/).
