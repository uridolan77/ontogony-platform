# Target architecture

## Ownership

```text
ontogony-platform
  Owns release mechanics, evidence schema, release operator docs,
  and cross-repo release workflow templates.

allagma-dotnet
  Owns runtime lock, runtime compatibility matrix, system cohesion
  orchestration, package-mode consumer proof, and scenario summaries.

kanon-dotnet
  Owns semantic contract gates and Kanon compatibility manifest.

conexus-dotnet
  Owns model gateway contract gates and capacity baseline reports.
```

## Release gate topology

```text
run-locked-runtime-release-gate.ps1
  |
  +-- read allagma-dotnet/docs/system/ontogony-runtime.lock.json
  |
  +-- checkout four repos at lockedCommits
  |
  +-- validate lock structure and package versions
  |
  +-- run repo-local gates
  |     +-- Platform build/test/pack
  |     +-- Kanon build/test contract gates
  |     +-- Conexus build/test default filter
  |     +-- Allagma build/test/conformance
  |
  +-- run package-mode release train
  |
  +-- run system cohesion suite
  |     +-- completed run
  |     +-- human gate wait/approve/deny
  |     +-- Kanon assistance
  |     +-- Conexus fallback
  |     +-- restart survival
  |     +-- streaming smoke
  |     +-- Conexus capacity baseline
  |
  +-- write release evidence bundle
  |
  +-- validate evidence bundle against JSON schema
```

## Evidence output

Recommended output:

```text
artifacts/releases/SYSTEM-ALPHA-007/<timestamp>/
  release-evidence-bundle.json
  release-evidence-bundle.md
  runtime-lock.snapshot.json
  repo-commits.json
  repo-gates/
  package-mode/
  system-cohesion/
  capacity/
  restart/
  streaming/
  logs/
```

## Modes

| Mode | Meaning | Use |
|---|---|---|
| `Locked` | Checkout and validate exact `lockedCommits` from runtime lock | Release evidence |
| `ExpectedRefs` | Checkout expected branch refs from runtime lock | Drift detection |
| `CurrentWorkspace` | Use already checked-out sibling dirs | Local debugging |
| `PackageOnly` | Run package-mode train only | PLATFORM-REL-001 validation |
| `CohesionOnly` | Run full-system cohesion only | SYSTEM-ALPHA-008 validation |

Only `Locked` mode may emit release-candidate evidence.
