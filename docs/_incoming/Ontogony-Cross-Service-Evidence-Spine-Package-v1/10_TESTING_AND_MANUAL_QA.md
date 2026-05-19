# Testing and manual QA

## Unit tests

- parse evidence identifier
- resolve run ID to graph
- resolve eval ID to graph
- resolve modelCallId to graph
- resolve decisionId to graph
- handle missing links
- render graph node cards
- export bundle redaction

## Integration tests

- mocked Allagma + Conexus + Kanon graph
- partial service failure
- stale/404 ID
- ambiguous ID with manual override

## Browser/e2e

- paste run ID → graph with run/eval/model-call/decision where available
- paste eval ID → graph with subject run and evidence export
- paste modelCallId → Conexus node and trace/run if available
- paste decisionId → Kanon node and run if available
- export evidence spine bundle

## Docker-local manual QA

Use a fresh run from local stack:
1. Start or seed run.
2. Capture run ID.
3. Resolve run ID in Evidence Spine.
4. Capture eval ID.
5. Resolve eval ID.
6. Capture model-call ID if present.
7. Resolve model-call ID.
8. Export bundle.
9. Verify graph links open relevant pages.
