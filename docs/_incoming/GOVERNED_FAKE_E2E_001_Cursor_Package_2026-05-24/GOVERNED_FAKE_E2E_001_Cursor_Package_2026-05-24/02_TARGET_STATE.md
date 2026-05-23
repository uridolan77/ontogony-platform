# 02 — Target State

## Operator-visible result

A new or existing local operator path should support:

```text
Start governed fake run
  -> completed Allagma run
  -> Kanon semantic decision
  -> Conexus fake model call
  -> route decision
  -> provider attempt
  -> Evidence Spine full graph
```

## Target operator experience

From the console:

```text
Allagma / Start Run
  Use preset: summarize player risk
  Provider mode: fake
  Start and open Evidence Spine
```

or:

```text
System / Evidence Spine
  Use latest completed governed fake run
  Resolve
```

The operator sees:

```text
Resolution summary
Root: Allagma run — <runId>
7+ nodes resolved
0 unexpected source failures
0 required live links missing
```

Expected nodes:

| Node | Required? | Notes |
| --- | --- | --- |
| Allagma run | yes | real live run, not fixture |
| Allagma event(s) | yes | at least lifecycle/planning/model-call event |
| Platform trace | yes | shared trace id |
| Platform correlation | yes | shared correlation id |
| Kanon decision | yes | planning/semantic decision |
| Conexus model call | yes | fake model call |
| Conexus route decision | yes | route detail resolves |
| Conexus provider attempt | yes | fake / fake.chat |

Expected edges:

| Edge | From | To |
| --- | --- | --- |
| `has_trace` | Allagma run / model call / Kanon decision | trace |
| `has_correlation` | Allagma run / model call / Kanon decision | correlation |
| `used_kanon_decision` | Allagma run | Kanon decision |
| `used_model_call` | Allagma run | Conexus model call |
| `used_route_decision` | Conexus model call | route decision |
| `has_provider_attempt` | Conexus model call | provider attempt |
| `derived_from` | model call | execution journal run |

## Non-target

This package does not make every historical run perfect. It proves the current live governed fake path and improves evidence handling enough that missing data is accurately classified.
