# Master Cursor prompt — ALLAGMA-AGENT-INTERACTION-001

You are working in the Ontogony local multi-repo system. Implement `ALLAGMA-AGENT-INTERACTION-001`.

## Goal

Upgrade the Agent Interaction workbench from fixture-first replay into a live, operator-grade Allagma interaction inspector.

The page must default to live lookup when live Allagma is reachable, support explicit mode switching, render real Allagma events/messages/tool/gate/model-call linkage, and clearly mark fixture/imported sessions as non-live.

## Repositories to inspect first

Inspect actual code before editing:

```text
ontogony-frontend
ontogony-ui
allagma-dotnet
conexus-dotnet
kanon-dotnet
ontogony-platform
```

Do not assume file names. Search for:

```text
Agent Interaction
interaction session
fixture replay
sample-run
thread-demo
run-demo
read replay
/allagma/v0/runs
/allagma/v0/runs/{runId}/events
/allagma/v0/runs/{runId}/audit
/admin/v0/model-calls
/ontology/v0/decision-records
Evidence Spine
resolveEvidenceSpine
```

## Required behavior

1. Agent Interaction must have an explicit mode switch:
   - `live_lookup`
   - `fixture_replay`
   - `imported_jsonl`

2. Live mode must be preferred when Allagma is reachable.

3. Fixture mode must show a strong badge:

   ```text
   Demo fixture — not live evidence
   ```

4. Fixture IDs must never contribute to readiness, topology, compatibility, or “system connected” claims.

5. Live lookup must resolve by at least Allagma run ID. If existing resolver supports trace/correlation/model-call/decision IDs, use it, but do not block the sprint on adding every root kind.

6. Timeline must render real Allagma events and classify them into readable phases:
   - run lifecycle
   - planning
   - topology authorization
   - workflow checkpoints
   - tool intents/action evaluations
   - human gates
   - model call
   - evaluation/replay
   - unresolved/unknown

7. Conexus model-call details must be shown when linked:
   - model call ID
   - request ID
   - execution run ID
   - model purpose
   - Conexus alias/model alias if available
   - selected provider and provider model if available
   - provider mode: fake/real/unknown
   - fallback used yes/no/unknown
   - tokens/cost/latency when available
   - messages when safely available

8. Kanon decisions must be shown when linked:
   - planning decision ID
   - action decision ID(s), if present
   - decision type
   - ontology version
   - provenance/evidence link
   - policy/gate outcome when applicable

9. Tool and human-gate events must be first-class timeline rows, not generic raw JSON.

10. Every unresolved timeline enrichment must show a structured reason, not raw “unknown”.

11. Allagma run list must be polished:
   - no unlabeled `unknown`
   - raw fake provider responses hidden from list cards
   - show purpose, provider mode, ontology, player/context, planning decision ID, model call ID when available
   - structure stream-withheld output as length/hash/reason
   - rename `Failed runs (sample)` to a real/current-list label

12. Start Run workbench must include:
   - request preview
   - model purpose validation against backend config when possible
   - idempotency explanation: same key for retry, new key for new run
   - actions: start, start and open run detail, start and open Agent Interaction

## Implementation discipline

- Make the smallest coherent implementation that passes the acceptance checklist.
- Prefer frontend composition if backend APIs already expose enough data.
- Add backend fields/routes only where live interaction cannot be reconstructed safely from current APIs.
- Preserve existing tests and add focused tests.
- Do not enable real external tools.
- Do not make fixture data look live.
- Use shared UI primitives from `@ontogony/ui` where available.

## Deliverables

At minimum:

- Live Agent Interaction mode.
- Explicit fixture/import mode.
- Agent interaction session contract/types.
- Real Allagma timeline rendering.
- Conexus model-call enrichment.
- Kanon decision enrichment.
- Tool/gate event rendering.
- Run list polish.
- Start Run preview/open actions.
- Tests and validation notes.

## Final response from Cursor should include

- Files changed by repo.
- API routes used/added.
- Tests run and results.
- Known limitations.
- One live run ID used for validation, if available.
