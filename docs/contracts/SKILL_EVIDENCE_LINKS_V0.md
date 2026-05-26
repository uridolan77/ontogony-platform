# Skill Evidence Links V0

## Purpose

This spec defines the minimum evidence links required for reconstructable skill optimization.

## Canonical reference shape

```json
{
  "type": "conexusModelCall",
  "id": "conexus_call_optimizer_001",
  "role": "optimizerCall",
  "label": "Optimizer proposed bounded add/delete/replace edits",
  "uri": "/conexus/model-calls/conexus_call_optimizer_001"
}
```

## Reference types

```text
allagmaRun
allagmaOperation
trace
correlation
conexusModelCall
conexusRouteDecision
kanonDecision
kanonEvidenceGraph
kanonQualitySnapshot
skillArtifact
skillVersion
skillEdit
skillEvaluationGate
skillDeploymentBinding
evaluationTaskset
evaluationTask
evaluationResult
operatorNote
sourceDocument
```

## Minimum evidence per accepted version

An accepted skill version should link to:

```text
base SkillVersion
SkillOptimizationRun
at least one rollout evidence batch
at least one optimizer model call or fixture generator
at least one SkillEdit
one SkillEvaluationGate
one Kanon acceptance decision
```

## Minimum evidence per deployed version

A deployed skill version should additionally link to:

```text
Kanon publish/deploy decision
SkillDeploymentBinding
Conexus route/injection evidence from at least one active call
```

## Safe rationale standard

The system may store:

- rationale summaries;
- verifier feedback;
- policy evaluation summaries;
- edit expected effect;
- human operator notes;
- task failure labels;
- tool-observation summaries.

The system must not require or expose hidden model chain-of-thought.
