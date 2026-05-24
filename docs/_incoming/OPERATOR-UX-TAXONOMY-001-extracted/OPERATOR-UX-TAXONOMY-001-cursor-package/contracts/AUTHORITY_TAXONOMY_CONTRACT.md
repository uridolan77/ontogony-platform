# Authority taxonomy contract

## States

| State | Meaning |
|---|---|
| `authoritative` | Source is the owning authority for this fact/decision. |
| `advisory` | AI/helper/draft output that requires review. |
| `demo` | Fixture/demo output not to be treated as truth. |
| `inferred` | Derived by frontend/resolver from available evidence. |
| `historical` | True for a previous run/export/import, not necessarily current live state. |
| `unknown` | Authority cannot be determined. |

## Examples

- Kanon decision record: `authoritative` for semantic decision.
- Conexus model-call telemetry: `authoritative` for provider/model-call evidence.
- Allagma run event stream: `authoritative` for governed execution event sequence.
- Conexus assistance draft: `advisory`.
- Fixture Agent Interaction session: `demo`.
- Imported evidence bundle: `historical`.
- Frontend-computed graph edge: `inferred` unless confirmed by source.

## Rule

Never display advisory/demo/inferred material as if it were authoritative.
