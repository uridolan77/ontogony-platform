# Credential Source Taxonomy

## Enum

```ts
export type OperatorCredentialSource =
  | "not_set"
  | "default_dev"
  | "session"
  | "local_browser"
  | "env_injected"
  | "service_configured"
  | "unknown_legacy";
```

## Meanings

| Source | Meaning | Severity |
| --- | --- | --- |
| `not_set` | No credential has been configured. | warning/info depending on need |
| `default_dev` | Development default value is in use. | warning |
| `session` | Value is kept in current browser session only. | info |
| `local_browser` | Value is persisted in local browser storage. | warning |
| `env_injected` | Value is provided to backend/service process by environment variable; raw value not visible to console. | neutral/info |
| `service_configured` | Service reports credential/config present but raw value is not visible. | neutral/info |
| `unknown_legacy` | Existing state could not be classified. Should be remediated. | warning |

## Forbidden display

Never display:

```text
unknown source
```

Use:

```text
Source: source not classified
```

only for `unknown_legacy`, with remediation.
