# Prompt — Allagma RC Closeout

Repo: `uridolan77/allagma-dotnet`

Complete P3-SB-006 RC closeout for the sandbox evidence phase. Verify dashboard status, evidence commit hash, service-reconstruction wording, marker path fallback, regression test, OpenAPI exposure, real external execution blocked, and production sandbox execution forbidden.

Commands:

```powershell
dotnet test tests/Allagma.Tests/Allagma.Tests.csproj --filter "FullyQualifiedName~Phase3RestartAuditTests|FullyQualifiedName~Phase3MinimalExecuteTests"
pwsh ./scripts/validate-phase3-entry-guard.ps1
```
