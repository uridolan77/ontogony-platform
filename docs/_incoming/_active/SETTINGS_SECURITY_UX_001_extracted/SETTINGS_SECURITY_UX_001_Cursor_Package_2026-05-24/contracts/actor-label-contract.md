# Actor Label Contract

## Actor kinds

```ts
export type ActorDisplayKind =
  | "current_operator"
  | "request_actor"
  | "historical_run_actor"
  | "service_actor"
  | "unknown_actor";
```

## Labels

| Kind | Label |
| --- | --- |
| `current_operator` | Current operator actor |
| `request_actor` | Request actor |
| `historical_run_actor` | Historical run actor |
| `service_actor` | Service actor |
| `unknown_actor` | Actor identity |

## Rule

Do not display a bare `Actor` label on pages where more than one actor context can appear.
