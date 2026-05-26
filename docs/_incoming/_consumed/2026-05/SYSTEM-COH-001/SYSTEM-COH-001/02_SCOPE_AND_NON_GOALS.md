# SYSTEM-COH-001 scope and non-goals

## In scope

### 1. Cohesion baseline consolidation

Create a concise but deep closeout layer over existing matrices and runtime lock artifacts.

Expected target:

```text
docs/system/SYSTEM_COHESION_BASELINE.md
docs/system/SYSTEM_COHESION_ACCEPTANCE_MATRIX.md
docs/system/system-cohesion-acceptance.matrix.json
```

### 2. Release-mode validation

Add a validator that checks:

- required matrix files exist;
- runtime lock is present;
- expected route prefix/auth/env/test matrices are linked;
- acceptance matrix JSON is valid;
- scenario statuses use allowed values;
- required scenarios are not marked optional in release mode;
- known deferrals have owner, reason, and next gate.

### 3. Live/evidence gate unification

Wrap existing smoke scripts into one acceptance runner that can call existing lower-level scripts and write one normalized `summary.json`.

### 4. Error compatibility matrix

Do not force every service into one public error shape. Instead document:

- native service error shape;
- client adapter behavior;
- Allagma orchestration mapping;
- retryability classification;
- evidence/log fields.

### 5. Context propagation proof

Validate trace/correlation/actor/run/idempotency propagation through docs, constants, tests, and E2E evidence.

### 6. Observability evidence gate

Create an explicit gate for dashboard/SLO/metric evidence, even if marked deferred for alpha.

### 7. Real-tool safety continuity

Maintain real tool execution as blocked. Improve proof that it remains blocked.

## Out of scope

- Enabling real external tool execution.
- Implementing production IAM / enterprise auth.
- Promoting ontology API to v1.
- Replacing Conexus provider routing or fallback architecture.
- Moving semantic authority into Allagma or Conexus.
- Moving orchestration into Kanon.
- Forcing Conexus OpenAI-compatible errors into Kanon/Allagma-native shape.
- Adding large new product features.
- Rewriting frontend/UI architecture.

## Non-negotiable boundary rules

```text
Allagma may choose a model purpose.
Allagma must not choose provider/model routing policy.

Kanon may call Conexus for advisory assistance.
Kanon must not let Conexus output become authority without Kanon decision/provenance.

Conexus may record model-call telemetry and fallback.
Conexus must not own semantic truth or workflow state.

Platform may define neutral schemas/helpers.
Platform must not encode gaming/wellness/product semantics.
```
