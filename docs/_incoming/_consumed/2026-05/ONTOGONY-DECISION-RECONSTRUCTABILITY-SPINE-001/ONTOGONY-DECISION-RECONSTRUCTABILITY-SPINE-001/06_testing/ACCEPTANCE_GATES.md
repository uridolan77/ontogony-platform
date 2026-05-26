# Acceptance Gates

## Backend gates

### Contract gates

- New contracts compile.
- JSON serialization is stable.
- Enum values are documented and tested.
- Route inventories/OpenAPI snapshots are updated where applicable.

### Classifier gates

Tests must prove:

- all seven properties classify correctly;
- every property supports at least three of four F/P/S/O states where meaningful;
- score calculation is deterministic;
- governance status is separate from strict score;
- safe reasoning policy never requires hidden chain-of-thought.

### Evidence assembly gates

At least three source ids must be reconstructable:

```text
run id
model call or route decision id
kanon decision id
```

### Missing evidence gates

Each blocking missing property must emit:

- property name;
- classification;
- owner service;
- missing fragment type;
- suggested fix.

## Frontend gates

- A report with all complete fields renders cleanly.
- A report with blocking missing evidence is clearly visible without scrolling through raw traces.
- Raw fragments are accessible but collapsed.
- F/P/S/O labels are understandable.
- Fixture/demo data is marked.
- No hidden chain-of-thought language implies Ontogony stores private model reasoning.

## System gate

A local operator can:

1. open a run or evidence spine node;
2. click “Decision reconstruction”;
3. see status, score, property matrix, missing evidence, and linked fragments;
4. understand whether the issue is in Allagma, Kanon, or Conexus.

## Suggested quality bar

For high/critical actions, completion should not be accepted unless all blocking properties are `F` and post-condition is `F` or explicitly structural/non-mutating.
