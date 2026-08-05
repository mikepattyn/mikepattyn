#!/usr/bin/env pwsh

param(
    [Parameter(Mandatory = $true, Position = 0)][string]$Environment
)

$ErrorActionPreference = "Stop"

$Root = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$Output = Join-Path $Root "apps/fishi-tracking-app/mobile/build/web"

& (Join-Path $PSScriptRoot "build-fish-web.ps1")
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

& (Join-Path $PSScriptRoot "sync-static-site.ps1") "Fish" $Environment $Output "WebBucket"
