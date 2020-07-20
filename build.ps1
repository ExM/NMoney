$ErrorActionPreference = "Stop"
$mainFolder = Resolve-Path (Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

& dotnet clean
if ($lastexitcode -ne 0) { throw "Error in 'dotnet clean'" }

& dotnet restore
if ($lastexitcode -ne 0) { throw "Error in 'dotnet restore'" }

& dotnet build -c:Release $mainFolder\NMoney.sln
if ($lastexitcode -ne 0) { throw "Error in 'dotnet build -c:Release $mainFolder\NMoney.sln'" }
