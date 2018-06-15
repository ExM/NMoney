$ErrorActionPreference = "Stop"
$mainFolder = Resolve-Path (Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

& dotnet test $mainFolder/NMoney.Tests/NMoney.Tests.csproj -v d
