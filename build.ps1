$ErrorActionPreference = "Stop"
$mainFolder = Resolve-Path (Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

& dotnet clean
& dotnet restore
& dotnet build -c:Release $mainFolder\NMoney.sln
