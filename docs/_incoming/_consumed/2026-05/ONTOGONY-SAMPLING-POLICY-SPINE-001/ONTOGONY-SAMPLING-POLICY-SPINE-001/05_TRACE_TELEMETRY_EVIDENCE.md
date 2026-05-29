# Trace, Telemetry, and Evidence

## Required trace shape

Every LLM invocation trace should include:

```json
{
  "sampling": {
    "contractVersion": "sampling.policy.trace.v0",
    "resolutionId": "spres_...",
    "requestedProfileId": "CreativeIdeation",
    "effectiveProfileId": "DeterministicContract",
    "decision": "AllowedWithWarnings",
    "policyBasis": "taskKind=schema_mapping;outputContract=json_schema;riskTier=medium",
    "requestedParameters": {
      "temperature": 0.8,
      "topP": 0.95
    },
    "effectiveParameters": {
      "temperature": 0.0,
      "topP": 0.7,
      "candidateCount": 1
    },
    "providerParameters": {
      "temperature": 0.0,
      "top_p": 0.7
    },
    "warnings": [
      {
        "code": "ProfileDowngradedByPolicy",
        "severity": "warning",
        "message": "CreativeIdeation is not allowed for schema_mapping; using DeterministicContract."
      }
    ],
    "violations": []
  }
}
```

## Why requested vs effective matters

Requested-vs-effective sampling is essential for reconstructability. Without it, a trace can show the output but not the governance decision that shaped the output distribution.

Required fields:

- requested profile id;
- requested raw parameters, if any;
- effective profile id;
- effective canonical parameters;
- provider-specific translated parameters;
- policy decision;
- warnings/violations;
- resolver version;
- Kanon authority result if consulted;
- human override id if used.

## Telemetry dimensions

Add dimensions where existing telemetry conventions allow:

```text
sampling.profile_id
sampling.decision
sampling.warning_count
sampling.violation_count
sampling.temperature
sampling.top_p
sampling.candidate_count
sampling.legacy_raw_params_used
sampling.kanon_mode
sampling.provider_translation_warnings
```

## Evidence events

Suggested events:

```json
{
  "eventType": "SamplingPolicyResolved",
  "runId": "...",
  "stepId": "...",
  "resolutionId": "...",
  "effectiveProfileId": "DeterministicContract",
  "decision": "Allowed",
  "createdAtUtc": "..."
}
```

```json
{
  "eventType": "SamplingPolicyViolationRaised",
  "runId": "...",
  "stepId": "...",
  "violationCode": "DiversityProbeCannotDirectlyExecute",
  "severity": "error",
  "createdAtUtc": "..."
}
```

## Security / privacy

Sampling policy traces should not include prompt text unless the existing trace system already safely stores prompts. The sampling envelope is safe to store independently.
