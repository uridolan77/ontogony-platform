# 10 — Implementation Sequence

## Recommended exact order

### 1. `PLAT-EVAL-001`

Add neutral evaluation contracts to Ontogony.Platform. No runtime change.

### 2. `PLAT-TOPO-001`

Add neutral topology vocabulary/contracts. No runtime change.

### 3. `AGM-TOPO-001`

Allagma records task classification and selected topology. Default remains `single_workflow`.

### 4. `AGM-TOPO-002`

Add topology information to run audit bundle and event query responses.

### 5. `KANON-TOPO-001`

Kanon exposes topology-policy evaluation.

### 6. `AGM-TOPO-003`

Allagma calls Kanon topology policy for high-risk or override modes.

### 7. `CX-ROUTE-EVIDENCE-001`

Conexus emits route decision records and links them to model calls.

### 8. `AGM-EVAL-001`

Allagma eval harness runs deterministic scenario evals.

### 9. `SYS-EVAL-001`

Cross-repo eval smoke proves the whole loop.

### 10. `SYS-OBS-EVAL-001`

Add eval metrics and operator docs.

## Stop conditions

Pause the phase if any of these occur:

```text
Ontogony.Platform starts accumulating product semantics.
Allagma bypasses Kanon for high-risk topology authorization.
Conexus route evidence contains raw secrets.
Baseline comparison can trigger duplicate side effects.
Eval artifacts contain raw sensitive payloads by default.
Parallel/decentralized topology is implemented before baseline eval exists.
```

## Promotion condition

Only after `SYS-EVAL-001` passes should you consider implementing `parallel_review`.
