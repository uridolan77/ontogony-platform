# Conexus Backend Implementation

## New domain area

Suggested namespace:

```text
Conexus.Domain.Sampling
Conexus.Application.Sampling
Conexus.Infrastructure.Sampling
Conexus.Api.Controllers
```

Adapt names to existing repo conventions.

## Domain records

Implement equivalent records/classes:

```csharp
public sealed record SamplingProfile(
    string Id,
    string DisplayName,
    string Intent,
    decimal Temperature,
    decimal TopP,
    int CandidateCount,
    decimal? PresencePenalty,
    decimal? FrequencyPenalty,
    bool DirectExecutionAllowed,
    bool AllowsTools,
    bool AllowsSideEffects,
    string DeterminismGuarantee,
    IReadOnlyList<string> AllowedTaskKinds,
    IReadOnlyList<string> BlockedOutputContracts,
    IReadOnlyList<string> Notes);

public sealed record SamplingPolicyResolution(
    string ResolutionId,
    string? RequestedProfileId,
    string EffectiveProfileId,
    SamplingPolicyDecision Decision,
    SamplingParameters EffectiveParameters,
    IReadOnlyList<SamplingPolicyWarning> Warnings,
    IReadOnlyList<SamplingPolicyViolation> Violations,
    string PolicyBasis,
    DateTimeOffset ResolvedAtUtc);
```

## Core interfaces

```csharp
public interface ISamplingProfileRegistry
{
    IReadOnlyCollection<SamplingProfile> List();
    SamplingProfile? TryGet(string profileId);
}

public interface ISamplingPolicyResolver
{
    Task<SamplingPolicyResolution> ResolveAsync(
        SamplingPolicyResolveRequest request,
        CancellationToken cancellationToken);
}

public interface ISamplingPolicyValidator
{
    Task<SamplingPolicyValidationResult> ValidateAsync(
        SamplingPolicyResolveRequest request,
        SamplingProfile effectiveProfile,
        CancellationToken cancellationToken);
}

public interface IProviderSamplingParameterTranslator
{
    ProviderSamplingParameters Translate(
        SamplingProfile profile,
        ProviderCapabilities capabilities);
}
```

## Profile registry

Start with JSON-backed registry from `contracts/sampling-profiles.v0.json`.

Recommended behavior:

- load once at startup;
- validate schema on startup or test;
- expose a read-only list route;
- allow environment override only if already supported by platform configuration patterns;
- do not allow silent mutation from callers.

## Resolver rules

### Rule 1 — explicit profile wins only if valid

If caller requests `samplingProfileId`, validate against the operation. If invalid, deny or downgrade depending on mode.

### Rule 2 — default from task/risk

If no explicit profile is requested:

```text
contract_bound          -> DeterministicContract
schema_mapping          -> DeterministicContract
routing                 -> DeterministicContract
policy_decision         -> DeterministicContract
json_extraction         -> ExtractionStrict
analysis                -> AnalyticalReasoning
repair                  -> RepairRetry
creative                -> CreativeIdeation
alternative_generation  -> DiversityProbe
```

### Rule 3 — high-risk restricts stochasticity

For `riskTier=high|critical`, profile must be one of:

- `DeterministicContract`
- `ExtractionStrict`
- `RepairRetry`

unless a human-approved override exists.

### Rule 4 — side effects require direct-execution-safe profile

If `directExecution=true` or `outputContract=tool_call|side_effect_action`, reject profiles where `directExecutionAllowed=false` or `allowsSideEffects=false`.

### Rule 5 — legacy raw params are wrapped

If caller passes raw `temperature`/`top_p` without a profile, create an `AdHocLegacy` resolution warning:

```text
code=LegacyRawSamplingParameters
severity=warning
recommendedMigration=samplingProfileId
```

Do not add `AdHocLegacy` to the canonical registry unless needed for backward compatibility.

## Routes

### GET /llm/v0/sampling-profiles

Returns list of active profiles.

### GET /llm/v0/sampling-profiles/{profileId}

Returns a single profile or 404.

### POST /llm/v0/sampling-policy/resolve

Input:

```json
{
  "caller": "string",
  "operationKind": "schema_mapping",
  "taskKind": "contract_bound",
  "riskTier": "medium",
  "outputContract": "json_schema",
  "requestedProfileId": null,
  "directExecution": false,
  "toolUse": false
}
```

Output: `SamplingPolicyResolution`.

### POST /llm/v0/sampling-policy/validate

Same input, but returns validation result without provider execution.

## Provider adapter translation

Provider adapters must receive effective parameters from the resolution only.

Example translation rules:

| Canonical field | OpenAI-style | Anthropic-style | Notes |
|---|---|---|---|
| `temperature` | `temperature` | `temperature` | omit if provider default must be used |
| `topP` | `top_p` | `top_p` | some providers may use `top_p` only |
| `candidateCount` | `n` | maybe unsupported | if unsupported, warning required |
| `seed` | `seed` | unsupported/varies | only set if provider supports |
| `jsonSchemaStrict` | `response_format` | tool/schema mode | map by provider capability |

Do not throw for unsupported optional parameters unless the profile requires them. Add a provider capability warning to trace.

## Configuration

Suggested config keys:

```text
Conexus:SamplingPolicy:Enabled=true
Conexus:SamplingPolicy:DefaultProfileId=AnalyticalReasoning
Conexus:SamplingPolicy:StrictModeEnabled=false
Conexus:SamplingPolicy:AllowLegacyRawParameters=true
Conexus:SamplingPolicy:KanonAuthorityMode=off|advisory|strict
```

Strict mode should be off by default until all call sites are migrated.

## Migration path

1. Add resolver and trace without changing behavior.
2. Migrate internal call sites to named profiles.
3. Turn raw-param warnings into test failures for new call sites.
4. Enable strict validation for contract-bound operations.
5. Later, consider global strict mode.
