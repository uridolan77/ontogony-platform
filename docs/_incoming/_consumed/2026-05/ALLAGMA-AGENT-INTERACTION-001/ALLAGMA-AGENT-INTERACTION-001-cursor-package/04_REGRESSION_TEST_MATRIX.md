# Regression test matrix

| Area | Scenario | Expected result |
|---|---|---|
| Mode | Allagma reachable | Agent Interaction defaults to live lookup |
| Mode | Allagma unreachable | Live mode shows unavailable reason; fixture available only by explicit selection |
| Mode | Fixture selected | Strong demo badge; no readiness contribution |
| Mode | JSONL imported | Imported/offline badge and imported source metadata |
| Live run | Valid completed run ID | Session summary + timeline rendered |
| Live run | Missing run ID | Structured `not_found` or `not_resolved`, no crash |
| Timeline | Known Allagma event names | Mapped to phase/title/status |
| Timeline | Unknown event name | Rendered as unresolved/other with raw details behind expander |
| Timeline | Duplicate events | Preserve chronology; group only where UI clearly indicates grouping |
| Kanon | Planning decision present | Decision card with ontology/provenance link |
| Kanon | Action decision absent | `not_recorded`, not generic unknown |
| Conexus | Model call present | Provider/model/tokens/latency/attempts displayed when available |
| Conexus | Model call missing | Structured missing reason; timeline still usable |
| Messages | Redacted content | Redaction is explicit and does not break layout |
| Tools | Tool intent blocked | Shows proposed/evaluated/blocked with policy reason if available |
| Gates | Waiting human gate | Gate row with waiting status and resume link where applicable |
| Gates | Approved/denied gate | Resolution row and outcome visible |
| Run list | Existing fake-provider runs | No raw fake response in card summary |
| Run list | Unknown task type | Label says `Task type: unknown` or field hidden |
| Run list | Stream output withheld | length/hash/reason display |
| Start Run | Preview | Request preview updates when fields change |
| Start Run | Existing idempotency key retry | UI explains reuse semantics |
| Start Run | Start and open Agent Interaction | Navigates to live interaction session after successful creation |
| Export | Live session export | Contains source attempts/missing reasons; no duplicate sections |
| Export | Fixture export | Marked as demo/non-live |
