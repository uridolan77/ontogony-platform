# Timeline resolution contract

## Purpose

Unify live timeline enrichment and missing reasons.

## Missing reason shape

```ts
export interface AgentInteractionMissingReason {
  id: string;
  code:
    | "not_applicable"
    | "not_recorded"
    | "not_available"
    | "not_resolved"
    | "not_found"
    | "authorization_failed"
    | "backend_missing"
    | "fixture_only"
    | "imported_only"
    | "redacted"
    | "not_implemented"
    | "lookup_failed";
  message: string;
  system?: "allagma" | "kanon" | "conexus" | "platform" | "frontend";
  endpoint?: string;
  identifierKind?: string;
  identifierValue?: string;
  relatedEventId?: string;
  suggestedNextStep?: string;
}
```

## Examples

```json
{
  "code": "not_recorded",
  "message": "Action-evaluation decision id is not present in current Allagma event payload.",
  "system": "allagma",
  "relatedEventId": "event-tool-eval-completed"
}
```

```json
{
  "code": "redacted",
  "message": "Conexus model-call messages are redacted by admin policy.",
  "system": "conexus",
  "endpoint": "/admin/v0/model-calls/{modelCallId}"
}
```

## Rule

Every failed enrichment path should produce a missing reason and a source attempt. The UI should remain useful even with partial data.
