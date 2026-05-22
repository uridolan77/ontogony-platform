# @ontogony/agent-interaction

Protocol-neutral TypeScript mechanics for the Ontogony **agent interaction spine** and **AG-UI compatibility adapter**.

- Canonical types align with `docs/schemas/ontogony-agent-interaction-event-v0.schema.json`.
- AG-UI mapping is pure and fixture-tested (`ADAPTER-AGUI-001`).
- Ontogony events remain canonical; AG-UI is an export/wire view.

## Install (monorepo)

```json
"@ontogony/agent-interaction": "file:../ontogony-platform/packages/ontogony-agent-interaction"
```

Run `npm install` in the consumer repo (triggers `prepare` → `build`).

## Usage

```ts
import {
  toAgUiEvents,
  stripOntogonyProvenanceFromAgUiEvents,
  serializeAgUiEventsToJsonl,
} from "@ontogony/agent-interaction";

const wire = stripOntogonyProvenanceFromAgUiEvents(toAgUiEvents(ontogonyEvents));
const jsonl = serializeAgUiEventsToJsonl(wire);
```

AG-UI-only import:

```ts
import { toAgUiEvent } from "@ontogony/agent-interaction/ag-ui";
```

## Docs

- [`docs/operators/AGENT_INTERACTION_SPINE_CONTRACT.md`](../../docs/operators/AGENT_INTERACTION_SPINE_CONTRACT.md)
- [`docs/operators/AG_UI_COMPATIBILITY_ADAPTER.md`](../../docs/operators/AG_UI_COMPATIBILITY_ADAPTER.md)

## Scripts

```bash
npm run build
npm run test
npm run typecheck
```
