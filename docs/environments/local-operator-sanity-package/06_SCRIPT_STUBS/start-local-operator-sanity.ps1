param([string]$DevRoot="C:\dev",[switch]$StartFrontend)
$ErrorActionPreference="Stop"; Set-StrictMode -Version Latest
$allagma=Join-Path $DevRoot "allagma-dotnet"
$kanon=Join-Path $DevRoot "kanon-dotnet"
$conexus=Join-Path $DevRoot "conexus-dotnet"
$frontend=Join-Path $DevRoot "ontogony-frontend"
foreach($p in @($allagma,$kanon,$conexus,$frontend)){ if(!(Test-Path $p)){ throw "Missing repo: $p" } }
$processes=@()
$processes += Start-Process powershell -PassThru -ArgumentList @("-NoExit","-Command","`$env:ASPNETCORE_ENVIRONMENT='Development'; Set-Location '$kanon'; dotnet run --project src/Kanon.Api/Kanon.Api.csproj --urls http://localhost:5081")
$processes += Start-Process powershell -PassThru -ArgumentList @("-NoExit","-Command","`$env:ASPNETCORE_ENVIRONMENT='Development'; Set-Location '$conexus'; dotnet run --project src/Conexus.Api/Conexus.Api.csproj --urls http://localhost:5082")
$processes += Start-Process powershell -PassThru -ArgumentList @("-NoExit","-Command","`$env:ASPNETCORE_ENVIRONMENT='Development'; `$env:Allagma__Evaluation__ManualWriteEnabled='true'; `$env:Kanon__BaseUrl='http://localhost:5081'; `$env:Conexus__BaseUrl='http://localhost:5082'; `$env:Conexus__ProjectApiKey='cx-dev-key-change-me'; Set-Location '$allagma'; dotnet run --project src/Allagma.Api/Allagma.Api.csproj --urls http://localhost:5083")
if($StartFrontend){ $processes += Start-Process powershell -PassThru -ArgumentList @("-NoExit","-Command","Set-Location '$frontend'; npm install; npm run dev") }
$out=Join-Path $allagma "artifacts/env/local-operator-sanity"
New-Item -ItemType Directory -Force -Path $out | Out-Null
$processes | Select-Object Id,ProcessName,StartTime | ConvertTo-Json -Depth 4 | Set-Content (Join-Path $out "processes.json") -Encoding UTF8
Write-Host "Started local operator sanity stack."
