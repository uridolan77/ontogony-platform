# Migration: PR20 (Ontogony.Errors JSON wire shape + mapping hooks)

## Ontogony.Errors

- **`OntogonyExceptionMappingOptions`** (configure in `AddOntogonyErrors`):
  - **`ErrorCodeJsonKey`** (default `"code"`) — JSON property name for the error code on the wire.
  - **`DetailsJsonKey`** (default `"details"`) — JSON property name for structured details (often a list).
  - **`IncludeInstanceInJson`** (default `true`) — whether to emit `instance` (request path) in JSON.
  - **`UnhandledErrorCode`** (default `"UnhandledError"`) — stable code for unmapped exceptions (HTTP 500).
- **`ExceptionMapping` / `options.Map<TException>(...)`** now accepts optional delegates:
  - **`resolveErrorCode`** — override the static `ErrorCode` per exception instance.
  - **`resolvePublicMessage`** — override `PublicMessage` / exception-message policy per instance.
  - **`detailsFactory`** — supply a serializable details payload (for example a validation error list).
  - **`resolveStatusCode`** — override the mapped HTTP status per instance (defaults to the static `statusCode` argument).
- **`OntogonyExceptionHandlingMiddleware`** no longer serializes `ApiError` directly for the JSON body. It builds a `Dictionary<string, object?>` so the wire keys honor `ErrorCodeJsonKey` / `DetailsJsonKey`. In-memory `ApiError` still uses `Code`, `Details`, and `Instance` for ProblemDetails bridges and logging.

## Downstream repos

- **Services with a legacy JSON schema** (for example `error` + `errors` instead of `code` + `details`): set `ErrorCodeJsonKey` / `DetailsJsonKey` and `IncludeInstanceInJson` in `AddOntogonyErrors`, and replace local exception middleware with `UseOntogonyExceptionHandling()` after request tracing.
- **Consumers of error JSON:** if you asserted on `code` / `details` / `instance`, align expectations with the service’s configured keys or keep defaults unchanged.
