# Ontogony.Errors — semantic contract

**Status:** Production-safe for HTTP exception middleware, `ApiError` / ProblemDetails mapping, and configurable exception classification.

## Guarantees

- Consistent **mechanical** error surfaces for APIs (status codes, safe public messages, optional detail redaction).
- Middleware behavior that avoids writing duplicate bodies when responses have already started.

## Does not guarantee

- Which exceptions map to which HTTP status codes in your domain (service repos own mappings via options).

## Related

- [../adoption/error-middleware-adoption.md](../adoption/error-middleware-adoption.md)
- [../adoption/observability-error-ordering.md](../adoption/observability-error-ordering.md)
