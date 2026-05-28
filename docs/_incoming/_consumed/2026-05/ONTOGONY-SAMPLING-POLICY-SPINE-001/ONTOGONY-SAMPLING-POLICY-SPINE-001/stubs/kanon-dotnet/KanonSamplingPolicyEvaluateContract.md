# Kanon Sampling Policy Evaluate Contract Stub

Suggested route:

```http
POST /ontology/v0/sampling-policy/evaluate
```

Kanon receives the Conexus-local candidate resolution and returns a semantic authority judgement. The route should use current Kanon route/controller/service conventions.

## Request

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

## Response

```json
{
  "decision": "AllowedWithWarnings",
  "authorityEffect": "advisory",
  "recommendedProfileId": "DeterministicContract",
  "warnings": [],
  "violations": []
}
```
