# Frontend UI Implementation

## Goal

Make sampling policy visible to developers/operators without turning it into noise.

## Required UI surfaces

### Trace drawer

Add a `SamplingPolicyPanel` or section showing:

- profile badge;
- decision state;
- requested profile;
- effective profile;
- temperature;
- top-p;
- candidate count;
- direct execution allowed;
- warning/violation chips;
- provider translation warnings.

### Run timeline

For Allagma-backed runs, show compact events:

```text
Sampling: DeterministicContract · allowed · T=0 · top_p=0.7
Sampling warning: Creative profile downgraded for schema mapping
```

### Request inspector

Where LLM request metadata is shown, add requested-vs-effective sampling fields.

### Admin/profile legend

A read-only legend from `GET /llm/v0/sampling-profiles`:

- profile id;
- display name;
- intent;
- allowed task kinds;
- blocked output contracts;
- default parameters.

## TypeScript contract sketch

```ts
export type SamplingPolicyDecision =
  | 'Allowed'
  | 'AllowedWithWarnings'
  | 'Denied'
  | 'RequiresApproval';

export interface SamplingPolicyTrace {
  contractVersion: 'sampling.policy.trace.v0';
  resolutionId: string;
  requestedProfileId?: string | null;
  effectiveProfileId: string;
  decision: SamplingPolicyDecision;
  policyBasis: string;
  requestedParameters?: SamplingParameters | null;
  effectiveParameters: SamplingParameters;
  providerParameters?: Record<string, unknown>;
  warnings: SamplingPolicyNotice[];
  violations: SamplingPolicyNotice[];
}

export interface SamplingParameters {
  temperature?: number | null;
  topP?: number | null;
  candidateCount?: number | null;
  seed?: number | null;
  presencePenalty?: number | null;
  frequencyPenalty?: number | null;
}

export interface SamplingPolicyNotice {
  code: string;
  severity: 'info' | 'warning' | 'error';
  message: string;
  recommendedProfileId?: string | null;
}
```

## Display logic

Suggested badge states:

| Decision | Badge |
|---|---|
| `Allowed` | normal |
| `AllowedWithWarnings` | warning |
| `Denied` | error |
| `RequiresApproval` | pending |

## Frontend tests

1. Renders profile badge with effective profile.
2. Shows requested-vs-effective downgrade.
3. Shows warning list.
4. Handles missing sampling trace gracefully.
5. Shows profile legend from API fixture.
6. Filters traces by profile id.
