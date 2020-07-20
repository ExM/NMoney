$ErrorActionPreference = "Stop"

& dotnet tool restore
if ($lastexitcode -ne 0) { throw "Error in 'dotnet tool restore'" }

& dotnet restore
if ($lastexitcode -ne 0) { throw "Error in 'dotnet restore'" }