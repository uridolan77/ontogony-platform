# Boundary decision tree

Use this whenever Conexus work seems to need new shared code.

## Step 1 — Does the concept require gateway/product meaning?

Examples:

- provider route
- model alias
- fallback policy
- project API key
- gateway price catalog
- OpenAI-compatible request behavior
- provider-specific mapping

If yes, put it in `conexus-dotnet`.

## Step 2 — Is the concept mechanical and reusable?

Examples:

- secret value resolution
- redaction policy
- idempotency state model
- quota ledger reservation/refund
- canonical fingerprinting
- telemetry sink abstraction
- execution journal helper
- HTTP resilience
- persistence/outbox
- test host fixture

If yes, put it in `ontogony-platform`.

## Step 3 — Is it only an adapter?

If Conexus adapts a gateway-specific entity into an existing platform mechanic, keep that adapter in Conexus.

Examples:

- `ProjectQuotaPolicy` → `QuotaConsumptionRequest`
- `ProviderConfig.SecretRef` → `ISecretValueResolver`
- `ChatCompletionRequest` → `LlmRequestEnvelope`

## Step 4 — Can it be documented without code?

If a behavior is a convention rather than a reusable API, prefer docs first. Add code only when repeated implementation pressure appears.
