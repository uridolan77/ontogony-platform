# Contracts and Profiles

## Canonical profiles v0

### DeterministicContract

For contract-bound calls: schema mapping, routing, tool-plan validation, policy classification, code patch execution plans, authority checks.

Recommended parameters:

```json
{
  "temperature": 0.0,
  "topP": 0.7,
  "candidateCount": 1,
  "directExecutionAllowed": true,
  "allowsSideEffects": true,
  "determinismGuarantee": "provider-dependent"
}
```

### ExtractionStrict

For extraction into a JSON schema or structured contract. Similar to deterministic, but should prefer strict response format/tool schema when provider supports it.

### RepairRetry

For retrying malformed structured output. Low temperature, focused, no creative expansion.

### AnalyticalReasoning

For architecture reviews, diagnostics, synthesis, repo assessment, design tradeoff analysis. Allows modest stochasticity but should not directly execute side effects.

### CreativeIdeation

For names, copy, UX text, scenarios, comedy, philosophical metaphors. Not for execution, routing, schema mapping, or policy.

### DiversityProbe

For generating multiple alternatives. Must not directly execute. Requires a subsequent selection/compression step, preferably deterministic.

## Profile compatibility matrix

| Profile | Contract-bound | Analysis | Creative | Tool use | Side effects | Multi-candidate |
|---|---:|---:|---:|---:|---:|---:|
| DeterministicContract | yes | yes | allowed but not ideal | yes | yes | no |
| ExtractionStrict | yes | limited | no | yes | no | no |
| RepairRetry | yes | limited | no | yes | no | no |
| AnalyticalReasoning | no for strict contracts | yes | limited | no direct | no | no |
| CreativeIdeation | no | no for decisions | yes | no | no | no |
| DiversityProbe | no | yes for alternatives | yes | no direct | no | yes |

## Violation codes

Use stable violation codes. Suggested v0 codes:

```text
UnknownSamplingProfile
CreativeProfileNotAllowedForContractBoundTask
DiversityProbeCannotDirectlyExecute
HighRiskOperationRequiresDeterministicProfile
RawSamplingParametersRequireMigration
ProviderDoesNotSupportRequiredSamplingParameter
CandidateCountUnsupportedByProvider
ToolUseRequiresDirectExecutionSafeProfile
SideEffectRequiresDirectExecutionSafeProfile
KanonDeniedSamplingPolicy
KanonUnavailableInStrictMode
```

## Warning codes

```text
ProviderDependentDeterminism
LegacyRawSamplingParameters
ProviderParameterOmitted
KanonUnavailableAdvisoryMode
ProfileDowngradedByPolicy
ProfileOverriddenByHumanApproval
```

## Contract versioning

Use explicit version strings:

```text
sampling.policy.contract.v0
sampling.profiles.v0
sampling.policy.trace.v0
```

Do not make provider-specific fields part of the canonical contract except inside a provider translation envelope.
