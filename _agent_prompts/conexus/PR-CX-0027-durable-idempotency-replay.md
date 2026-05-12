# Implementation prompt: Durable idempotency with replay

Implement exactly this Conexus PR:

`conexus-dotnet/docs/planning/robustness/pr-specs/PR-CX-0027-durable-idempotency-replay.md`

Run:
- dotnet restore Conexus.sln
- dotnet build Conexus.sln --no-restore
- dotnet test Conexus.sln --no-build

Update BUILD_VALIDATION and summarize boundary impact.
