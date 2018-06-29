$ErrorActionPreference = "Stop"

& git clean -dxf

& dotnet restore
& dotnet build -c:Release
& dotnet pack -c:Release

