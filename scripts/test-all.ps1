$ErrorActionPreference = "Stop"

dotnet restore Ontogony.Platform.sln
dotnet build Ontogony.Platform.sln --no-restore
dotnet test Ontogony.Platform.sln --no-build --collect:"XPlat Code Coverage"
