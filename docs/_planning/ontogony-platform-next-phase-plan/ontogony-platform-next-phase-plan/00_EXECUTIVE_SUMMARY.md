# Executive Summary

The next platform phase should be small and mechanical.

The immediate platform value is not adding big frameworks. It is standardizing the cross-service mechanics that Kanon, Agentor, and Conexus will all need:

```text
trace propagation
actor propagation
idempotency header conventions
integration error handling
integration metrics
static architecture/forbidden-dependency tests
```

Most frameworks discussed for the next phase should not enter platform packages:

| Framework | Platform decision |
|---|---|
| .NET Aspire | Use in devhost/apphost, not core packages |
| Microsoft Agent Framework | Agentor only |
| Orleans | Agentor only if needed |
| YARP | Gateway/apphost only |
| Wolverine/MassTransit | Defer unless platform messaging is insufficient |
| Provider SDKs | Conexus only, never platform |

## First PR

Start with:

```text
PLAT-INT-001 — Service-to-service integration conventions
```

because all runtime integration depends on consistent headers, trace, actor, idempotency, and error behavior.
