# Example Run Event Sequence

Expected low-risk default run:

```text
Allagma.RunCreated
Allagma.TaskClassified
Allagma.TopologySelected
Allagma.KanonPlanRequested
Allagma.KanonPlanCompiled
Allagma.WorkflowCheckpointRecorded
Allagma.ToolIntentProposed
Allagma.ToolIntentEvaluationRequested
Allagma.ToolIntentEvaluationCompleted
Allagma.ToolIntentAllowed
Allagma.ModelRequested
Allagma.ModelCompleted
Allagma.RunCompleted
Allagma.EvaluationStarted
Allagma.EvaluationCompleted
```

Expected high-risk run:

```text
Allagma.RunCreated
Allagma.TaskClassified
Allagma.TopologySelected
Allagma.TopologyAuthorizationRequested
Allagma.TopologyAuthorizationCompleted
Allagma.KanonPlanRequested
...
Allagma.RunHumanGatePaused
```

Expected denied topology:

```text
Allagma.RunCreated
Allagma.TaskClassified
Allagma.TopologySelected
Allagma.TopologyAuthorizationRequested
Allagma.TopologyAuthorizationCompleted
Allagma.RunFailed
```
