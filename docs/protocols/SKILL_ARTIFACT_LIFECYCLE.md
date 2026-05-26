# Skill artifact lifecycle

Canonical lifecycle for `ONTOGONY-SKILL-OPTIMIZATION-SPINE-001`. Product repos may adapt route names; semantics are fixed.

```text
DraftSkill
  -> CandidateSkillVersion
  -> UnderOptimization
  -> GatePending
  -> AcceptedValidated
  -> Published
  -> Deployed
  -> Superseded | Retired | RolledBack
```

Rejected candidates are retained as evidence:

```text
CandidateSkillVersion
  -> GateRejected
  -> RejectedEditBuffer
```

## Authority

- **Kanon** validates edits, gate outcomes, and publish/deploy policy.
- **Allagma** records optimization run timeline, operations, and replay fixtures.
- **Conexus** applies **deployment binding** at target-model call time only; optimizer calls are explicit optimization-run phases, not default inference.

## Related contracts

See [SKILL_OPTIMIZATION_SPINE.md](./SKILL_OPTIMIZATION_SPINE.md).
