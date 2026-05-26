# Kanon Cursor Prompt

Implement Kanon support for `ONTOGONY-SKILL-OPTIMIZATION-SPINE-001`.

1. Inspect current decision lifecycle, evidence spine, ontology version, domain pack, quality snapshot, route inventory, and OpenAPI patterns.
2. Add or extend contracts for SkillArtifact, SkillVersion, SkillEdit, SkillEvaluationGate, RejectedSkillEditBuffer, and SkillDeploymentBinding.
3. Implement a SkillEdit governance validator.
4. Create decision records for edit validation, candidate acceptance/rejection, publish, deploy, rollback.
5. Add evidence graph roots and edges for skill artifacts/versions/gates/deployments.
6. Add deterministic fixtures for accepted and rejected candidates.
7. Add tests for validation failures, accepted/rejected decisions, evidence graph fan-out, and route drift.
8. Update docs and route inventories.

Do not store hidden chain-of-thought. Store safe rationale summaries, policy basis, verifier feedback, and operator notes.
