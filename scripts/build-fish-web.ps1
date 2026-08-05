#!/usr/bin/env pwsh
$ErrorActionPreference = "Stop"

$Root = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$Mobile = Join-Path $Root "apps/fishi-tracking-app/mobile"
$Output = Join-Path $Mobile "build/web"

Push-Location $Mobile
try {
    flutter build web --release
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}
finally {
    Pop-Location
}

Write-Host "Flutter web build output: $Output"
Write-Host "Sync to S3 using AWS CLI and the bucket from SSM /Fish/{Environment}/Frontend/WebBucket"
