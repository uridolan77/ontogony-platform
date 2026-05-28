## Review: Sampling Policy Spine implementation

**Overall assessment: partial but promising.**
The implementation has a real Conexus-side sampling-policy core now, not just docs. It adds domain contracts, resolver, validator, provider-parameter translation, HTTP endpoints, Kanon gateway wiring, and a frontend trace panel. But the spine is **not yet end-to-end enforced on live `/v1/chat/completions` calls**, which is the main gap.

I would rate the implementation around **6.8 / 10** as a spine foundation, but only **4.5 / 10** as a fully governed runtime mechanism.

---

## What landed well

### 1. Conexus has a real sampling-policy domain model

The core records are clean and cover the important concepts: `SamplingPolicyDecision`, `SamplingParameters`, `SamplingProfile`, `SamplingPolicyResolveRequest`, and `SamplingPolicyResolution`. The profile contract includes governance-relevant flags like `DirectExecutionAllowed`, `AllowsTools`, `AllowsSideEffects`, `BlockedOutputContracts`, `MaxRiskTier`, and determinism metadata. 

This is the right move architecturally: sampling becomes a **policy object**, not just `temperature` and `top_p`.

### 2. The resolver is structured correctly

`SamplingPolicyResolver` composes registry lookup, validation, provider translation, and optional Kanon evaluation. That is the correct shape for Conexus as the execution gateway. 

It resolves an effective profile, falls back to `DeterministicContract` for unknown profiles, warns on raw legacy parameters, translates provider parameters, builds a resolution, and optionally calls Kanon when authority mode is not `off`. 

### 3. The validator encodes the right first-order governance rules

The validator blocks stochastic profiles for direct execution, side-effecting operations, tool use, high-risk work, and contract-bound operations. This directly matches the intent of the package. 

The useful distinction is already present:

* deterministic-safe profiles: `DeterministicContract`, `ExtractionStrict`, `RepairRetry`
* stochastic profiles: `CreativeIdeation`, `DiversityProbe`
* contract-bound operations: schema mapping, routing, code patching, policy decisions, authority decisions, tool planning. 

That is exactly the conceptual layer I wanted: **sampling freedom is constrained by task type and operational risk**.

### 4. The public Conexus API routes exist

The endpoints are mapped under `/llm/v0`:

* `GET /llm/v0/sampling-profiles`
* `GET /llm/v0/sampling-profiles/{profileId}`
* `POST /llm/v0/sampling-policy/resolve`
* `POST /llm/v0/sampling-policy/validate`

The routes are implemented in `SamplingPolicyEndpoints` and mapped in `Program.cs`.  

This is good because it gives frontend, consumers, tests, and operators a stable surface.

### 5. Provider translation is present

`ProviderSamplingParameterTranslator` maps profile values to provider-style parameters like `temperature`, `top_p`, `n`, `seed`, penalties, and a strict JSON-schema preference flag. It also warns when Anthropic does not support candidate count or seed semantics. 

This is a good abstraction boundary. Provider adapters should not invent sampling policy. They should receive normalized, policy-approved parameters.

### 6. Kanon hook is real on the Conexus side

Conexus has a `KanonSamplingPolicyGateway` that posts to `/ontology/v0/sampling-policy/evaluate`. It supports advisory/strict behavior, fail-open advisory warnings, and strict-mode denial when Kanon is unavailable.  

This is the correct authority posture: Conexus owns gateway execution, Kanon may become semantic authority, and strict mode must fail closed.

### 7. Frontend trace components exist

The frontend has typed sampling trace contracts and a `SamplingPolicyPanel` / `SamplingPolicyBadge` that displays effective profile, decision, temperature, top-p, resolution id, warnings, and violations.  

The parser reads trace parameters like `sampling_effective_profile_id`, `sampling_decision`, `sampling_effective_temperature`, and `sampling_effective_top_p`. 

---

## Major gaps / issues

### 1. The policy is not enforced on `/v1/chat/completions`

This is the biggest issue.

The OpenAI-compatible request contract still exposes raw `temperature` and `top_p`. The comment explicitly says `top_p` is “passed through to the downstream provider unchanged.” 

The chat completion endpoint validates `top_p` range, but I do not see sampling policy resolution in the endpoint’s injected dependencies or before `chatCompletions.CompleteAsync(...)`. The endpoint injects `IChatCompletionService`, not `ISamplingPolicyResolver`, and passes the original request to the chat service.  

So right now the spine appears to be **queryable** and **traceable-ready**, but not necessarily **execution-governing**.

That means a caller can still send:

```json
{
  "temperature": 1.2,
  "top_p": 1,
  "tools": [...],
  "response_format": {...}
}
```

and unless the lower chat service handles policy elsewhere, the endpoint itself does not block it through the sampling resolver.

### 2. `samplingProfileId` is not part of the OpenAI-compatible request

The package intent was to make every LLM call carry a `SamplingProfileId` or derive one from policy. But `OpenAiChatCompletionRequest` currently has `model`, `messages`, raw `temperature`, `top_p`, `max_tokens`, `stream`, `metadata`, `user`, `tools`, `tool_choice`, and `response_format` — no first-class `sampling_profile_id` or equivalent. 

You can still derive from metadata, but that makes the policy spine less explicit and less contract-bound.

Recommended fix: add one of these:

```csharp
[JsonPropertyName("sampling_profile_id")]
public string? SamplingProfileId { get; init; }
```

or, if you want to avoid polluting OpenAI compatibility:

```csharp
[JsonPropertyName("x_ontogony_sampling_profile_id")]
public string? OntogonySamplingProfileId { get; init; }
```

Then allow metadata fallback only as legacy compatibility.

### 3. Kanon decision is not fully applied

This is subtle but important.

The Kanon gateway parses a response containing `Decision`, `AuthorityEffect`, `RecommendedProfileId`, warnings, and violations. 

But `ApplyKanonEvaluation` only appends Kanon warnings and violations. It ignores Kanon’s `Decision`, `AuthorityEffect`, and `RecommendedProfileId`. 

Then `DetermineDecision` derives the final decision purely from whether any notice has `Severity == "error"`; otherwise warnings become `AllowedWithWarnings`, and no warnings become `Allowed`. 

That means if Kanon returns `RequiresApproval` or `Denied` without encoding it as an error violation, Conexus may flatten it into `Allowed` or `AllowedWithWarnings`.

Recommended fix: final decision should be the stricter of local decision and Kanon decision:

```text
Denied > RequiresApproval > AllowedWithWarnings > Allowed
```

And `authorityEffect` should be preserved in the resolution.

### 4. Several options look inactive or underused

`SamplingPolicyOptions` defines:

* `Enabled`
* `DefaultProfileId`
* `StrictModeEnabled`
* `AllowLegacyRawParameters`
* `KanonAuthorityMode`
* `BlockDeniedOnChatCompletion` 

But in the reviewed resolver, `DefaultProfileId` is not used for fallback; the resolver directly falls back to `AnalyticalReasoning`. 

Also, based on the reviewed chat endpoint, `BlockDeniedOnChatCompletion` is not yet active at the main execution boundary. The endpoint does not call the resolver before dispatching to `CompleteAsync`. 

Recommended fix: either wire these options or remove them until they are real. Dead config is dangerous in governance code because operators assume toggles have teeth.

### 5. `MaxRiskTier` exists but the validator does not appear to use it

`SamplingProfile` includes `MaxRiskTier`. 

But the validator’s high-risk rule is hard-coded: high/critical risk requires one of the deterministic-safe profile IDs.  

That is acceptable for the first slice, but it means the profile contract is more expressive than the enforcement engine.

Recommended fix: add a proper risk-tier ordering:

```text
low < medium < high < critical
```

Then validate:

```text
request.RiskTier <= profile.MaxRiskTier
```

### 6. Frontend trace parser is thin

The frontend trace type supports full `SamplingPolicyTrace`, including provider parameters, warnings, violations, requested/effective parameters, and decision. 

But the parser currently reconstructs notices only from comma-separated warning/violation codes and uses the code itself as the message. 

That is okay for a compressed trace, but it loses the actual governance rationale. For operator debugging, the frontend should consume either:

```text
sampling_policy_trace_json
```

or structured evidence from the model-call detail endpoint.

### 7. UI tone helper has a type smell

`samplingProfileRiskTone()` returns `"unknown" as "healthy"` for unknown profiles. 

That cast is a small but real quality issue. Better to expand the UI status type or return `"degraded"` for unknown profiles. In governance UI, unknown should not masquerade as healthy.

---

## Architectural status

### What exists now

```text
Conexus
  ✅ Sampling domain contracts
  ✅ Resolver
  ✅ Validator
  ✅ Provider parameter translator
  ✅ Public resolve/validate/list endpoints
  ✅ Kanon HTTP gateway from Conexus side
  ✅ Frontend display components
```

### What is still missing

```text
Runtime execution path
  ❌ /v1/chat/completions does not visibly require/resolve sampling profile
  ❌ raw temperature/top_p still appear pass-through
  ❌ denied sampling resolution not visibly blocking chat completion
  ❌ sampling resolution not visibly attached to model-call evidence from the endpoint path

Kanon authority
  ⚠️ Conexus calls Kanon, but final decision ignores Kanon.Decision unless encoded as violation
  ⚠️ I did not verify matching Kanon server endpoint implementation in this pass

Allagma governance
  ❌ I did not verify Allagma run-gate/event integration

Frontend
  ✅ display components exist
  ⚠️ I did not verify they are mounted in the main trace drawer/session UI
  ⚠️ parser loses full notice messages
```

---

## Recommended next slice: `ONTOGONY-SAMPLING-POLICY-SPINE-001A`

This should be a closure/hardening slice, not a new conceptual package.

### PR 1 — Runtime enforcement in Conexus

Add policy resolution before provider invocation in the chat pipeline:

```text
OpenAiChatCompletionRequest
  → derive SamplingPolicyResolveRequest
  → ISamplingPolicyResolver.ResolveAsync
  → if Denied and BlockDeniedOnChatCompletion=true: return 400/403
  → if RequiresApproval: return 409/423 or create Allagma gate
  → overwrite downstream provider sampling params with EffectiveParameters
  → attach trace fields to model-call evidence
```

This is the highest-priority fix.

### PR 2 — Add first-class request field

Add:

```csharp
sampling_profile_id
```

Then support legacy metadata:

```text
metadata["sampling_profile_id"]
metadata["x_ontogony_sampling_profile_id"]
```

But canonical should be a first-class field.

### PR 3 — Apply Kanon decision properly

Preserve and apply:

```text
kanonDecision
authorityEffect
recommendedProfileId
kanonAvailable
```

Decision precedence:

```text
Denied > RequiresApproval > AllowedWithWarnings > Allowed
```

### PR 4 — Evidence trace closure

Every model-call evidence record should include:

```text
sampling_resolution_id
sampling_requested_profile_id
sampling_effective_profile_id
sampling_decision
sampling_policy_basis
sampling_requested_temperature
sampling_requested_top_p
sampling_effective_temperature
sampling_effective_top_p
sampling_warning_codes
sampling_violation_codes
```

Prefer also a structured JSON artifact for the full resolution.

### PR 5 — Frontend mounting and richer trace

Mount `SamplingPolicyPanel` in the actual trace drawer / model-call evidence detail. Replace code-only notices with real messages when structured trace is available.

---

## Bottom line

The implementation is **a real foundation**, not fake work. The Conexus policy model is well shaped, the validator captures the right governance intuitions, and the Kanon gateway is directionally correct.

But the key missing closure is this:

> Sampling policy exists as a side API, but it is not yet visibly the execution contract of live LLM calls.

Once `/v1/chat/completions` is forced through `ISamplingPolicyResolver`, denied decisions can block execution, effective parameters override raw caller parameters, and the resolution is persisted into evidence, this becomes a strong spine. Right now it is a strong **policy substrate**, not yet a fully enforced **runtime governance layer**.
