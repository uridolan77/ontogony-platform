# Target Architecture

## Conceptual architecture

```text
                   ┌──────────────────────────────────┐
                   │        Ontogony Skill Lab         │
                   │ inventory · lineage · diff · gate │
                   └──────────────────┬───────────────┘
                                      │
┌─────────────────────────────────────▼─────────────────────────────────────┐
│                         Skill Optimization Spine                           │
│                                                                            │
│  SkillArtifact ── SkillVersion ── SkillEdit ── EvaluationGate ── Deployment │
│        │              │              │              │              │       │
│        └──────────── evidence links / decisions / traces ──────────┘       │
└───────────────┬────────────────────────┬────────────────────────┬─────────┘
                │                        │                        │
     ┌──────────▼──────────┐  ┌──────────▼──────────┐  ┌──────────▼──────────┐
     │       Allagma        │  │        Kanon         │  │       Conexus        │
     │ run lifecycle        │  │ semantic authority   │  │ model routing        │
     │ rollout/replay       │  │ edit decisions       │  │ skill injection      │
     │ gates/operations     │  │ evidence graph       │  │ optimizer/target     │
     └─────────────────────┘  └─────────────────────┘  └─────────────────────┘
```

## Primary objects

### SkillArtifact

Stable identity for a skill family.

Examples:

```text
demo.spreadsheet-analysis
kanon.ontology-mapping-review
allagma.workflow-repair
conexus.routing-diagnostic
```

### SkillVersion

Immutable version of a skill document plus metadata:

- content hash;
- base version;
- status;
- domain scope;
- compatibility;
- evidence links;
- evaluation summary;
- publish/deployment state.

### SkillEdit

Atomic proposal against a base version:

```text
add | delete | replace
```

Each edit has source evidence, expected effect, bounded budget units, risk classification, validation status, and decision links.

### SkillOptimizationRun

Allagma-owned process that coordinates rollout evidence, candidate generation, validation, evaluation gate, and status transitions.

### SkillEvaluationGate

Comparison between incumbent and candidate on a selection split. The default acceptance policy is strict improvement.

### RejectedSkillEditBuffer

Negative evidence memory. Rejected edits should be queryable, summarized, and fed back into later optimization, but not deployed.

### SkillDeploymentBinding

Mapping from skill version to execution context:

```text
agent profile + domain + harness + model route + environment + precedence
```

## Data ownership recommendation

Use this ownership boundary unless the current repos clearly suggest a better one:

```text
Allagma owns orchestration state and run timelines.
Kanon owns semantic validation, decisions, policy authority, and evidence graph classification.
Conexus owns model routing, optimizer/target call metadata, and injection into runtime requests.
Platform owns cross-repo protocol docs and local composition.
Frontend/UI owns operator-facing projections only.
```

## Cross-service IDs

Every major object should carry stable IDs:

```text
skillArtifactId
skillVersionId
skillOptimizationRunId
skillEditId
skillEvaluationGateId
kanonDecisionId
allagmaRunId
conexusModelCallId
traceId
correlationId
```

## Skill injection path

Normal inference should use only accepted/published/deployed skills.

```text
incoming agent request
        ↓
resolve domain/profile/harness
        ↓
select active SkillDeploymentBinding
        ↓
load immutable SkillVersion content/hash
        ↓
inject skill into request context
        ↓
execute target model call
        ↓
record skillVersionId + skillContentHash in model-call metadata
```

No optimizer model call is allowed in this path.

## Optimization path

Optimization is offline / controlled / governed:

```text
select skill artifact + base version
        ↓
run rollout batch with target model + base skill
        ↓
collect evidence + verifier scores
        ↓
call optimizer model to propose edits
        ↓
validate edit structure and policy safety
        ↓
apply bounded candidate patch
        ↓
run held-out selection evaluation
        ↓
accept/reject candidate
        ↓
publish/deploy only through explicit governance action
```

## Evidence graph shape

Suggested graph root:

```text
SkillVersion(skillVersionId)
```

Suggested edges:

```text
SkillVersion --derivedFrom--> SkillVersion
SkillVersion --createdBy--> SkillOptimizationRun
SkillOptimizationRun --usedRollout--> AllagmaRun
SkillOptimizationRun --usedOptimizerCall--> ConexusModelCall
SkillEdit --supportedBy--> EvidenceFragment
SkillEdit --decidedBy--> KanonDecision
SkillEvaluationGate --compares--> SkillVersion(candidate/incumbent)
SkillDeploymentBinding --deploys--> SkillVersion
SkillDeploymentBinding --authorizedBy--> KanonDecision
```
