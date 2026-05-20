# Cross-service evidence spine — next options

After EVIDENCE-SPINE-009 closeout, consider these follow-ups (not committed). Ordered by operator impact.

| ID | Theme | Rationale |
| --- | --- | --- |
| **EVIDENCE-SPINE-009A** | Docker-live browser QA | Run `run-evidence-spine-docker-local-verification.ps1 -Build` against seeded stack; record real IDs in evidence |
| **EVIDENCE-SPINE-010** | Dataset/scenario resolver roots | Taxonomy already lists kinds; wire `GET /allagma/v0/evaluation-datasets/{id}` and eval list filters |
| **EVIDENCE-SPINE-011** | `includeExports` expansion | Attach `allagma.auditBundle` and `allagma.evidenceExport` nodes when flags set |
| **EVIDENCE-SPINE-012** | Human gate direct lookup | Requires Allagma or Kanon GET-by-gate-id contract—avoid list scans |
| **EVIDENCE-SPINE-013** | Consolidate trace correlation UI | Single lookup bar on System/Conexus using spine parser; deprecate four-field bar |
| **EVIDENCE-SPINE-014** | Graph visualization v2 | Optional canvas view for large graphs within `maxNodes` cap |
| **EVIDENCE-SPINE-015** | Server-side resolve preview API | Optional read-only aggregator for CI/replay—not semantic authority |
| **EVIDENCE-SPINE-016** | Correlation-first expansion | Stronger `correlationId` root algorithm when trace is absent |

## Recommended order

1. **009A** — prove live stack with operator-recorded IDs (low code, high confidence).
2. **010** — completes eval-operator journeys without workbench dead-ends.
3. **011** — export flags match operator expectation from run/eval detail panels.
4. **012** — only after API contract exists in `allagma-dotnet` or `kanon-dotnet`.
5. **013–014** — UX polish once resolver coverage is stable.
6. **015–016** — architectural; schedule only if client-side limits block operators.

## Do not mix with

- **ALLAGMA-ACTION-008+** — consequential mutations belong in actionability workbench, not spine resolver.
- **Platform semantic APIs** — spine must remain mechanical orchestration over existing GET contracts.
