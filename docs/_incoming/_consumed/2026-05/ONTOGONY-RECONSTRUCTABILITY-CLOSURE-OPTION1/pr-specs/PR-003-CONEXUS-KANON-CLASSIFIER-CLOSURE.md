# PR-003 — Conexus → Kanon classifier closure

## Repo focus

Primary:

```text
C:\dev\conexus-dotnet
```

Secondary:

```text
C:\dev\kanon-dotnet
```

## Goal

Prove Conexus decision events classify through Kanon with no high/critical FAIL.

## Implementation tasks

### 1. Build Conexus fixture

Create fixture from fake-provider model call evidence.

Include:

```text
route_decision
model_call
quota_policy_evaluation if possible
cache_decision if policy enabled
streaming_lifecycle if stream fixture exists
```

### 2. Add classify-batch integration

Test should:

```text
1. Project Conexus events.
2. Serialize to Kanon classify-batch input.
3. Run classifier.
4. Assert high/critical events do not FAIL.
5. Record diagnostics for WARN.
```

### 3. Fix emitter gaps

If classifier fails:
- add missing inputs;
- add policy basis;
- add project identity;
- add authorization envelope;
- add verified post-condition;
- add fragments.

Do not lower severity to avoid the classifier.

## Required docs

```text
docs/evidence/CONEXUS_KANON_RECONSTRUCTABILITY_CLOSURE.md
docs/CURRENT_STATE.md
docs/KNOWN_LIMITATIONS.md
```

## Acceptance criteria

```text
Conexus classification fixture exists.
No high/critical Conexus event classifies FAIL.
Redaction gate passes.
Classifier warnings are documented.
```
