# Failure policy

## Release evidence modes

Only `mode = Locked` can produce release evidence.

| Mode | May produce release evidence? |
|---|---:|
| Locked | yes |
| ExpectedRefs | no |
| CurrentWorkspace | no |
| PackageOnly | no |
| CohesionOnly | no |

## Scheduled workflow failures

A scheduled cohesion failure should:

1. preserve artifacts;
2. classify scenario and failure class;
3. avoid updating lock automatically;
4. open or update an issue only if the repo has automation permission;
5. not block normal PRs unless configured as a release branch protection gate.

## Capacity baseline failures

Conexus capacity failure is a system release failure only when:

- run mode is `Locked`;
- threshold file is from the locked Conexus commit;
- fake provider/local environment prerequisites are satisfied;
- validator confirms the report is complete.

Otherwise classify as `capacity.inconclusive`.

## Package-mode failures

Package-mode failure blocks release if any of the following is true:

- package version mismatch with runtime lock;
- restore uses sibling ProjectReferences;
- `Kanon.Client`/`Conexus.Client` types are incompatible with Allagma;
- generated package feed is incomplete;
- Allagma package-mode tests fail.

## Drift

Drift is expected in moving-main nightly runs. It should be reported, not treated as release failure unless the workflow is explicitly running in `Locked` release mode.
