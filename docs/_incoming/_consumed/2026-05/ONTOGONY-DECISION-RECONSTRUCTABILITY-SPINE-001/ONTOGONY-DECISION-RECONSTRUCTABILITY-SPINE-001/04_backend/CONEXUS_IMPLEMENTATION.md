# Conexus Implementation Plan

## Role

Conexus should provide model-call, route-decision, provider, fallback, guardrail, and tool-call evidence fragments.

## Slice A — Route decision evidence

For each routing decision, capture or expose:

- route decision id;
- selected provider/model;
- candidate providers/models if available;
- routing policy id/version;
- reason code or safe route explanation;
- fallback/retry information;
- guardrail status;
- input hash and output hash;
- actor/correlation/trace/run ids where available.

## Slice B — Model call evidence

For each model call, expose:

- model call id;
- provider;
- model;
- input hash, not raw sensitive content unless already part of approved logs;
- output hash;
- tool-call request ids;
- status;
- latency/token metadata if already available;
- correlation id and trace id;
- linked Allagma run id or Kanon decision id if known.

## Slice C — Tool action evidence

For agent tool calls mediated by Conexus:

- tool name;
- tool arguments hash;
- target resource/method;
- execution status;
- error or blocked status;
- post-condition state if Conexus can verify it, otherwise explicit unknown.

## Slice D — Safe reasoning surrogate

Add a short, explicit `routeExplanation` or `decisionReasonCode` when possible. This is not model chain-of-thought. It should be a system-generated summary such as:

```text
selected_openai_gpt5_5_due_to_policy_default_and_cost_tier
fallback_to_fake_provider_because_openai_key_missing
blocked_tool_call_due_to_guardrail_policy
```

This can classify `reasoningEvidence` as `P` or sometimes `F` when deterministic.

## Slice E — API / DTO exposure

Prefer extending existing admin/read DTOs for model calls and route decisions. Candidate additions:

```ts
decisionEventFragment?: DecisionEventFragmentContract
reconstructabilityLinks?: {
  decisionEventId?: string;
  kanonReportUrl?: string;
}
```

## Required tests

- route decision emits policy id/version or deterministic default marker;
- model call emits input/output hashes;
- fallback decision has output action and safe route explanation;
- tool call blocked by guardrail produces `outputAction.status = blocked`;
- no hidden chain-of-thought appears in persisted DTOs.

## Docs

Update Conexus routing/provider docs with a section: “Reconstructability Evidence Emitted by Conexus.”
