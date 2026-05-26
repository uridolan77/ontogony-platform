# SYSTEM-COH-001 implementation sequence

## Phase 0 — Reality audit

Repo: all repos

Tasks:

1. List current branches/SHAs.
2. Locate all system cohesion docs/scripts/tests.
3. Identify already-implemented package targets.
4. Produce `docs/evidence/SYSTEM_COH_001_CURRENT_STATE_AUDIT.md` in `allagma-dotnet`.
5. Explicitly list stale package assumptions that were already closed.

Acceptance:

- Audit names each repo and its current commit.
- Audit lists current relevant artifacts.
- Audit separates `implemented`, `needs hardening`, `missing`, `deferred`.

## Phase 1 — Acceptance matrix

Repo: `allagma-dotnet`

Create/update:

- `docs/system/SYSTEM_COHESION_ACCEPTANCE_MATRIX.md`
- `docs/system/system-cohesion-acceptance.matrix.json`

Required scenario families:

1. compatibility lock/matrix validation;
2. health/readiness;
3. completed governed run;
4. idempotent retry;
5. human gate pause/resume;
6. Kanon assistance via Conexus;
7. Conexus fallback;
8. correlation chain;
9. Evidence Spine/operator visibility;
10. replay/restart survival;
11. package-mode build;
12. real-tools blocked proof;
13. observability evidence gate.

Statuses:

```text
PASS
FAIL
DEFERRED_WITH_REASON
NOT_APPLICABLE_FOR_ALPHA
OPTIONAL_LOCAL_ONLY
```

Release mode must reject `FAIL` and unclassified omissions. It may accept explicitly justified deferrals.

## Phase 2 — Validator

Repo: `allagma-dotnet`

Add:

- `scripts/validate-system-coh-001.ps1`

Validator checks:

- required docs exist;
- required JSON parses;
- matrix scenario ids are unique;
- all required scenario ids exist;
- all deferrals include reason, owner, and next gate;
- required scripts referenced by acceptance matrix exist;
- system README links the new baseline artifacts;
- real-tools blocked references exist;
- runtime lock validator is present.

## Phase 3 — Acceptance runner

Repo: `allagma-dotnet`

Add:

- `scripts/run-system-coh-001-acceptance.ps1`

Runner should wrap existing smoke scripts rather than duplicate logic.

Suggested flags:

```powershell
-DevRoot C:\dev
-OutputDirectory artifacts/system-coh-001/<timestamp>
-IncludeKanonAssistance
-IncludeConexusFallback
-IncludeCorrelationChain
-IncludeEvidenceSpineExport
-IncludePackageMode
-RequireEvidence
-ReleaseMode
```

Runner writes:

```text
artifacts/system-coh-001/<timestamp>/summary.json
artifacts/system-coh-001/<timestamp>/README.md
```

## Phase 4 — Platform schema/contracts

Repo: `ontogony-platform`

Add or update neutral schemas/contracts:

- `docs/contracts/CROSS_SERVICE_CONTEXT_PROPAGATION_V1.md`
- `docs/schemas/ontogony-system-cohesion-summary-v1.schema.json`
- `docs/schemas/ontogony-cross-service-error-classification-v1.schema.json`

Platform owns only neutral schema semantics.

## Phase 5 — Service alignment docs

Repos: Kanon, Conexus, Frontend/UI

Add short alignment docs:

- Kanon: which SYSTEM-COH scenarios it satisfies and how.
- Conexus: model alias/fallback/model-call evidence participation.
- Frontend: Evidence Spine/operator console inspection proof.
- UI: no broad UI rewrite; only document shared components used by cohesion views if needed.

## Phase 6 — Tests

Add tests close to existing test structure.

Allagma tests:

- acceptance matrix JSON parses and scenario ids are unique;
- system README links required docs;
- real tools remain blocked;
- context propagation matrix references current helper constants;
- Conexus model purpose aliases are semantic aliases, not provider-owned SDK/routing policy.

Platform tests:

- JSON schemas are valid;
- fixture summary validates.

Frontend tests:

- operator console/Evidence Spine export fixture includes cohesion identifiers.

## Phase 7 — Closeout

Create:

```text
allagma-dotnet/docs/evidence/SYSTEM_COH_001_CLOSEOUT.md
```

Include:

- exact commands run;
- repo refs;
- scenario matrix result table;
- artifacts generated;
- known deferrals;
- next recommended package.
