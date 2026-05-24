# Message rendering contract

## Purpose

Render real interaction messages when available, without inventing messages or exposing sensitive raw content.

## Target shape

```ts
export interface AgentInteractionMessage {
  id: string;
  sourceSystem: "allagma" | "kanon" | "conexus" | "operator" | "provider";
  role: "operator" | "system" | "user" | "assistant" | "tool" | "provider" | "internal";
  timestamp?: string;
  title?: string;
  content: string;
  contentState: "visible" | "redacted" | "withheld" | "not_recorded" | "not_available";
  redactionReason?: string;
  relatedEventId?: string;
  relatedIdentifiers?: Record<string, string | undefined>;
}
```

## Rules

- If a message is not stored, show `not_recorded`.
- If policy blocks display, show `redacted` with reason.
- If content is too long or stream output is withheld, show `withheld` with length/hash/reason if available.
- Do not synthesize full messages from summaries.
- Do not render secrets, credentials, API keys, or secret-looking fields.

## Minimum messages to show when available

- Operator objective/input summary.
- Allagma system/task context.
- Kanon plan summary.
- Conexus model-call request summary.
- Provider/assistant response summary.

## Stream-withheld shape

```text
Output: withheld
Length: 274
Hash: b94142...
Reason: streaming output redacted from list view
```
