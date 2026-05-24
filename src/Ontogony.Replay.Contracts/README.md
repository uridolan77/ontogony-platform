# Ontogony.Replay.Contracts

## What this is

Replay manifest DTOs for deterministic debugging and incident reconstruction:

- replay manifest
- input refs
- output refs
- environment snapshot
- determinism hints
- step records

Cross-service replay runtime vocabulary (REPLAY-RUNTIME-001):

- replay modes and target kinds
- eligibility, request, result, delta, and evidence bundle records
- service attempt and evidence reference shapes

See `docs/contracts/REPLAY_RUNTIME_CONTRACT.md`.

## What this is not

- not a replay engine
- not model determinism guarantee
- not workflow orchestration
- not a storage provider
- not product policy

Use this when a service needs to describe what would be replayed, not how to replay it.
