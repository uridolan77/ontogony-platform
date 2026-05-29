# P03 — SHARED-ERROR-CONTRACT-001

Implement slice 3. Start in `ontogony-platform`, then adopt per API host.

## Read

- `slices/SHARED-ERROR-CONTRACT-001/README.md`
- `contracts/CROSS_SERVICE_ERROR_ENVELOPE_V1.md`
- `allagma-dotnet/docs/system/SYSTEM_ERROR_COMPATIBILITY_MATRIX.md`
- `kanon-dotnet/docs/integrations/ERROR_CONTRACTS.md`

## Task

1. Finalize platform contract + JSON schema.
2. Audit each API's exception middleware — map to `CrossServiceErrorEnvelope`.
3. Add/update tests: 400, 403, 404, 500 samples per service.
4. Document Kanon typed validation DTO exceptions.
5. Run `validate-cross-service-error-envelope.ps1 -DevRoot C:\dev`.
6. Per-repo closeout evidence.

## Do not

- Change Kanon success/failure DTO shapes for validate/compile endpoints.
- Log raw prompts, secrets, or PII in `detail`.

## Done when

`ERR-001`–`ERR-005` PASS.
