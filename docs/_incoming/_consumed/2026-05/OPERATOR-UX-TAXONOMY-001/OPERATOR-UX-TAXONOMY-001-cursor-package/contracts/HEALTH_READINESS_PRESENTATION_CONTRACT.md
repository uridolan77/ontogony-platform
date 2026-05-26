# Health/readiness presentation contract

## Rule

Do not summarize a service as `healthy` unless all required dimensions for the current context are clean.

## Recommended service card model

```ts
interface OperatorServiceStatusCard {
  service: 'conexus' | 'kanon' | 'allagma' | string;
  connectivity: ConnectivityStatus;
  readiness: ReadinessStatus;
  contractHealth: ContractHealthStatus;
  operatorUsability: OperatorUsabilityStatus;
  dataSource: DataSourceStatus;
  checkedAt?: string;
  reasons: string[];
  nextActions: string[];
}
```

## Examples

### Good live-ready state

```text
Kanon
Connectivity: live
Readiness: ready
Contract: valid
Usability: usable
```

### Good live-but-not-ready state

```text
Conexus
Connectivity: live
Readiness: not ready — provider route check failed
Contract: warning — /health missing version metadata
Usability: degraded
```

### Bad summary

```text
Conexus healthy
Live · health payload format warning
not ready
```

That mixes contradictory states without hierarchy.
