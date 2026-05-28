# Test Plan

## Conexus unit tests

### Resolver tests

- no requested profile + `schema_mapping` -> `DeterministicContract`;
- no requested profile + `analysis` -> `AnalyticalReasoning`;
- no requested profile + `creative` -> `CreativeIdeation`;
- requested `CreativeIdeation` + `schema_mapping` -> warning/downgrade or denial depending strictness;
- requested `DiversityProbe` + `directExecution=true` -> denied;
- high-risk operation + `AnalyticalReasoning` -> denied or downgraded;
- legacy raw params -> warning and wrapped resolution;
- unknown profile -> denied.

### Validator tests

- side-effect output requires direct-execution-safe profile;
- tool call output rejects creative/diversity profiles;
- JSON schema extraction prefers `ExtractionStrict`;
- provider unsupported candidate count yields warning;
- provider required parameter unsupported yields violation.

### Provider translator tests

- OpenAI translation maps `topP` to `top_p`;
- unsupported `candidateCount` does not silently disappear without warning;
- strict JSON/schema flag maps only when supported;
- seed is included only for provider capability.

### API route tests

- `GET /llm/v0/sampling-profiles` returns registry;
- `GET /llm/v0/sampling-profiles/{id}` returns 404 for unknown;
- `POST /llm/v0/sampling-policy/resolve` returns expected effective profile;
- `POST /llm/v0/sampling-policy/validate` does not execute provider calls.

## Kanon tests

- profile/task compatibility classification;
- advisory fail-open behavior;
- strict fail-closed behavior;
- no fake authority labels on unavailable Kanon.

## Allagma tests

- run timeline contains sampling events;
- diversity probe cannot directly precede side-effect action;
- human gate approval/rejection records evidence.

## Frontend tests

- render sampling badge;
- render warning downgrade;
- render violation state;
- trace without sampling remains compatible;
- profile legend loads from fixture.

## Golden fixtures

Add fixtures for:

1. deterministic schema mapping;
2. analytical repo review;
3. creative ideation;
4. diversity probe with candidates;
5. invalid creative contract-bound request;
6. legacy raw params warning;
7. Kanon unavailable advisory warning.

## Regression guards

Add a repository-level search/guard if current conventions allow it:

- new Conexus provider calls should not pass literal raw `temperature` without policy resolution;
- trace schema should include `sampling` when an LLM call is recorded;
- strict profiles should not drift above configured thresholds.
