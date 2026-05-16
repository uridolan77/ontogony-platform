# Ontogony.Errors — semantic contract

**Status:** Production-safe for HTTP exception middleware, `ApiError` / ProblemDetails mapping, and configurable exception classification.

## Guarantees

- Consistent **mechanical** API error payloads (`ApiError` JSON + ProblemDetails bridge) for endpoint responses.
- Stable cross-service internal envelope shape (`CrossServiceErrorEnvelope`) for downstream client mapping.
- Middleware behavior that avoids writing duplicate bodies when responses have already started.

## Does not guarantee

- Which exceptions map to which HTTP status codes in your domain (service repos own mappings via options).
- Product-specific public HTTP error contracts outside the Ontogony mechanical payload/envelope shapes.

## Related

- [../adoption/error-middleware-adoption.md](../adoption/error-middleware-adoption.md)
- [../adoption/observability-error-ordering.md](../adoption/observability-error-ordering.md)
