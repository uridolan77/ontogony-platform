# UX Spec — Decision Reconstruction Panel

## Purpose

Give operators and developers a direct answer to:

```text
Can this action be reconstructed well enough to trust, audit, replay, or debug it?
```

## Entry points

Add links/buttons from existing pages where relevant:

- Evidence Spine graph node details;
- Allagma run detail / operations panel;
- human gate detail;
- Kanon decision lifecycle page;
- Conexus model call / route decision detail;
- cross-service trace lookup.

Label suggestion:

```text
Decision reconstruction
```

Avoid jargon-only labels such as “DES matrix” in primary UI.

## Layout

### Header card

Show:

- decision event id;
- decision kind;
- severity;
- status `PASS/WARN/FAIL`;
- strict score;
- occurred time;
- origin service;
- linked run/trace/correlation ids.

### Property matrix

Seven rows:

```text
Inputs
Policy basis
Operator identity
Authorization envelope
Reasoning evidence
Output action
Post-condition state
```

Columns:

```text
Status      F/P/S/O badge
Governance Blocking? yes/no
Evidence    count + clickable fragments
Rationale   short explanation
```

### Missing evidence panel

Group diagnostics by severity:

- Blocking;
- Warning;
- Info.

Each diagnostic should show:

- property;
- likely owner service;
- missing fragment type;
- suggested fix.

### Evidence fragments panel

List linked fragments by service:

- Allagma fragments;
- Kanon fragments;
- Conexus fragments;
- platform/external fragments.

Each item should include trust level and source id.

### State view

For mutating actions, show:

```text
Before hash / snapshot id
After hash / snapshot id
Delta hash
Verification status
```

For non-mutating actions, show:

```text
No state change — explicitly marked not applicable.
```

### Reasoning evidence notice

Always include a small notice:

```text
Ontogony does not require hidden chain-of-thought. This field shows safe external rationale evidence, such as policy summaries, route explanations, validator reports, or human notes.
```

## Empty/loading/error states

- Loading: “Building reconstruction report…”
- No report: “No decision event is linked to this artifact yet.”
- Classification failure: “The source exists, but Ontogony could not assemble it into a decision event.”
- Fixture data: visibly mark as fixture/demo.

## UX tone

The page should be diagnostic and calm. Avoid crowding it with every raw trace field. Raw fragments should be collapsible.
