# Security and Negative Test Matrix

## Auth matrix

| Test | Expected result |
|---|---|
| Missing service token | 401 |
| Invalid service token | 401 |
| Valid token, wrong role/scope | 403 |
| Missing Conexus project key | 401 |
| Invalid Conexus project key | 401 |
| Admin route with project key | 403 |
| Public health route with no token | 200 or documented health response |
| Protected route with no token | 401 |

## Payload safety matrix

| Test | Expected result |
|---|---|
| Empty body where body required | 400 stable error |
| Invalid JSON | 400 stable error |
| Oversized body | 413 or documented error |
| Unknown enum | 400 stable error |
| Unexpected additional fields | accepted only if explicitly designed |
| Prompt injection text in model-bound field | no policy bypass, no unsafe side effect |
| PII in assistance context | redacted or rejected according to policy |

## Side-effect safety matrix

Real tool execution should remain blocked until trust model is implemented.

Required tests before enabling real tools:

- deny-by-default tool registry;
- per-tool secret isolation;
- outbound allowlist;
- no arbitrary filesystem access;
- dry-run vs execute split;
- durable side-effect ledger;
- replay does not re-execute side effects;
- human gate required for consequential actions;
- audit/event evidence exported.

## Frontend security checks

- UI does not expose service tokens in browser-visible config.
- Error toasts do not expose secrets.
- Admin actions are hidden/disabled for non-admin identities.
- Dangerous actions require confirmation or human gate where applicable.
