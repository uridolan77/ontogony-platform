# Evaluation Harness

## First slice harness

Use deterministic fake-provider evaluation. Do not require live OpenAI calls for test completion.

## Dataset split semantics

```text
train split      = rollout evidence used for candidate edit generation
selection split  = held-out gate used for acceptance/rejection
test split       = optional final reporting only
```

## Evaluation task shape

```json
{
  "taskId": "selection_task_001",
  "taskKind": "spreadsheet-qa",
  "inputRef": "fixtures/spreadsheets/hidden-sheet-demo.xlsx",
  "question": "What is the final adjusted revenue?",
  "expectedAnswer": "12345",
  "verifier": "exact-match-fixture",
  "metadata": {
    "requiresHiddenSheetInventory": true
  }
}
```

## Result shape

```json
{
  "taskId": "selection_task_001",
  "skillVersionId": "skillver_demo_spreadsheet_v1",
  "score": 1.0,
  "verifier": "exact-match-fixture",
  "failureLabels": [],
  "safeRationaleSummary": "The skill directed the agent to inspect hidden sheets before answering."
}
```

## Future extension

Later versions can support:

- model-graded rubrics;
- human review gates;
- statistical confidence intervals;
- bootstrap comparisons;
- domain-specific task generators;
- multi-objective gates for quality, cost, latency, safety, and compliance.

Do not add these in the first slice unless already easy from existing infrastructure.
