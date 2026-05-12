# Implementation prompt: Remove raw secret logging

Implement exactly this Conexus PR:

`conexus-dotnet/docs/planning/robustness/pr-specs/PR-CX-0022-remove-raw-secret-logging.md`

Run:
- dotnet restore Conexus.sln
- dotnet build Conexus.sln --no-restore
- dotnet test Conexus.sln --no-build

Update BUILD_VALIDATION and summarize boundary impact.
