# Conexus Implementation Plan

## Role

Conexus owns model routing, provider abstraction, optimizer/target call separation, and skill injection metadata.

## Key distinction

Skill optimization has two model roles:

```text
target model    = model whose behavior is being improved by the skill
optimizer model = model that proposes bounded skill edits from rollout evidence
```

They may be the same provider/model family, but must be represented separately.

## New / extended metadata

Every skill-injected target model call should include:

```json
{
  "skillInjection": {
    "enabled": true,
    "skillArtifactId": "demo.spreadsheet-analysis",
    "skillVersionId": "skillver_demo_spreadsheet_v1",
    "skillContentHash": "sha256:...",
    "skillDeploymentBindingId": "skillbind_demo_local_001",
    "injectionMode": "developer-context",
    "resolvedBy": "skill-binding-resolver-v0"
  }
}
```

Every optimizer model call should include:

```json
{
  "skillOptimization": {
    "role": "optimizer",
    "skillOptimizationRunId": "skillopt_run_demo_001",
    "baseVersionId": "skillver_demo_spreadsheet_v0",
    "outputContract": "skill-edit-list-v0"
  }
}
```

## Normal inference invariant

Normal inference may use skill injection. It must not call the optimizer.

Add a testable invariant:

```text
When request kind = normal inference, optimizerModelCallCount == 0.
```

## Routing profiles

Suggested local profiles:

```text
fake-target-local
fake-optimizer-local
openai-target-default
openai-optimizer-default
```

OpenAI support should be optional and behind configuration. The fake provider should be sufficient for deterministic tests.

## Skill injection modes

```text
system-prefix
developer-context
tool-policy-context
harness-persistent-memory
none
```

First slice can use `developer-context` or the closest existing pattern.

## Optimizer output contract

Optimizer calls should return structured JSON or a validated DTO, not arbitrary free text.

Minimum shape:

```json
{
  "edits": [
    {
      "operation": "add",
      "sectionPath": "/Procedure",
      "proposedText": "Before answering, inventory all sheets and named ranges.",
      "rationaleSummary": "Failures skipped hidden sheets.",
      "expectedEffect": "Reduce missed data references.",
      "riskClass": "low"
    }
  ],
  "rejectedEditAwareness": [
    "Do not remove workbook inventory step."
  ]
}
```

## Endpoints / diagnostics

Adapt names to existing Conexus admin route conventions:

```text
GET /admin/v0/skill-routing/profiles
GET /admin/v0/model-calls/{callId}/skill-metadata
POST /admin/v0/skill-injection/resolve
```

## Tests

Add tests for:

- skill metadata on target calls;
- optimizer metadata on optimizer calls;
- fake optimizer returns valid edit list;
- invalid optimizer edit output is rejected;
- normal inference does not call optimizer;
- skill binding resolver precedence;
- OpenAPI/admin route drift.

## Docs

Update:

- model routing guide;
- provider/fake provider guide;
- admin auth notes if new admin routes are added;
- local fixture guide.
