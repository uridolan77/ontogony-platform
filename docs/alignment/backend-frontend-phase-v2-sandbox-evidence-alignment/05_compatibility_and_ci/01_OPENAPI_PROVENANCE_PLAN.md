# OpenAPI and Provenance Plan

## Backend artifacts
For each backend service: OpenAPI JSON, service name, repo, **sourceCommit** (last commit that changed the snapshot), **provenanceCommit** (commit that wrote the sidecar), generatedAt, **snapshotSha256** (canonical contract identity), contract version, and test evidence path. Legacy `commit` mirrors `sourceCommit`.

## Frontend artifacts
Snapshot source repo, snapshot commit, snapshot hash, generated client version/date, frontend commit, and adapter test results.

## Required for this phase
Allagma must provide an OpenAPI snapshot that includes audit bundle contract, sandbox evidence contract, side-effect ledger row contract, run events endpoint, and capability endpoint if added. Frontend must record provenance when refreshing.
