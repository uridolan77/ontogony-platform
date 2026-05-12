# Boundary Guardrails

## Allowed in Ontogony.Platform

- LLM request/response/cost/usage records.
- Tool-call records as facts.
- Execution run/step/attempt/checkpoint mechanics.
- Artifact references and storage contracts.
- Source/document/chunk/embedding/corpus references.
- Redaction mechanics.
- Quota and usage-window mechanics.
- Evaluation and policy-decision records.

## Forbidden in Ontogony.Platform

- Model-routing policy.
- Provider preference/ranking.
- Agent planning or tool-selection semantics.
- Human-review policy.
- Canonization, contradiction, evidence authority, epistemic status.
- Retrieval relevance policy.
- Product workflows.

## Review test

If a type or method requires knowing what an output *means*, it probably belongs in Conexus, Agentor, Athanor, or a KB product repo — not in the platform.
