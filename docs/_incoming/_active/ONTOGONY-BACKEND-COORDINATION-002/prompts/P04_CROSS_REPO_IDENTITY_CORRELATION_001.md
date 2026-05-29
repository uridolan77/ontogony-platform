# P04 — CROSS-REPO-IDENTITY-CORRELATION-001

Implement slice 4.

## Read

- `slices/CROSS-REPO-IDENTITY-CORRELATION-001/README.md`
- `contracts/CROSS_SERVICE_CONTEXT_PROPAGATION_V1.md`
- `allagma-dotnet/docs/system/SYSTEM_TRACE_CONTEXT_MATRIX.md`

## Task

1. Finalize propagation contract v1 on platform.
2. Ensure Allagma HTTP clients to Kanon/Conexus/Aisthesis/Metabole forward:
   - Trace ID
   - Correlation ID
   - Idempotency key (mutations)
   - Actor ID (when present)
3. Update trace context matrix to match `Ontogony.Http` constants.
4. Run correlation chain acceptance:
   ```powershell
   ./scripts/run-system-coh-001-acceptance.ps1 -DevRoot C:\dev -IncludeCorrelationChain
   ```
5. Evidence closeout in Allagma + platform.

## Done when

`IDN-001`–`IDN-005` PASS.
