$ErrorActionPreference = "Stop"
$mainFolder = Resolve-Path (Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)
$vswhere = Join-Path $mainFolder "packages\vswhere\tools\vswhere.exe"
$version = "15.0"
if (-Not (Test-Path $vswhere)) { throw "vswhere not found, ensure packages are restored. Expected path is: " + $vswhere }
$vspath = & "$vswhere" -version "$version" -property installationPath
if ($vspath -Eq $null) { throw "VS or VS Build Tools $version was not found" }

$msbuildExe = Join-Path $vspath "MSBuild\$version\Bin\MSBuild.exe"
if (-Not (Test-Path $msbuildExe)) { throw "MSBuild not found, expected path: " + $msbuildExe }

& "$msbuildExe" /target:"Clean,Restore,Build" /p:Configuration=Release /p:Platform="Any CPU" $mainFolder\NMoney.sln
