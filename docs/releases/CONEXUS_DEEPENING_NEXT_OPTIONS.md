# Conexus deepening — next options

After 007 closeout and browser QA, consider these follow-ups (not committed).

| ID | Theme | Rationale |
| --- | --- | --- |
| **CONEXUS-DEEPEN-008** | Durable operator audit / request search | Admin audit list exists; no full-text or multi-field search across historical requests |
| **CONEXUS-DEEPEN-009** | Structured evidence spine integration | Replace per-call evidence-links with platform `EVIDENCE-SPINE-*` resolver when scheduled |
| **CONEXUS-DEEPEN-010** | Real-provider failure fixtures | Repeatable local scenarios for fallback chains in observability UI |
| **EVIDENCE-SPINE-001** | Unified cross-service evidence graph | Supersedes identifier-only links from 005 |
| **UI-HARDEN** | Observability density pass | Shared tables/empty states if tabbed Conexus pages need further primitive alignment |

## Recommended order

1. Execute **007 browser manual QA** and record results.
2. Fix repo-wide frontend typecheck debt blocking CI confidence.
3. Choose **008** (search/history) vs **009** (spine) based on operator interviews — do not implement spine ad hoc inside Conexus.
4. Update `conexus-dotnet/docs/operations/KNOWN_LIMITATIONS.md` § product surface when browser QA confirms frontend observability is the supported operator path.

## Out of scope (remain gateway boundaries)

- Raw prompt/completion operator review UI (policy + redaction must lead).
- Kanon semantic authority features (Kanon repo).
- Allagma orchestration UX (Allagma repo).
- Production Grafana/dashboards pack (operations program).
