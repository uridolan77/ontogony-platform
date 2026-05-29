# Patch guide — required-edge matrix v2

1. Do not remove v1 evaluator.
2. Add v2 as additive evaluator/report fields.
3. Add profile-aware `requiredWhen` semantics.
4. Add `notApplicableReason` support.
5. Ensure diagnostics include producer owner and suggested fix.
6. Add fixtures for complete/missing/not-applicable cases.
7. Keep bundle fingerprint stable across export time.
