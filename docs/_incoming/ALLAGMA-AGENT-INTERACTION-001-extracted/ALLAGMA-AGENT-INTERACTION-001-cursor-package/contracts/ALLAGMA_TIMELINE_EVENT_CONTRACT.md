# Allagma timeline event contract

## Purpose

Map raw Allagma run events to operator-readable chronological timeline rows.

## Target shape

```ts
export interface AgentInteractionTimelineEvent {
  id: string;
  timestamp?: string;
  sequence?: number;
  sourceSystem: "allagma" | "kanon" | "conexus" | "platform" | "operator" | "import";
  rawEventType?: string;
  phase: AgentInteractionTimelinePhase;
  title: string;
  status: AgentInteractionTimelineStatus;
  summary: string;
  details?: Record<string, unknown>;
  relatedIdentifiers?: Record<string, string | string[] | undefined>;
  evidenceLinks?: AgentInteractionLink[];
  missing?: AgentInteractionMissingReason[];
  rawAvailable: boolean;
  rawRedacted: boolean;
}

export type AgentInteractionTimelinePhase =
  | "run_lifecycle"
  | "classification"
  | "topology"
  | "planning"
  | "workflow_checkpoint"
  | "tool_intent"
  | "action_evaluation"
  | "human_gate"
  | "model_call"
  | "evaluation"
  | "replay"
  | "unresolved";
```

## Required event mappings

Map actual repo event names to these operator titles.

| Raw event idea | Phase | Title |
|---|---|---|
| RunCreated | run_lifecycle | Run created |
| TaskClassified | classification | Task classified |
| TopologySelected | topology | Topology selected |
| TopologyAuthorizationRequested | topology | Topology authorization requested |
| TopologyAuthorizationCompleted | topology | Topology authorization completed |
| KanonPlanRequested | planning | Kanon plan requested |
| KanonPlanCompiled | planning | Kanon plan compiled |
| WorkflowCheckpointRecorded | workflow_checkpoint | Workflow checkpoint recorded |
| ToolIntentProposed | tool_intent | Tool intent proposed |
| ToolIntentEvaluationRequested | action_evaluation | Tool evaluation requested |
| ToolIntentEvaluationCompleted | action_evaluation | Tool evaluation completed |
| ToolIntentBlocked | tool_intent | Tool intent blocked |
| ConexusModelRequested | model_call | Conexus model requested |
| ConexusModelCompleted | model_call | Conexus model completed |
| RunCompleted | run_lifecycle | Run completed |
| EvaluationRunRecorded | evaluation | Evaluation run recorded |
| BaselineComparisonRecorded | evaluation | Baseline comparison recorded |

## Unknown event rule

Unknown events are allowed but must be labelled:

```text
Unmapped event: <rawEventType>
```

and placed in `phase: "unresolved"` or a safe `other` equivalent. Do not drop them silently.
