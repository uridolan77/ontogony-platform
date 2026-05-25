# Contract Discipline > 9 master plan

## Scoring definition

A repo scores above 9 in contract discipline when:

- every public HTTP route is inventoried and classified;
- OpenAPI snapshots are regenerated and tested through a documented path;
- typed clients have coverage manifests and safe server-only classification;
- package versions and runtime lock are validated by machine-readable gates;
- breaking changes require explicit migration/changelog/downstream impact;
- error envelopes and intentional DTO-shaped errors are contractually documented and tested;
- propagation headers are frozen and conformance-tested;
- narrative docs cannot contradict generated artifacts without failing tests.

## Primary deliverables

1. `CONTRACT_TRUTH_SOURCE.md` per repo.
2. Generated manifest validation per repo.
3. Route/OpenAPI/client coverage drift tests.
4. Error/header contract conformance tests.
5. Package-mode proof where applicable.
6. Allagma runtime lock promotion with post-lock delta reset.
7. Final closeout score report.
