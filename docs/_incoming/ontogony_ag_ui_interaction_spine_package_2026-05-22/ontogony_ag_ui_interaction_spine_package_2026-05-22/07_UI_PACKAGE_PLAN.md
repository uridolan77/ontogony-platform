# 07 — @ontogony/ui Agent Package Plan

**Repo:** `ontogony-ui`

## New export subpath

Add:

```json
"./agent": {
  "types": "./dist/components/agent/index.d.ts",
  "import": "./dist/components/agent/index.js",
  "require": "./dist/components/agent/index.cjs"
}
```

## File layout

```text
src/components/agent/
  AgentEventTimeline.tsx
  AgentEvidenceLinksPanel.tsx
  AgentInterruptCard.tsx
  AgentMessageStream.tsx
  AgentRunStatusHeader.tsx
  AgentStateDiffPanel.tsx
  AgentToolCallCard.tsx
  AgentUiIntentRenderer.tsx
  AgentWorkbenchLayout.tsx
  eventViewModels.ts
  types.ts
  index.ts
```

## Component responsibilities

### `AgentEventTimeline`

A compact chronological view of events, grouped by run/step when possible.

Props:

```ts
interface AgentEventTimelineProps {
  events: AgentTimelineEventViewModel[];
  selectedEventId?: string;
  onSelectEvent?: (eventId: string) => void;
  density?: "comfortable" | "compact";
}
```

### `AgentInterruptCard`

Displays pending/resolved/cancelled interrupts with schema-driven form rendering delegated to the consuming app.

Props:

```ts
interface AgentInterruptCardProps {
  interrupt: AgentInterruptViewModel;
  renderResponseForm?: (interrupt: AgentInterruptViewModel) => React.ReactNode;
  onApprove?: () => void;
  onReject?: () => void;
  onCancel?: () => void;
}
```

### `AgentToolCallCard`

Displays tool/model/action calls, arguments, status, result, and evidence links.

### `AgentEvidenceLinksPanel`

Displays service-owned evidence links:

- Allagma run/audit/eval
- Kanon decision/provenance/semantic graph
- Conexus model call/route/evidence bundle
- Evidence Spine graph

### `AgentUiIntentRenderer`

Registry-driven renderer. It accepts a typed intent and trusted registry from the consuming app.

```ts
interface AgentUiIntentRendererProps {
  intent: AgentUiIntentViewModel;
  registry: Record<string, React.ComponentType<any>>;
  fallback?: React.ReactNode;
}
```

No arbitrary runtime component loading.

## View-model contract

The UI package should not import product API DTOs. It should expose product-neutral view models.

```ts
export interface AgentTimelineEventViewModel {
  id: string;
  type: string;
  label: string;
  timestamp?: string;
  source?: string;
  severity?: "debug" | "info" | "warning" | "error";
  status?: "pending" | "running" | "completed" | "failed" | "cancelled";
  description?: string;
  evidenceLinks?: AgentEvidenceLinkViewModel[];
  raw?: unknown;
}
```

## Storybook stories

Create stories for:

- successful run timeline
- model-call with route decision
- human gate pending approval
- approval with edits
- interrupted run resumed
- missing evidence links
- redacted payloads
- generative UI intent fallback

## Test coverage

- timeline grouping and ordering
- interrupt actions and accessible labels
- evidence links render service labels
- tool-call argument/result rendering
- state diff collapsed/expanded behavior
- export subpath smoke import

## Design constraints

- Product-neutral naming.
- No direct routing dependency except optional link `href` rendering.
- Works with compact operator layouts.
- Does not introduce another visual vocabulary that conflicts with existing system/execution/observability components.
