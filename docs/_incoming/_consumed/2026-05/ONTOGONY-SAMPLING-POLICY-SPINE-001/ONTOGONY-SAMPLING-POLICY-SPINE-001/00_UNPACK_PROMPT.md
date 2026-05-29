# ONTOGONY-SAMPLING-POLICY-SPINE-001 — Unpack / Cursor Implementation Prompt

You are working inside the Ontogony multi-repo workspace. Implement **ONTOGONY-SAMPLING-POLICY-SPINE-001** as a governed decoding/sampling policy layer, centered on Conexus and surfaced through Kanon, Allagma, and the frontend trace UI.

## Intent

Add a first-class **Sampling Policy Layer** so temperature, top-p, seed, candidate count, penalties, and deterministic/creative profiles are no longer ad-hoc provider parameters. Every LLM call must resolve a named `samplingProfileId`, produce an `effectiveSamplingPolicy`, and record requested-vs-effective decoding parameters in trace/evidence.

The change must make sampling parameters part of the agent execution contract:

- contract-bound calls must be deterministic or near-deterministic;
- creative calls may use wider sampling;
- diversity probes may generate multiple candidates but must not directly execute side effects;
- overrides must be explicit, traceable, and governable;
- Kanon may advise/deny invalid sampling usage;
- Allagma may gate high-risk stochastic overrides;
- frontend trace UI must show the sampling profile and any policy warnings.

## Repos to inspect before coding

Inspect the current structure and conventions in:

1. `conexus-dotnet` — primary implementation target.
2. `kanon-dotnet` — semantic authority / governance hook.
3. `allagma-dotnet` — workflow gate/event integration.
4. `ontogony-frontend` — trace drawer, run detail, request inspector.
5. `ontogony-platform` — package tracking, manifests, evidence index.

Do not blindly copy paths from this package. Use the current repo conventions, route naming, namespaces, test style, and documentation locations. If existing abstractions already cover part of this, extend them instead of duplicating them.

## Deliverables

### Conexus

Implement a canonical sampling layer:

- domain contracts for `SamplingProfile`, `SamplingPolicy`, `SamplingPolicyDecision`, `SamplingPolicyResolution`;
- default profile registry from `contracts/sampling-profiles.v0.json`;
- resolver that selects an effective profile from call metadata;
- validator that blocks or warns on invalid combinations;
- provider adapter translation into OpenAI/Anthropic/local-model parameter names;
- trace enrichment on every LLM call;
- public read/resolve/validate routes:
  - `GET /llm/v0/sampling-profiles`
  - `GET /llm/v0/sampling-profiles/{profileId}`
  - `POST /llm/v0/sampling-policy/resolve`
  - `POST /llm/v0/sampling-policy/validate`
- tests and golden fixtures.

### Kanon

Add advisory/strict semantic authority hooks:

- classify operation/task/risk into allowed sampling profile bands;
- flag creative/diversity profiles used on contract-bound operations;
- expose an evaluation route, or stub contract if the route is intentionally deferred;
- ensure advisory mode fails open and strict mode is host-gated.

Suggested route if aligned with current Kanon route conventions:

- `POST /ontology/v0/sampling-policy/evaluate`

### Allagma

Add workflow evidence and optional human-gate semantics:

- record sampling policy events in run timeline;
- block direct side effects from `DiversityProbe` results unless a deterministic selection step follows;
- optionally require human approval for unsafe profile overrides in high-risk workflow states.

Suggested events:

- `SamplingPolicyResolved`
- `SamplingPolicyWarningRaised`
- `SamplingPolicyViolationRaised`
- `SamplingProfileOverrideRequested`
- `SamplingProfileOverrideApproved`
- `SamplingProfileOverrideRejected`

### Frontend

Add UI visibility:

- trace drawer sampling badge;
- requested-vs-effective sampling parameters;
- warnings/violations display;
- filter by profile/risk;
- profile legend/documentation panel.

### Platform docs

Add evidence files under the platform docs convention. Update manifests/indexes only if the repo has them.

## Implementation constraints

1. Do not let callers pass raw `temperature`/`top_p` directly without a resolved profile unless this already exists for backward compatibility. If backward compatibility is needed, wrap raw values into `AdHocLegacy` with a warning and migration path.
2. Do not allow `CreativeIdeation` or `DiversityProbe` for schema mapping, routing, code patching, policy decisions, authority decisions, or side-effecting actions.
3. Do not treat `temperature=0` as guaranteed determinism. Record `determinismGuarantee: provider-dependent` unless provider seed/backend determinism is explicitly supported.
4. Do not create fake Kanon labels if Kanon is unavailable. Add explicit advisory warnings instead.
5. Keep the first implementation non-breaking: default to current provider behavior only when no safer mapping exists, but always trace the effective policy.
6. Add tests before or alongside implementation.

## Suggested PR sequence

### PR1 — Contracts and profile registry

- Add profile contracts and JSON registry.
- Add schema tests/golden fixtures.
- No runtime behavior change except documentation.

### PR2 — Resolver/validator and Conexus routes

- Implement `ISamplingPolicyResolver` and `ISamplingPolicyValidator`.
- Add resolve/validate routes.
- Add deterministic mapping from `operationKind/taskKind/riskTier` to profile.

### PR3 — Provider adapter enforcement and trace enrichment

- Thread effective sampling through all LLM calls.
- Prevent raw ungoverned overrides.
- Add trace/evidence fields and tests.

### PR4 — Kanon advisory/strict authority

- Add optional Kanon evaluation client in Conexus, or implement route in Kanon if ready.
- Strict mode must be disabled unless explicitly host-enabled.

### PR5 — Allagma events/gates

- Add run events and side-effect safety rule for diversity probe output.
- Add tests for direct-execution blocking.

### PR6 — Frontend visibility

- Show sampling profile badges in trace/run UI.
- Surface warnings and effective parameters.

### PR7 — Closeout

- Update docs, route inventory, OpenAPI snapshots, evidence index, and package status.

## Acceptance gates

Run the repo’s existing default gates. Add package-specific gates equivalent to:

```bash
dotnet test --filter SamplingPolicy
npm test -- sampling
npm run typecheck
npm run lint
```

Exact commands must follow the current repo standards.

## Definition of done

The package is complete when:

- every LLM call has an effective `samplingProfileId`;
- contract-bound calls cannot silently use creative/diverse sampling;
- sampling profile is visible in traces and frontend diagnostics;
- requested vs effective values are recorded;
- Kanon advisory/strict behavior is explicit and tested;
- Allagma has timeline evidence for profile resolution and overrides;
- tests cover resolver defaults, invalid overrides, provider translation, traces, and UI rendering;
- package closeout notes document what is implemented and what is deferred.
