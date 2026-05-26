# SYSTEM-COH-001 acceptance criteria

## Hard acceptance

SYSTEM-COH-001 is accepted only if all of the following are true:

1. **Canonical ownership is explicit.** Allagma owns the cohesion matrix/lock, Platform owns neutral schemas, Kanon/Conexus own service-specific alignment, Frontend/UI own operator visibility.
2. **No stale gap is implemented twice.** Existing matrices, runtime locks, streaming/model-purpose docs, and E2E helpers are reused and hardened.
3. **Acceptance matrix exists.** `system-cohesion-acceptance.matrix.json` contains every required scenario family.
4. **Validator exists.** `validate-system-coh-001.ps1` fails on missing required docs/scripts/JSON and unclassified deferrals.
5. **Runner exists.** `run-system-coh-001-acceptance.ps1` writes a normalized summary artifact.
6. **Context propagation is checked.** Trace/correlation/run/idempotency/actor handling is documented and tested or linked to conformance tests.
7. **Error compatibility is classified.** Public service-specific error shapes are documented, with Allagma translation rules where applicable.
8. **Evidence Spine/operator proof is represented.** The baseline includes an operator visibility scenario, even if live UI proof is deferred.
9. **Real tools remain blocked.** A test/doc link proves real side-effect tool execution is not enabled by accident.
10. **Closeout report exists.** It lists commands run and exact repo refs.

## Required scenario ids

The acceptance matrix must include at least:

```text
compatibility_lock
service_health_readiness
governed_run_complete
idempotent_run_retry
human_gate_pause_resume
kanon_conexus_assistance
conexus_fallback
correlation_chain
evidence_spine_operator_visibility
restart_replay_survival
package_mode_build
real_tools_blocked
observability_evidence_gate
```

## Allowed scenario statuses

```text
PASS
FAIL
DEFERRED_WITH_REASON
NOT_APPLICABLE_FOR_ALPHA
OPTIONAL_LOCAL_ONLY
```

Release mode rules:

- `FAIL` is never accepted.
- `DEFERRED_WITH_REASON` must include `reason`, `owner`, and `nextGate`.
- `OPTIONAL_LOCAL_ONLY` is not accepted for hard-required scenario ids unless explicitly downgraded by the closeout report.
- Missing scenario ids fail validation.

## Minimum closeout command list

At minimum, closeout must document the result of:

```powershell
./scripts/validate-runtime-lock.ps1 -ReleaseMode
./scripts/validate-system-coh-001.ps1 -ReleaseMode
dotnet test Allagma.sln -c Release
```

When available, also include:

```powershell
./scripts/run-system-coh-001-acceptance.ps1 -ReleaseMode -RequireEvidence
./scripts/architecture-conformance/run-cross-repo-conformance.ps1
./scripts/run-package-mode-build.ps1
dotnet test tests/Allagma.ArchitectureConformance.Tests -c Release
npm run contracts:discipline       # ontogony-frontend, if relevant
```
