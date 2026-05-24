# SOURCE-BINDINGS-POLISH-001 — active workstream

**Status:** implemented (2026-05-24)  
**Primary repo:** `ontogony-frontend`, `ontogony-ui` (SourceBindingCard / SourceBindingTestResult)

## Delivered

- Removed misleading “Backend-waiting list APIs” card when bindings are supported; corrected route label to `GET /ontology/v0/source-bindings` (SBP-001)
- Empty create form + explicit **Load sample binding**; no prefilled production-like defaults (SBP-002)
- Normalized source system / schema / object / field labels; explicit target kind (property vs relationship) (SBP-003)
- Confidence validation 0.00–1.00 (SBP-004)
- Separate review, lifecycle, and test status on binding cards; no inferred test status from review (SBP-005)
- Hide Approve/Reject on approved rows; unavailable actions explained inline (SBP-006)
- Test warning details from Kanon `errors[]`; copyable binding IDs (SBP-007)
- Unit + component tests + e2e create-flow update (SBP-008)

## Verify

```powershell
cd C:\dev\ontogony-frontend
npm run typecheck
npm test -- --run src/kanon/polish/kanonSourceBindingDefaults.test.ts src/kanon/forms/kanonSourceBindingFormValidation.test.ts src/kanon/components/KanonSourceBindingActions.test.tsx src/kanon/components/KanonSemanticSubstrateLimitationsCard.test.tsx src/kanon/adapters/kanonLifecycleAdapters.test.ts

cd C:\dev\ontogony-ui
npm test -- --run src/components/semantic/SourceBindingCard.test.tsx
```

## Next

`SETTINGS-SECURITY-UX-001` per [`NEXT.md`](../NEXT.md).
