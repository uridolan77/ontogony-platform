# Kanon Governance Hooks

## Role

Kanon should act as semantic authority over whether a sampling profile is appropriate for a task, not as the runtime provider-parameter translator.

Conexus owns runtime resolution and enforcement. Kanon evaluates meaning and risk:

- Is this operation contract-bound?
- Is this output a semantic authority decision?
- Is the requested profile too stochastic?
- Is a diversity probe being used where a canonical answer is required?
- Is a creative profile used in a place that will directly execute?

## Authority modes

### off

No Kanon call. Conexus local rules apply.

### advisory

Conexus calls Kanon if configured. If Kanon is unavailable, Conexus continues and records `KanonUnavailableAdvisoryMode` warning.

### strict

Conexus treats Kanon denial as a hard failure. Strict mode must be explicitly host-enabled. If Kanon is unavailable in strict mode, return denial or fail closed according to current platform convention.

## Suggested Kanon contract

Route:

```http
POST /ontology/v0/sampling-policy/evaluate
```

Request:

```json
{
  "contractVersion": "sampling.policy.contract.v0",
  "caller": "conexus-dotnet",
  "operationKind": "schema_mapping",
  "taskKind": "contract_bound",
  "riskTier": "medium",
  "outputContract": "json_schema",
  "requestedProfileId": "CreativeIdeation",
  "effectiveProfileId": "DeterministicContract",
  "directExecution": false,
  "toolUse": false,
  "sideEffecting": false
}
```

Response:

```json
{
  "decision": "AllowedWithWarnings",
  "authorityEffect": "advisory",
  "recommendedProfileId": "DeterministicContract",
  "warnings": [
    {
      "code": "CreativeProfileNotAllowedForContractBoundTask",
      "severity": "warning",
      "message": "Creative profile is not semantically appropriate for contract-bound schema mapping."
    }
  ],
  "violations": []
}
```

## Kanon tests

Minimum tests:

1. `CreativeIdeation` denied/warned for `schema_mapping`.
2. `DiversityProbe` denied for `directExecution=true`.
3. `AnalyticalReasoning` allowed for repo diagnostic analysis.
4. `DeterministicContract` allowed for policy classification.
5. Unknown profile denied.
6. Advisory mode fail-open warning when route unavailable.
7. Strict mode fail-closed when route unavailable.

## Avoid fake labels

If Kanon is unavailable, Conexus must not pretend that Kanon approved anything. The trace must say:

```json
{
  "kanon": {
    "authorityMode": "advisory",
    "available": false,
    "warnings": ["KanonUnavailableAdvisoryMode"]
  }
}
```
