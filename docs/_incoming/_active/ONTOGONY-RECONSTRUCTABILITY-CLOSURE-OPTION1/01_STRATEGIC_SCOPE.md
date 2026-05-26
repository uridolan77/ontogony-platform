# Strategic scope

## Why this package exists

The backend repos are already strong alpha implementations. The next score jump does not come from more routes or more pages. It comes from proving that the system's most important claim is real:

```text
A consequential AI/system decision can be reconstructed after the fact.
```

This requires a cross-service spine:

```text
Allagma emits governed execution decision events.
Conexus emits model access decision events.
Kanon classifies all decision events.
Platform enforces shared mechanics/conformance.
```

## In scope

- Allagma decision-event classifier integration against Kanon.
- Conexus decision-event emitters for route/model/quota/cache/streaming decisions.
- Cross-service golden trace evidence.
- Platform conformance kits for mechanics used in reconstructability.
- Docs/current-state updates that prevent agents from following stale status.

## Out of scope

- Production IAM/RBAC hardening.
- Real external tool execution.
- Provider SDK expansion.
- Vendor-specific feature parity.
- Full frontend panel before backend proof.
- Moving `/ontology/v0` to `/ontology/v1`.
- Turning Platform into a semantic/event authority.

## Success definition

The package is successful when the following statement is true and machine-checked:

```text
For a representative cross-service run, every high/critical decision event emitted by Allagma and Conexus classifies PASS or WARN through Kanon, with no dangling fragments and with trace/correlation identity preserved.
```
