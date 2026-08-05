#!/usr/bin/env pwsh

param(
    [Parameter(Mandatory = $true, Position = 0)][string]$Environment
)

$ErrorActionPreference = "Stop"

$Root = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$Web = Join-Path $Root "apps/kapsalon/apps/web"
$BuildConfiguration = $Environment.ToLowerInvariant()
$Output = Join-Path $Web "dist/web/browser"

Push-Location $Web
try {
    $Template = Get-Content "src/environments/environment.template.ts" -Raw
    $Template = $Template -replace '__TENANT_ID__', 'sabunandsteel'
    $Template = $Template -replace '__ENABLE_DEMO_SHORTCUTS__', 'false'
    Set-Content -Path "src/environments/environment.$BuildConfiguration.ts" -Value $Template -NoNewline

    npm ci
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    npm run test:ci
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    npm run build -- --configuration $BuildConfiguration
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}
finally {
    Pop-Location
}

& (Join-Path $PSScriptRoot "sync-static-site.ps1") "Kapsalon" $Environment $Output
