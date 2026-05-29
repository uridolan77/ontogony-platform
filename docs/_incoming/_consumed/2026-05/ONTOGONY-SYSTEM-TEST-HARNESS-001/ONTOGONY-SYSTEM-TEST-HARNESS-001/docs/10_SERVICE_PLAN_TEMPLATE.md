# <SERVICE>-TEST-PLAN-001

## 1. Service identity

- Repo:
- Runtime role:
- Owned capabilities:
- Explicit non-owned capabilities:
- Critical dependencies:
- Consumers:

## 2. Testing objective

State what percentage of manual regression this plan should replace and which manual checks remain human-owned.

## 3. Route/API inventory

| Method | Path | Auth | Owner capability | Current test? | Target test |
|---|---|---|---|---|---|

## 4. Current test inventory

| Test project/file | What it covers | Gaps |
|---|---|---|

## 5. Critical flows

| Flow ID | Flow | Dependencies | Risk | Automation priority |
|---|---|---|---|---|

## 6. Happy path tests

| Test ID | Given | When | Then | Evidence required |
|---|---|---|---|---|

## 7. Negative path tests

| Test ID | Fault | Expected error | Evidence required |
|---|---|---|---|

## 8. Idempotency/retry/replay tests

| Test ID | Scenario | Expected result |
|---|---|---|

## 9. Persistence/restart tests

| Test ID | Scenario | Expected result |
|---|---|---|

## 10. Observability tests

| Test ID | Required trace/log/metric/event |
|---|---|

## 11. UI coverage mapping

| Backend capability | UI route/component | Current coverage | Gap |
|---|---|---|---|

## 12. CI gates

| Gate | Tests | Required for merge? |
|---|---|---|

## 13. Acceptance criteria

- [ ] Route matrix complete.
- [ ] Happy path suite implemented.
- [ ] Negative path suite implemented.
- [ ] Contract tests implemented.
- [ ] Evidence bundle generated.
- [ ] Manual checklist reduced.
