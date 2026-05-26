# Final Checklist

## Contracts

- [ ] SkillArtifact contract implemented or documented.
- [ ] SkillVersion contract implemented or documented.
- [ ] SkillEdit contract implemented or documented.
- [ ] SkillOptimizationRun contract implemented or documented.
- [ ] SkillEvaluationGate contract implemented or documented.
- [ ] RejectedSkillEditBuffer contract implemented or documented.
- [ ] SkillDeploymentBinding contract implemented or documented.

## Backend

- [ ] Allagma can create/read a skill optimization run.
- [ ] Allagma exposes timeline/operations/gate/rejected-buffer data.
- [ ] Kanon validates edits and records decisions.
- [ ] Kanon evidence graph links skill objects.
- [ ] Conexus records target skill injection metadata.
- [ ] Conexus records optimizer model-call metadata.
- [ ] Normal inference does not call optimizer.

## Frontend

- [ ] Skill inventory page/panel exists.
- [ ] Skill detail shows active version, lineage, gate, deployment.
- [ ] Optimization run page shows timeline and candidate edits.
- [ ] Rejected edit buffer is visible and collapsible.
- [ ] Deployment binding and rollback info are visible.
- [ ] Evidence links open relevant Allagma/Kanon/Conexus objects.

## Tests

- [ ] Strict improvement accepted.
- [ ] Tie rejected.
- [ ] Regression rejected.
- [ ] Protected section mutation rejected.
- [ ] Budget exceeded rejected.
- [ ] Rejected buffer persists.
- [ ] Deployment requires publish/authorization.
- [ ] Skill-injected target call metadata exists.
- [ ] Normal inference optimizer call count is zero.
- [ ] Frontend renders accepted/rejected/deployed states.

## Docs

- [ ] Repo docs updated.
- [ ] Route inventories/OpenAPI regenerated where required.
- [ ] Local fixture guide added.
- [ ] Deferred items clearly marked.
- [ ] Package closeout notes written.
