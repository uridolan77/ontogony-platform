# Redaction Preview Contract

## TypeScript shape

```ts
export type RedactionReason =
  | "allowed_field"
  | "allowlist_excluded"
  | "force_redact_field"
  | "secret_like_field_name"
  | "secret_like_value"
  | "oversize_value"
  | "unsupported_value_type";

export type RedactionPreviewItem = {
  path: string;
  reason: RedactionReason;
  label?: string;
};

export type RedactionPreview = {
  schemaVersion: "redaction-preview.v1";
  safeToSend: boolean;
  kept: RedactionPreviewItem[];
  removed: RedactionPreviewItem[];
  warnings: string[];
  outboundPreview: unknown;
};
```

## Required secret-like names

At minimum:

```text
apikey
api_key
secret
token
password
passwd
connectionstring
connection_string
privatekey
private_key
bearer
authorization
cookie
```

Case-insensitive.

## UI requirements

Show:

- Kept fields
- Removed fields
- Reason
- Outbound preview
- Warnings

Never show redacted raw values in the removed list.
