# Prompt 01 — Frontend Resolver

```text
Implement the frontend part of EVIDENCE-SPINE-REAL-001.

Requirements:
1. Add or normalize stable EvidenceMissingReasonCode values.
2. Add applicability classification: required, optional, not_applicable.
3. Add structured EvidenceMissingLink entries for unresolved relationships.
4. Replace generic source attempt error rendering with reasonCode/message/suggestedNextStep.
5. Implement graph node canonicalization and placeholder merge.
6. Normalize modelCallId/requestId/executionRunId display and graph identifiers.
7. Normalize Allagma route templates to /allagma/v0/... in source attempts.
8. Direct Conexus model-call roots must mark Kanon decision links as not_applicable unless explicitly linked.
9. Governed Allagma and baseline/evaluation roots must require Kanon planning decision and Conexus model call when present in run metadata/events/evidence.
10. Update export bundle to include missing reason/applicability/source attempts if not already included.

Tests to add:
- direct Conexus root -> Kanon not_applicable;
- Allagma root -> Kanon required;
- placeholder + live node merge;
- route decision typed 404 -> backend_missing/not_recorded;
- route decision 200 -> node + edge;
- malformed Allagma /runs route no longer appears;
- model call/request/execution IDs rendered separately.

Keep changes focused. Do not rebuild all Agent Interaction UI in this task.
```
