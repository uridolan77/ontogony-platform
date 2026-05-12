# Implementation prompt: OpenAI provider uses Ontogony HTTP pipeline

Implement exactly this Conexus PR:

`conexus-dotnet/docs/planning/robustness/pr-specs/PR-CX-0021-openai-uses-ontogony-http-pipeline.md`

Run:
- dotnet restore Conexus.sln
- dotnet build Conexus.sln --no-restore
- dotnet test Conexus.sln --no-build

Update BUILD_VALIDATION and summarize boundary impact.
