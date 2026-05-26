# Golden Fixtures

## Fixture: spreadsheet-analysis improvement

### Base skill failure

Base skill answers spreadsheet questions without first inventorying all sheets. It fails hidden-sheet and named-range tasks.

### Candidate edit

Add a procedure step:

```text
Before calculating or answering, inventory all worksheets, hidden sheets, named ranges, filters, and formulas that may affect the requested value.
```

### Selection scores

```text
incumbentScore = 0.66
candidateScore = 0.83
delta = +0.17
status = accepted
```

## Fixture: bad candidate regression

### Bad edit

Delete the workbook inventory instruction.

### Selection scores

```text
incumbentScore = 0.66
candidateScore = 0.33
delta = -0.33
status = rejectedRegression
```

## Fixture: tie rejection

### Neutral edit

Add wording that does not affect answer quality.

### Selection scores

```text
incumbentScore = 0.66
candidateScore = 0.66
delta = 0.00
status = rejectedTie
```

## Fixture ids

Use stable ids:

```text
skillArtifactId: demo.spreadsheet-analysis
baseVersionId: skillver_demo_spreadsheet_v0
acceptedVersionId: skillver_demo_spreadsheet_v1
badCandidateVersionId: skillver_demo_spreadsheet_bad_001
skillOptimizationRunId: skillopt_run_demo_001
acceptedGateId: skillgate_demo_accepted_001
rejectedGateId: skillgate_demo_rejected_001
rejectedEditBufferId: rejbuf_demo_001
```

## Required evidence links

The accepted fixture must link:

```text
Allagma run
Conexus optimizer call
Conexus target rollout calls
Kanon acceptance decision
Skill evaluation gate
Skill deployment binding
```

The rejected fixture must link:

```text
Allagma run
Conexus optimizer call
Kanon rejection decision
Skill evaluation gate
Rejected edit buffer
```
