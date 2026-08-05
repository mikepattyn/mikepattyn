#!/usr/bin/env pwsh
$ErrorActionPreference = "Stop"

$Root = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$ConfigPath = Join-Path $Root "infra/cdk/Mikepattyn.CDK.Constructs/Constants.Deployment.cs"

if (-not (Test-Path $ConfigPath -PathType Leaf)) {
    Write-Error "Copy Constants.Deployment.cs.example → Constants.Deployment.cs first"
    exit 1
}
