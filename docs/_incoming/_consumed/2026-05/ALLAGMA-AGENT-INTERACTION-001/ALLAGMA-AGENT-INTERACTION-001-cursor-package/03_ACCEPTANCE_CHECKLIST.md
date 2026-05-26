# Acceptance checklist

## Agent Interaction modes

- [ ] Page has explicit mode switch: live lookup, fixture replay, imported JSONL.
- [ ] Live lookup is default when Allagma is reachable.
- [ ] Fixture replay is never default when live backend is usable.
- [ ] Fixture replay displays: `Demo fixture — not live evidence`.
- [ ] Imported JSONL displays an imported/offline badge.
- [ ] Fixture/import sessions are excluded from readiness, compatibility, topology, and system-connected claims.

## Live run resolution

- [ ] Agent Interaction can resolve a real Allagma run ID.
- [ ] It loads run detail.
- [ ] It loads run event stream.
- [ ] It loads audit/replay/evaluation links when available.
- [ ] It records source attempts with endpoint/system/status/latency/error code.
- [ ] Failed enrichments are shown as structured missing reasons.

## Timeline rendering

- [ ] Run lifecycle events are rendered.
- [ ] Topology selection/authorization events are rendered.
- [ ] Kanon plan request/compiled events are rendered.
- [ ] Workflow checkpoint events are rendered.
- [ ] Tool intent proposed/evaluated/blocked/allowed/executed events are rendered.
- [ ] Human gate requested/approved/denied/resolved events are rendered when present.
- [ ] Conexus model requested/completed events are rendered.
- [ ] Evaluation/baseline events are rendered when present.
- [ ] Unresolved timeline events have reason codes.
- [ ] No timeline row is only raw JSON unless explicitly expanded.

## Messages

- [ ] Real Allagma messages/objective/input are rendered in a safe summary.
- [ ] Conexus model-call messages are rendered when safely available.
- [ ] Raw prompts/secrets are redacted according to project policy.
- [ ] Missing messages are labelled `not_recorded`, `not_available`, or `redacted`, not `unknown`.

## Kanon enrichment

- [ ] Planning decision ID is visible when present.
- [ ] Action decision IDs are visible when present.
- [ ] Planning and action decisions are distinct.
- [ ] Ontology version is visible.
- [ ] Provenance/evidence link is visible.
- [ ] Missing action decisions are explicitly `not_recorded` or `not_resolved`.

## Conexus enrichment

- [ ] Model call ID is visible.
- [ ] Request ID and execution run ID are separate.
- [ ] Provider mode fake/real/unknown is explicit.
- [ ] Selected provider/provider model shown when available.
- [ ] Alias/model purpose shown when available.
- [ ] Fallback chain and fallback-used status shown when available.
- [ ] Tokens/cost/latency shown when available.
- [ ] Route decision link shown when available.

## Allagma run list polish

- [ ] No unlabeled `unknown` is visible.
- [ ] Raw fake provider response is hidden from run cards.
- [ ] Run card shows purpose/provider/ontology/context/status/evidence IDs.
- [ ] Stream-withheld output is structured as length/hash/reason.
- [ ] `Failed runs (sample)` is renamed or made truthful.

## Start Run workbench

- [ ] Request preview is shown before submit.
- [ ] Model purpose is validated against runtime config when possible.
- [ ] Idempotency key behavior is explained.
- [ ] Start and open Agent Interaction action exists.
- [ ] Start and open run detail action exists.

## Export

- [ ] Export bundle includes mode/data-source semantics.
- [ ] Export bundle does not duplicate sections.
- [ ] Fixture export is clearly marked demo/non-live.
- [ ] Export includes source attempts and missing reasons.

## Safety

- [ ] Real external tool execution remains blocked.
- [ ] No new secret exposure.
- [ ] Raw provider/debug payloads are moved to detail/expanders.
