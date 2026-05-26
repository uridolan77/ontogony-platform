# Skill sandbox activation lifecycle

Canonical lifecycle for `ONTOGONY-SKILL-RELEASE-GOVERNANCE-001`. Product repos may adapt route names; **semantics are fixed**.

## End-to-end flow (v0)

```text
SkillOptimizationRun (accepted candidate)
  → SkillPromotionRequest (manual, sandbox target)
  → SkillReleaseDecision (Kanon: approved_for_sandbox | rejected | deferred)
  → SkillReleaseDeploymentBinding (created → active | paused)
  → SkillRollback (optional: executed)
```

## Version release states

See [SKILL_VERSION_RELEASE_STATE_V0.md](../contracts/SKILL_VERSION_RELEASE_STATE_V0.md).

```text
accepted_candidate
  → promotion_requested
  → approved_for_sandbox
  → sandbox_bound
  → sandbox_active
  → rolled_back
```

`rejected_for_release` and `sandbox_paused` are valid terminal or interim states.

## Authority

| Step | Owner |
| --- | --- |
| Accepted candidate exists | Skill Optimization + Kanon skill-edit governance |
| Promotion requested | Operator + Allagma registry |
| Release approved/rejected/deferred | **Kanon** |
| Binding created/activated/paused | **Allagma** (only after Kanon allow) |
| Rollback executed | **Kanon** decision + **Allagma** orchestration |

Conexus supplies read-only model-call evidence from the source optimization run; Conexus does **not** deploy skills.

## Forbidden transitions (v0)

```text
accepted_candidate → sandbox_active (skip promotion + Kanon release decision)
approved_for_sandbox → sandbox_active (skip binding record)
any state → production_active
automatic_promotion without operator request
```

## Deferred (later packages)

```text
production_approved
production_active
progressive_rollout
automatic_rollback_triggers
real_consumer_activation
```

## Related

- [SKILL_RELEASE_GOVERNANCE.md](./SKILL_RELEASE_GOVERNANCE.md)
- [SKILL_OPTIMIZATION_SPINE.md](./SKILL_OPTIMIZATION_SPINE.md)
