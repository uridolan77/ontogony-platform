# Allagma actionability workbench — next options

After ACTION-007 closeout, browser QA, and Docker `allagma-api` rebuild, consider these follow-ups (not committed).

| ID | Theme | Rationale |
| --- | --- | --- |
| **ACTION-007A** | Execute browser manual QA checklist | Record results in frontend 007 evidence; flip 007 verdict to PASS |
| **ACTION-007B** | Fix Docker allagma build TLS + redeploy | Unblocks live operations contract QA against 5083 |
| **ACTION-008** | Human-gate deny UX polish | Kanon-only deny; clearer pairing with resume outcomes |
| **ACTION-009** | Manual evaluation operator workbench | Backend exists but policy-gated; needs explicit non-prod harness |
| **EVIDENCE-SPINE-002** | Allagma adapters in evidence spine | Links run/eval/audit nodes across services |
| **ALLAGMA-ACTION-010** | Failed-run seed fixture for Docker QA | Deterministic retry/cancel/replay demos without harness scripts |

## Recommended order

1. **007B** — rebuild `allagma-api` in `docker/local-working-system` (resolve NuGet SSL in Docker build).
2. **007A** — run browser checklist at `http://localhost:5175/allagma/…`.
3. Choose **008** vs **009** based on operator interviews (gates vs eval authoring).
4. Schedule **EVIDENCE-SPINE** work separately — do not expand run-detail links into an ad hoc spine.
