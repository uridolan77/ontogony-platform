# PR53 — Pre-Conexus safety and blueprint alignment

## Breaking: `Ontogony.Redaction.RedactionResult`

The `Original` property was removed. `RedactionResult` now carries only the safe `Value`, classification, and metadata. Callers that needed the raw input for tests should keep that value in a local variable before calling `IRedactor`.

## Breaking: `Ontogony.Replay.Contracts.ReplayManifest`

`CreatedAt` is now `DateTimeOffset` (was `string`) for consistent temporal typing and deterministic JSON alongside other platform DTOs.

## `Ontogony.Logging`

- `Ontogony.Logging` now references `Ontogony.Redaction`.
- `BeginOntogonyScope(..., IRedactor? redactor)` applies `IRedactor.RedactFields` to `additionalFields` when `redactor` is non-null.
- `OntogonyLoggingScopeMiddleware` resolves optional `IRedactor` from `HttpContext.RequestServices` and passes it into the scope.

**Repos to update:** Any consumer that constructed `RedactionResult` or `ReplayManifest` positionally, or assumed `RedactionResult.Original` exists. Conexus.NET should register `AddOntogonyRedaction()` (or `AddOntogonySecrets()`, which pulls redaction in) before relying on automatic scope redaction.
