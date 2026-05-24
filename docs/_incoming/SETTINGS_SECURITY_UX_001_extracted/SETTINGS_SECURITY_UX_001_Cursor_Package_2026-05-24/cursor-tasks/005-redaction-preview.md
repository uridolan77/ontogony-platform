# Cursor Task 005 — Redaction Preview

## Goal

Render a redaction preview before Conexus Assistance sends context.

## Steps

1. Add redaction preview builder.
2. Remove secret-like sample context.
3. Render kept/removed/reasons.
4. Show outbound preview.
5. Add tests.

## Acceptance

- `apiKey`, `token`, `connectionString` fields are removed from preview.
- Assistance sample contains no secret-looking keys or values.
