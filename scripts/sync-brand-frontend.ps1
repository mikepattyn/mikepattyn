#!/usr/bin/env pwsh
# Usage: sync-brand-frontend.ps1 <AppName> <AppDir>
# AppName: Mikepattyn | AlienButNice

param(
    [Parameter(Mandatory = $true, Position = 0)][string]$AppName,
    [Parameter(Mandatory = $true, Position = 1)][string]$AppDir
)

$ErrorActionPreference = "Stop"

$Environment = "Production"
$Root = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$Source = Join-Path $Root $AppDir
$Output = Join-Path $Source "dist"

if (-not (Test-Path $Source -PathType Container)) {
    Write-Error "App directory not found: $Source"
    exit 1
}

Push-Location $Source
try {
    npm ci
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    npm run build
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}
finally {
    Pop-Location
}

& (Join-Path $PSScriptRoot "sync-static-site.ps1") $AppName $Environment $Output
