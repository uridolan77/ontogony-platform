# Acceptance matrix

| ID | Criterion | Evidence |
|---|---|---|
| PLAT-MECH-001 | Mechanics-only proposal gate doc exists and defines reuse test | `docs/governance/PLATFORM_MECHANICS_ONLY_PROPOSAL_GATE.md` |
| PLAT-MECH-002 | Proposal-gate schema validates accepted/rejected examples | `schemas/mechanics/v1/platform-proposal-gate.schema.json`, fixtures |
| PLAT-MECH-003 | Script fails proposals that import product meaning | `scripts/governance/check-platform-mechanics-only.ps1` |
| PLAT-MECH-004 | Mechanical schema registry v1 exists | `docs/contracts/MECHANICAL_SCHEMA_REGISTRY_V1.md` |
| PLAT-MECH-005 | Core schemas exist: error, headers, evidence-ref, idempotency, replay, actor | `schemas/mechanics/v1/*.schema.json` |
| PLAT-MECH-006 | Schema registry test validates all schemas and fixtures | `scripts/conformance/Test-MechanicalSchemaRegistry.ps1` |
| PLAT-MECH-007 | Header propagation conformance harness exists | `scripts/conformance/Test-HeaderPropagationConformance.ps1` |
| PLAT-MECH-008 | Error envelope conformance harness exists | `scripts/conformance/Test-ErrorEnvelopeConformance.ps1` |
| PLAT-MECH-009 | Idempotency conformance harness exists | `scripts/conformance/Test-IdempotencyConformance.ps1` |
| PLAT-MECH-010 | Outbox/artifact conformance harness exists | `scripts/conformance/Test-OutboxArtifactConformance.ps1` |
| PLAT-MECH-011 | Consumer conformance suite emits summary JSON | `artifacts/platform-mechanics-conformance/<stamp>/summary.json` |
| PLAT-MECH-012 | Observability meter naming conformance exists | `scripts/conformance/Test-ObservabilityMeterNaming.ps1` |
| PLAT-MECH-013 | No-product-semantics conformance runs in Platform and sibling mode | `scripts/conformance/Test-NoProductSemantics.ps1` |
| PLAT-MECH-014 | All harnesses support `-RepoRoot`, `-ConsumerName`, and `-OutputDirectory` | script help + smoke evidence |
| PLAT-MECH-015 | Consumer task cards exist for all five backend consumers | `repo-tasks/*.md` |
| PLAT-MECH-016 | Closeout explicitly states non-claims and deferrals | `docs/evidence/ONTOGONY_PLATFORM_MECHANICS_ONLY_CONFORMANCE_001_CLOSEOUT.md` |
