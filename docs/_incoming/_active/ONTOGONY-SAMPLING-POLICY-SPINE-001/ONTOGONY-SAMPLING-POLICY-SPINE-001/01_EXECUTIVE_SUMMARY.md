# Executive Summary

## Problem

LLM decoding parameters are currently easy to treat as low-level style knobs: `temperature`, `top_p`, `seed`, penalties, candidate count, and deterministic settings get passed directly to providers or hidden inside call sites. That is unsafe for an agentic platform.

For Ontogony, sampling is not merely generation style. It is part of the execution contract. A schema-mapping call, routing call, semantic-authority call, code-edit call, and side-effecting workflow decision should not have the same stochastic freedom as a name-generation or brainstorming call.

## Solution

Introduce **Sampling Policy Spine v0**:

- a canonical registry of named profiles;
- a resolver that chooses an effective profile per LLM call;
- a validator that checks requested overrides against task/risk constraints;
- provider-specific parameter translation;
- trace and evidence fields;
- governance hooks in Kanon and Allagma;
- frontend visibility.

## Core design principle

A model call should not say only:

```json
{"temperature": 0.8, "top_p": 0.95}
```

It should say:

```json
{
  "samplingProfileId": "CreativeIdeation",
  "policyBasis": "taskKind=ideation;riskTier=low",
  "effectiveParameters": {
    "temperature": 0.8,
    "topP": 0.95,
    "candidateCount": 1
  },
  "directExecutionAllowed": false
}
```

## Primary implementation target

`conexus-dotnet` is the authoritative runtime location because Conexus is the LLM routing/gateway layer. Kanon and Allagma should govern and evidence the policy, but Conexus must resolve and enforce it before provider invocation.

## Expected outcome

After this package, Ontogony gains a visible, testable, governed sampling layer. This materially strengthens:

- reproducibility;
- contract compliance;
- trace reconstructability;
- safety of tool/agent execution;
- provider portability;
- architectural clarity.
