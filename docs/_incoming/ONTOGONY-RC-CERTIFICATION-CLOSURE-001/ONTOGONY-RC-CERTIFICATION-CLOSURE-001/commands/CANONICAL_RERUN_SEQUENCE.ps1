$WorkspaceRoot = "C:\dev"
pwsh "$WorkspaceRoot\scripts\verify-ontogony-runtime-service-identity.ps1" -WorkspaceRoot $WorkspaceRoot -RequireAll -WriteEvidence
cd "$WorkspaceRoot\allagma-dotnet"; dotnet test Allagma.sln -c Release
cd "$WorkspaceRoot\kanon-dotnet"; dotnet test Kanon.sln -c Release
cd "$WorkspaceRoot\conexus-dotnet"; dotnet test Conexus.sln -c Release -p:NoWarn=CS1591 --filter "Category!=ExternalProviderSmoke&Category!=LoadSoak&Category!=PersistenceSmoke&Category!=CapacityBaseline"
cd "$WorkspaceRoot\metabole-dotnet"; dotnet test -c Release
cd "$WorkspaceRoot\aisthesis-dotnet"; dotnet test -c Release
cd "$WorkspaceRoot\allagma-dotnet"; pwsh .\scripts\run-package-mode-build.ps1
cd "$WorkspaceRoot\conexus-dotnet"; pwsh .\scripts\run-conexus-gateway-hardening-acceptance.ps1 -UsePostgres
cd "$WorkspaceRoot\allagma-dotnet"; pwsh .\scripts\run-five-service-stack.ps1 -DevRoot $WorkspaceRoot
pwsh "$WorkspaceRoot\scripts\verify-ontogony-runtime-service-identity.ps1" -WorkspaceRoot $WorkspaceRoot -RequireAll -WriteEvidence
cd "$WorkspaceRoot\aisthesis-dotnet"; pwsh .\scripts\system\run-five-service-live-certification.ps1 -Mode Live
cd "$WorkspaceRoot\allagma-dotnet"; pwsh .\scripts\system\run-system-cohesion-acceptance.ps1 -UseExistingServices -Quick
