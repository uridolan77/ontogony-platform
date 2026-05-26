# Repo task — allagma-dotnet

Allagma is the lead repo for SYSTEM-COH-001.

## Main responsibility

Own the canonical cohesion acceptance baseline for the governed runtime.

## Required work

1. Audit existing system docs:
   - `docs/system/README.md`
   - `SYSTEM_COMPATIBILITY_MATRIX.md`
   - `SYSTEM_ENVIRONMENT_MATRIX.md`
   - `SYSTEM_AUTH_MATRIX.md`
   - `SYSTEM_ROUTE_MATRIX.md`
   - `SYSTEM_TEST_MATRIX.md`
   - `SYSTEM_RUNTIME_CORRECTNESS_MATRIX.md`
   - `SYSTEM_TRACE_CONTEXT_MATRIX.md`
   - `ONTOGONY_RUNTIME_LOCK.md`
   - `ontogony-runtime.lock.json`
2. Add `SYSTEM_COHESION_BASELINE.md` as the closure/acceptance summary, not a duplicate matrix.
3. Add `system-cohesion-acceptance.matrix.json` and `SYSTEM_COHESION_ACCEPTANCE_MATRIX.md`.
4. Add `SYSTEM_ERROR_COMPATIBILITY_MATRIX.md`.
5. Add `SYSTEM_OBSERVABILITY_EVIDENCE_GATE.md`.
6. Add `validate-system-coh-001.ps1`.
7. Add `run-system-coh-001-acceptance.ps1`.
8. Add or extend tests for docs/matrix/real-tools-blocked/context propagation.
9. Create `docs/evidence/SYSTEM_COH_001_CLOSEOUT.md` after validation.

## Critical implementation notes

- Do not duplicate lower-level smoke logic already in `scripts/lib/system-cohesion-e2e.ps1` or first-real-system helpers.
- Do not enable real tools.
- Do not move Conexus model routing into Allagma.
- Keep model purposes semantic and alias-based.
- Preserve privacy rule: actor headers go to Kanon, not Conexus.
- Release mode should be stricter than local advisory mode.

## Tests to add or extend

Suggested tests:

- `SystemCohesionAcceptanceMatrixTests`
- `SystemCohesionDocsTests`
- `SystemCohesionRealToolsBlockedTests`
- `SystemCohesionErrorCompatibilityTests`
- `SystemCohesionContextPropagationDocsTests`

## Done when

Allagma can produce one closeout file and one machine summary that tell an operator exactly what is alpha-closed, what is deferred, and why.
