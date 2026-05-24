# Actor Preset Contract

## Presets

```ts
export type OperatorActorPresetId =
  | "kanon_readonly"
  | "local_admin"
  | "system_service"
  | "custom";
```

## Preset definitions

| ID | Label | Actor ID | Roles |
| --- | --- | --- | --- |
| `kanon_readonly` | Read-only semantic authority | `local-operator` | `Auditor, ProvenanceReader` |
| `local_admin` | Local admin/operator | `local-operator` | `Admin` |
| `system_service` | System service | `local-system` | `System` |
| `custom` | Custom | user-provided | user-provided |

## UI display

Always show:

```text
Current actor
Current roles sent
Selected profile
Recommended read-only profile
Capability summary
```

## Behavior

- Preset apply is explicit.
- Manual actor/role edits set profile to `custom`.
- Do not silently downgrade or upgrade roles.
