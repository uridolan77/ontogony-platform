# 07 — Frontend and Operator Plan

## Goal

The operator console must make the live governed fake path easy to run, inspect, and distinguish from fixture replay.

## Candidate frontend areas

```text
ontogony-frontend/src/evidence-spine/
ontogony-frontend/src/system/pages/
ontogony-frontend/src/allagma/
ontogony-frontend/src/kanon/
ontogony-frontend/src/conexus/
ontogony-frontend/src/shared/
ontogony-ui/src/
```

## Required UI changes

### Evidence Spine

Add or improve:

- root kind: Allagma run;
- latest live governed fake run shortcut;
- reason-code display;
- not-applicable vs missing handling;
- route-decision node detail;
- placeholder/resolved node merge;
- modelCall/request/execution id labeling.

### Agent Interaction

Add live mode first:

```text
Mode: Live lookup
Mode: Fixture replay
Mode: Imported JSONL
```

Fixture replay must have:

```text
Demo fixture — not live evidence
```

### Start Run

Add buttons:

```text
Start run
Start and open run detail
Start and open Evidence Spine
Start and open Agent Interaction
```

Add request preview and model-purpose validation.

### Run detail

Show:

- trace id;
- correlation id;
- planning decision id;
- model call id;
- route decision id if available;
- evidence links.

## UI language

Do not say:

```text
missing Kanon decision
```

for direct Conexus-only calls.

Say:

```text
Kanon decision not applicable for direct Conexus model call.
```

For governed fake run, Kanon decision is expected and missing should be marked as missing.

## Acceptance

A user can perform the entire proof from UI without copying IDs manually.
