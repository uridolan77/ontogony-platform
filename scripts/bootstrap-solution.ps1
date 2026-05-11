$ErrorActionPreference = "Stop"

if (Test-Path "Ontogony.Platform.sln") {
  Remove-Item "Ontogony.Platform.sln"
}

dotnet new sln -n Ontogony.Platform

Get-ChildItem -Recurse -Filter *.csproj | ForEach-Object {
  dotnet sln Ontogony.Platform.sln add $_.FullName
}

dotnet restore Ontogony.Platform.sln
