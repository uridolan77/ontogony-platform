# PLATFORM-SYSTEM-LEARNING-GUIDE-001 — Canonical learning path

**Package:** `SYSTEM-LEARNING-GUIDE-001`  
**Status:** PASS — canonical learning index and Phase 1–2 guides shipped under `docs/learn/`.

## Delivered

| Item | Location |
| --- | --- |
| Learning index | [`docs/learn/INDEX.md`](../learn/INDEX.md) |
| Start / architecture / local run / governed E2E | `00`–`03` |
| System truth, Evidence Spine, Agent Interaction | `04`–`06` |
| Boundaries, contract discipline | `07`–`08` |
| Extension guides | `09`–`14` |
| Console UX canonicalization | `15` |
| Glossary + audit matrix | `GLOSSARY.md`, `DOCS_AUDIT_MATRIX.md` |
| Validator | [`scripts/validate-learn-docs.ps1`](../../scripts/validate-learn-docs.ps1) |

## Verify

```powershell
cd C:\dev\ontogony-platform
.\scripts\validate-learn-docs.ps1
dotnet test tests/Ontogony.Infrastructure.Tests --filter FullyQualifiedName~SystemLearningGuideDocsTests --no-restore
```

## Acceptance mapping

| Criterion | How met |
| --- | --- |
| Start local system from docs | [02_RUN_LOCAL_SYSTEM.md](../learn/02_RUN_LOCAL_SYSTEM.md) |
| Run governed fake E2E from docs | [03_GOVERNED_FAKE_E2E.md](../learn/03_GOVERNED_FAKE_E2E.md) |
| Domain vs alias vs route | [07_DOMAIN_MODEL_ROUTING_BOUNDARIES.md](../learn/07_DOMAIN_MODEL_ROUTING_BOUNDARIES.md) |
| API route change checklist | [11_ADD_OR_CHANGE_AN_API_ROUTE.md](../learn/11_ADD_OR_CHANGE_AN_API_ROUTE.md) |
| Stale docs identified | [DOCS_AUDIT_MATRIX.md](../learn/DOCS_AUDIT_MATRIX.md) |
| Generated docs linked not copied | [08_CONTRACT_DISCIPLINE.md](../learn/08_CONTRACT_DISCIPLINE.md) |

## Phase 4–5 (follow-up)

- Apply historical markers to rows in audit matrix (no mass delete in this package).
- Optional: wire `validate-learn-docs.ps1` into platform CI when convenient.

## Related

- Incoming spec: `docs/_incoming/SYSTEM-LEARNING-GUIDE-001.zip`
- `allagma-dotnet/docs/evidence/AGM_REPLAY_RUNTIME_PROOF_LOCK_001.md` (prior roadmap item)
