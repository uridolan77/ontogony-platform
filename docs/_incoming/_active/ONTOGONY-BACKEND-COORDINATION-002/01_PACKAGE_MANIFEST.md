# Package manifest — ONTOGONY-BACKEND-COORDINATION-002

**ID:** `ONTOGONY-BACKEND-COORDINATION-002`  
**Title:** Ontogony Backend Coordination Sprint  
**Status:** `active`  
**Created:** 2026-05-29  
**Predecessor:** `BACKEND-REPO-CLEANUP-ORGANIZATION-001`

## Purpose

Close the six-repo cohesion gaps surfaced by the cleanup pass: docs ordering, compatibility manifest, shared errors, identity propagation, model alias discipline, system E2E, Aisthesis reconstructability spine, and Metabole data-spine hardening.

## Orchestration model

| Role | Repo | Responsibility |
| --- | --- | --- |
| **Coordination docs / contracts / schemas** | `ontogony-platform` | Parent package home, shared contracts, validation scripts |
| **Runtime lock / system matrices / E2E spine** | `allagma-dotnet` | `docs/system/*`, stack launcher, certification evidence |
| **Domain implementation** | Product repos | Slice-specific code/docs/tests |

## Child slices

| Order | Slice ID | Primary owner | Secondary |
| ---: | --- | --- | --- |
| 1 | `BACKEND-REPO-DOCS-ORDER-002` | All six backends | `ontogony-platform` (doc standard) |
| 2 | `SYSTEM-COMPATIBILITY-MATRIX-001` | `allagma-dotnet` | All repos (manifest inputs) |
| 3 | `SHARED-ERROR-CONTRACT-001` | `ontogony-platform` | All product repos (adoption) |
| 4 | `CROSS-REPO-IDENTITY-CORRELATION-001` | `ontogony-platform` | All product repos (middleware/clients) |
| 5 | `ALLAGMA-CONEXUS-MODEL-ALIAS-001` | `allagma-dotnet` | `conexus-dotnet` (alias registry) |
| 6 | `BACKEND-SYSTEM-E2E-001` | `allagma-dotnet` | All five runtime services |
| 7 | `AISTHESIS-RECONSTRUCTABILITY-SPINE-001` | `aisthesis-dotnet` | Producer repos |
| 8 | `METABOLE-DATA-SPINE-HARDENING-001` | `metabole-dotnet` | `kanon-dotnet` (SLOD handoff) |

Machine-readable: [`PACKAGE_MANIFEST.json`](./PACKAGE_MANIFEST.json)

## Repos in scope

- `ontogony-platform`
- `conexus-dotnet`
- `kanon-dotnet`
- `allagma-dotnet`
- `metabole-dotnet`
- `aisthesis-dotnet`

## Out of scope

- `ontogony-frontend` / `ontogony-ui` (operator console polish deferred unless slice explicitly requires read-only handoff)
- Production deployment, multi-region, IAM production hardening
- Kanon `/ontology/v1` graduation
- Real tool execution enablement

## Evidence convention

Each slice produces at minimum:

```text
<owner-repo>/docs/evidence/<SLICE-ID>_CLOSEOUT.md
<owner-repo>/docs/evidence/<SLICE-ID>_EVIDENCE.json   # when machine-readable proof exists
```

Parent closeout:

```text
allagma-dotnet/docs/evidence/ONTOGONY_BACKEND_COORDINATION_002_CLOSEOUT.md
ontogony-platform/docs/evidence/ONTOGONY_BACKEND_COORDINATION_002_CLOSEOUT.md
```

## Archive rule

When all slices close, move this folder to:

```text
ontogony-platform/docs/_incoming/_consumed/<YYYY-MM>/ONTOGONY-BACKEND-COORDINATION-002/
```

Add `CONSUMED.md` linking promoted canonical docs in each repo.
