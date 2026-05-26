# Executive Brief

## Why this package exists

Ontogony already has the right architectural ingredients for governed agent evolution:

- Allagma can run and replay agent workflows.
- Conexus can route model calls and capture provider-level execution metadata.
- Kanon can govern semantic authority, provenance, decisions, and evidence graphs.
- The frontend can expose an operator console for traces, decisions, and evidence.

The missing layer is a first-class way to turn repeated execution evidence into improved procedural knowledge without making the system unsafe or opaque.

## Strategic move

Add a **Skill Optimization Spine**:

```text
Skill = versioned procedural artifact
Optimization = governed run lifecycle
Edit = atomic bounded patch
Gate = held-out strict validation
Rejected edit = negative evidence
Deployment = explicit binding to agent/domain/harness/model route
```

This gives Ontogony a serious capability that ordinary agent frameworks lack: governed self-improvement of agent behavior without fine-tuning and without hidden runtime mutation.

## What this is not

This is not:

- a prompt scratchpad;
- a global memory dump;
- automatic production self-modification;
- a model-weight training pipeline;
- a one-shot skill generator;
- a hidden chain-of-thought logger;
- a standalone benchmark runner unrelated to the Ontogony evidence model.

## What this should become

A local operator should be able to ask:

```text
Why is this agent better today than yesterday?
Which skill changed?
Which evidence caused the change?
Which edits were accepted?
Which edits were rejected?
Which held-out gate proved the improvement?
Which domains/models/harnesses use the new version?
Can we roll back?
```

The system should answer from persisted evidence, not from memory or tribal knowledge.

## First-slice success criteria

The first slice is successful when it demonstrates:

1. One skill artifact with at least two versions.
2. One optimization run with rollout evidence.
3. One accepted candidate due to held-out improvement.
4. One rejected candidate due to tie/regression/validation failure.
5. One Kanon decision for acceptance/publish/deployment.
6. One Conexus skill-injected target call.
7. One frontend Skill Lab view that makes the process understandable.
8. Deterministic tests that prove the behavior.

## Strong product interpretation

Ontogony should present this as **governed agent learning from operational evidence**.

Do not market it as "prompt optimization." The correct product language is:

```text
Auditable skill evolution for governed agent systems.
```
