# Error envelope schema contract

## Purpose

Define a common mechanical shape for service errors.

## Must include

- stable error code;
- message;
- trace/correlation identifiers;
- optional details;
- optional retry metadata;
- no raw secrets;
- no provider response payloads unless redacted.

## Must not include

- semantic authority decisions;
- model routing policy;
- workflow policy;
- domain-specific remediation logic.
