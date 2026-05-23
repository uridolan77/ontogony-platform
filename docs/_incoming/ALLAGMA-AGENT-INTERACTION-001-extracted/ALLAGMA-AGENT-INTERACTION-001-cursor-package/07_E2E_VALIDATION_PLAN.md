# E2E validation plan

## Target proof run

Use or create a local fake-provider governed run:

```text
POST /allagma/v0/runs
purpose: summarize-player-risk
ontologyVersionId: gaming-core@0.1.0
provider: fake through Conexus alias risk-summary-v0
```

Expected chain:

```text
Allagma run created
Kanon planning decision compiled
Tool intent proposed/evaluated/blocked or allowed
Conexus fake model call completed
Run completed
Evaluation/baseline optionally recorded
```

## Manual validation sequence

1. Start local stack.
2. Confirm Allagma, Kanon, Conexus reachable.
3. Start a governed run from the console or API.
4. Click `Start and open Agent Interaction`.
5. Verify Agent Interaction opens in `live_lookup` mode.
6. Verify session summary contains run ID, ontology version, planning decision ID, model call ID, trace/correlation when available.
7. Verify timeline contains real Allagma events.
8. Verify model-call panel shows fake provider mode and provider attempt.
9. Verify Kanon decision panel links to decision/provenance.
10. Export bundle and inspect that it has no duplicate sections and includes mode/source attempts/missing reasons.
11. Switch to fixture replay and confirm demo badge.
12. Confirm fixture session does not affect readiness/topology/compatibility claims.

## Automated test hooks

Recommended local script assertions:

- page text does not include `fixture replay` as current session when live run ID is loaded.
- page text includes `Data source: live` for live run.
- page text includes `Demo fixture — not live evidence` for fixture mode.
- page text includes known event names or their mapped titles.
- page text does not include raw `Fake Conexus provider response to:` in run list summary.
- page text does not include unlabeled standalone `unknown` in run cards.

## Evidence artifacts

Save validation artifacts under:

```text
artifacts/allagma-agent-interaction-001/<timestamp>/
  run.json
  events.json
  interaction-session.json
  screenshot-or-dom-summary.txt
  validation-summary.md
```
