$ErrorActionPreference = "Stop"
$mainFolder = Resolve-Path (Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)
$paketPath = Join-Path $mainFolder ".paket\paket.exe"
if (-not (Test-Path $paketPath)) { throw "paket.exe not found" }

& "$paketPath" restore
if ($lastexitcode -ne 0) { throw "Error in packet" }
