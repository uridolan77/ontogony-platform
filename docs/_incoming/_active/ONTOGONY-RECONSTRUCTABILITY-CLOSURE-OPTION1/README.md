# ONTOGONY-RECONSTRUCTABILITY-CLOSURE-OPTION1

## Purpose

This package implements Option 1 from the backend 9+ discussion:

> Close the cross-service reconstructability spine before adding more product features.

The target is to move the four backend repos toward 9+ by proving that consequential decisions are reconstructable across:

```text
Kanon             = semantic/reconstructability authority
Allagma           = governed execution and run lifecycle
Conexus           = model access and route/model-call authority
Ontogony.Platform = shared mechanical contracts and conformance kits
```

This is not a production-hardening package and not a UI-first package. It is a system-closure package.

## Repos in scope

```text
C:\dev\allagma-dotnet
C:\dev\kanon-dotnet
C:\dev\conexus-dotnet
C:\dev\ontogony-platform
```

Optional follow-up, not first-class implementation scope:

```text
C:\dev\ontogony-frontend
C:\dev\ontogony-ui
```

## Strategic objective

By the end of the package, the system should prove:

```text
1. Allagma can emit normalized decision events for governed execution.
2. Kanon can classify those Allagma events with no high/critical FAIL.
3. Conexus can emit normalized decision events for route/model access.
4. Kanon can classify Conexus events with no high/critical FAIL.
5. A cross-service golden trace binds runId, traceId, Kanon decisions, Conexus model calls, and reconstructability results.
6. Platform conformance kits prove the shared mechanics used by all services.
```

## High-level order

1. Allagma → Kanon classifier closure.
2. Conexus decision-event emitters.
3. Cross-service golden trace.
4. Platform consumer conformance kits.
5. Frontend reconstructability panel only after backend proof.

Do not reorder these phases casually.
