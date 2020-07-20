$ErrorActionPreference = "Stop"
$mainFolder = Resolve-Path (Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

Remove-Item $mainFolder\Release -recurse -force -ErrorAction 0

& dotnet paket pack $mainFolder\Release
if ($lastexitcode -ne 0) { throw "Error in 'dotnet paket pack $mainFolder\Release'" }
