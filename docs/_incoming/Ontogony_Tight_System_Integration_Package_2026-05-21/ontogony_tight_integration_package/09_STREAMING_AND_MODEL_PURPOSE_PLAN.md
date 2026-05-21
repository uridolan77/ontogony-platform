# 09 — Streaming and model-purpose plan

## Current model

Allagma owns model purpose. Conexus owns alias routing.

Example:

```json
{
  "Allagma": {
    "ModelPurposes": {
      "summarize-player-risk": {
        "ConexusModelAlias": "risk-summary-v0",
        "SystemPrompt": "You are a concise risk-summary assistant."
      },
      "summarize-player-risk-stream": {
        "ConexusModelAlias": "risk-summary-stream-v0",
        "SystemPrompt": "You are a concise risk-summary assistant.",
        "Stream": true,
        "PersistStreamedOutput": false
      }
    }
  }
}
```

## Tightening goals

1. Every model purpose used by Allagma must be validated against Conexus aliases in local stack bootstrap.
2. Streaming purpose must have explicit evidence semantics.
3. Operator must see stream lifecycle without storing payload by default.
4. Package-mode build must fail if streaming client contract breaks.

## Required validations

| Validation | Owner | Description |
|---|---|---|
| Purpose config validation | Allagma | Missing alias/prompt/stream config fails startup where required |
| Alias seed validation | Conexus/Allagma | Local seed confirms `risk-summary-v0` and `risk-summary-stream-v0` exist |
| Streaming client compile gate | Allagma | Uses `IConexusChatCompletionsClient.StreamAsync` typed contract |
| Streaming smoke | Allagma | Confirms stream lifecycle events |
| Operator stream panel | Frontend | Displays chunk count and state |

## Idempotency boundary

Streaming and `Idempotency-Key` must remain explicitly constrained. Operator docs should state:

- Non-streaming completion can be replay-safe with Conexus replay semantics.
- Streaming request with `Idempotency-Key` is rejected by Conexus.
- Allagma should treat streamed runs as auditable lifecycle events, not replayed payload streams.

## Evidence payload policy

Default:

```json
"PersistStreamedOutput": false
```

Allowed only in local/dev or explicit audit experiments:

```json
"PersistStreamedOutput": true
```

If enabled, evidence must record retention policy, redaction posture, and operator warning.
