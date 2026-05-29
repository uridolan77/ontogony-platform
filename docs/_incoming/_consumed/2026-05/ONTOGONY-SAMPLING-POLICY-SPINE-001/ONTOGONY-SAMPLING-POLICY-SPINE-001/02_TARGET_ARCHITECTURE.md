# Target Architecture

## Conceptual components

```text
Caller / Agent Step / Workflow Node
        |
        v
SamplingPolicyResolver
        |
        |-- ProfileRegistry
        |-- OperationKind / TaskKind / RiskTier mapping
        |-- Requested override normalization
        v
SamplingPolicyValidator
        |
        |-- local validation rules
        |-- optional Kanon semantic authority
        v
EffectiveSamplingPolicy
        |
        |-- ProviderParameterTranslator
        |-- Trace/Evidence writer
        v
LLM Provider Adapter
```

## Canonical terms

### SamplingProfile

A named policy profile such as `DeterministicContract`, `AnalyticalReasoning`, or `CreativeIdeation`. Profiles are portable across providers.

### SamplingPolicyResolution

The resolved result for one LLM call. It includes requested profile, effective profile, effective parameters, decision, warnings, and provenance.

### SamplingPolicyDecision

The policy judgement:

- `Allowed`
- `AllowedWithWarnings`
- `Denied`
- `RequiresApproval`

### SamplingPolicyViolation

A structured explanation of why a requested profile/override is invalid.

Example:

```json
{
  "code": "CreativeProfileNotAllowedForContractBoundTask",
  "severity": "error",
  "message": "CreativeIdeation cannot be used for taskKind=schema_mapping.",
  "requestedProfileId": "CreativeIdeation",
  "recommendedProfileId": "DeterministicContract"
}
```

## Execution flow

1. Caller supplies metadata: `operationKind`, `taskKind`, `riskTier`, `outputContract`, `toolUse`, `requestedSamplingProfileId`, optional legacy raw parameters.
2. Resolver chooses a default if no profile was requested.
3. Resolver normalizes requested overrides.
4. Validator applies local static rules.
5. Optional Kanon evaluation adds semantic authority judgement.
6. Allagma receives events for workflow runs.
7. Provider adapter receives only the effective provider-specific parameters.
8. Trace contains the full requested/effective policy object.

## Required metadata per LLM call

Minimum:

```json
{
  "operationKind": "classification|routing|schema_mapping|code_generation|analysis|ideation|rewrite|tool_planning|side_effect_action",
  "taskKind": "contract_bound|analytical|creative|repair|extraction|diversity_probe",
  "riskTier": "low|medium|high|critical",
  "outputContract": "none|json_schema|tool_call|code_patch|policy_decision",
  "directExecution": false
}
```

## Default profile mapping

| Task / operation | Default profile | Notes |
|---|---:|---|
| schema mapping | `DeterministicContract` | no creative sampling |
| routing | `DeterministicContract` | provider/model routing must be stable |
| policy classification | `DeterministicContract` | Kanon authority path |
| extraction to schema | `ExtractionStrict` | strict JSON/schema mode when available |
| code patching | `DeterministicContract` or `RepairRetry` | allow retry profile only after deterministic failure |
| repo review / diagnosis | `AnalyticalReasoning` | moderate freedom, no side effects |
| UX copy / naming | `CreativeIdeation` | allowed low-risk only |
| alternative generation | `DiversityProbe` | must not directly execute |
| repair of invalid JSON/tool call | `RepairRetry` | low temp focused retry |

## Non-goals for v0

- No attempt to guarantee provider-level deterministic reproducibility across all vendors.
- No global optimization of decoding parameters.
- No automatic selection of temperature based on confidence unless explicitly added in a future adaptive policy package.
- No hidden fallback to high temperature.
