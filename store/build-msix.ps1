<#
  Builds the MSIX package for Microsoft Store.

  Usage (values from Partner Center > Product identity):
    .\store\build-msix.ps1 -IdentityName "12345ladofa.ThePen" -Publisher "CN=XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX" -PublisherDisplayName "ladofa" -Version 1.0.0.0

  The package is not signed here. Microsoft Store signs it after submission.
#>
param(
	# defaults are the values of Partner Center > ThePen > Product identity
	[string]$IdentityName = "Ladofa9.ThePen",
	[string]$Publisher = "CN=EDF078A0-D60C-4C7A-A4B3-963B926AEAA5",
	[string]$PublisherDisplayName = "Ladofa9",
	[string]$Version = "1.0.1.0",
	[ValidateSet("x64", "arm64")]
	[string]$Architecture = "x64"
)

$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $PSScriptRoot
$project = Join-Path $root "ThePen\ThePen.csproj"
$out = Join-Path $PSScriptRoot "out"
$layout = Join-Path $out "layout"
$package = Join-Path $out "ThePen_${Version}_${Architecture}.msix"

if ($Version -notmatch '^\d+\.\d+\.\d+\.0$' -or $Version.StartsWith("0.")) {
	throw "Version must look like 1.2.3.0 (the store keeps the last number, and the first one cannot be 0)."
}

# 1. publish - self contained, the store has no shared .NET desktop runtime
if (Test-Path $layout) { Remove-Item $layout -Recurse -Force }
dotnet publish $project -c Release -r "win-$Architecture" --self-contained true -o $layout -nologo -v q `
	-p:Version=$($Version.Substring(0, $Version.LastIndexOf('.'))) -p:DebugType=none
if ($LASTEXITCODE -ne 0) { throw "publish failed" }

# 2. license files travel with the program
Copy-Item (Join-Path $root "LICENSE.md"), (Join-Path $root "NOTICE"), (Join-Path $root "THIRD-PARTY-NOTICES.md") $layout

# 3. manifest and logos
Copy-Item (Join-Path $PSScriptRoot "Assets") (Join-Path $layout "Assets") -Recurse
$manifest = Get-Content (Join-Path $PSScriptRoot "AppxManifest.template.xml") -Raw -Encoding UTF8
$manifest = $manifest.Replace("{{IdentityName}}", $IdentityName).Replace("{{Publisher}}", $Publisher)
$manifest = $manifest.Replace("{{PublisherDisplayName}}", $PublisherDisplayName)
$manifest = $manifest.Replace("{{Version}}", $Version).Replace("{{Architecture}}", $Architecture)
[System.IO.File]::WriteAllText((Join-Path $layout "AppxManifest.xml"), $manifest, (New-Object System.Text.UTF8Encoding($false)))

# 4. pack with makeappx.exe from the Microsoft.Windows.SDK.BuildTools package
$tools = (dotnet build $project -getProperty:PkgMicrosoft_Windows_SDK_BuildTools).Trim()
$makeappx = Get-ChildItem (Join-Path $tools "bin") -Recurse -Filter makeappx.exe |
	Where-Object { $_.FullName -match '\\x64\\' } | Select-Object -First 1
if (-not $makeappx) { throw "makeappx.exe was not found. Run 'dotnet restore' first." }

# resources.pri lets Windows pick the scale-200 / targetsize logos
$makepri = Join-Path $makeappx.DirectoryName "makepri.exe"
$priConfig = Join-Path $out "priconfig.xml"
& $makepri createconfig /cf $priConfig /dq en-US /o | Out-Null
& $makepri new /pr $layout /cf $priConfig /of (Join-Path $layout "resources.pri") /o | Out-Null
if ($LASTEXITCODE -ne 0) { throw "makepri failed" }

if (Test-Path $package) { Remove-Item $package -Force }
& $makeappx.FullName pack /d $layout /p $package /o | Select-Object -Last 3
if ($LASTEXITCODE -ne 0) { throw "makeappx failed" }

"{0}  ({1:N1} MB)" -f $package, ((Get-Item $package).Length / 1MB)
