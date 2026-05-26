# SkillOpt Paper Mapping to Ontogony

Source paper: *SkillOpt: Executive Strategy for Self-Evolving Agent Skills*, arXiv:2605.23904.

## Paper idea -> Ontogony object

| SkillOpt concept | Ontogony implementation |
|---|---|
| Skill document as trainable external state | `SkillArtifact` + immutable `SkillVersion` |
| Frozen target model | Conexus `targetModelProfile` |
| Optimizer model | Conexus `optimizerModelProfile` |
| Scored rollouts | Allagma rollout evidence + Conexus target calls + evaluation results |
| Minibatch reflection | Allagma phase + optimizer model call metadata |
| Bounded add/delete/replace edits | `SkillEdit` contract + Kanon edit validator |
| Textual learning-rate budget | edit budget and schedule in `SkillOptimizationRun` |
| Held-out selection gate | `SkillEvaluationGate` with strict improvement default |
| Rejected-edit buffer | `RejectedSkillEditBuffer` negative evidence |
| Slow/meta update | protected skill section updated only by epoch-level governance |
| Exported `best_skill.md` | published/deployed `SkillVersion` content URI/hash |
| Zero inference-time optimizer calls | Conexus invariant and tests |

## What Ontogony adds beyond the paper

- Semantic authority and policy validation through Kanon.
- Reconstructable evidence graph for accepted and rejected edits.
- Operator-facing Skill Lab.
- Deployment bindings with rollback.
- Model routing separation between optimizer and target profiles.
- Governance decisions for publish/deploy/rollback.

## Deliberate differences

The paper optimizes a compact skill artifact. Ontogony must additionally manage:

- multi-service evidence;
- domain packs and ontologies;
- model routing policies;
- human gates;
- audit and rollback;
- local fake-provider reproducibility;
- frontend operator comprehension.

## Interpretation rule

Use the paper as architecture inspiration, not as an obligation to reproduce its benchmark harness. The first Ontogony package should prove the governed lifecycle, not chase paper-level benchmark breadth.
