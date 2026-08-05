#!/usr/bin/env pwsh
$ErrorActionPreference = "Stop"

$Root = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$Output = Join-Path $Root "infra/cdk/Mikepattyn.CDK/lambda/publish"
$Zip = Join-Path $Root "infra/cdk/Mikepattyn.CDK/lambda/kapsalon.zip"
$Kapsalon = Join-Path $Root "apps/kapsalon"

if (Test-Path $Output) {
    Remove-Item -Recurse -Force $Output
}
New-Item -ItemType Directory -Force -Path $Output, (Split-Path $Zip -Parent) | Out-Null

$projects = @(
    "$Kapsalon/backend/Kapsalon.Shared/Kapsalon.Shared.csproj",
    "$Kapsalon/backend/Kapsalon.Auth/Kapsalon.Auth.csproj",
    "$Kapsalon/backend/Kapsalon.Identity.Api/Kapsalon.Identity.Api.csproj",
    "$Kapsalon/backend/Kapsalon.Scheduling.Api/Kapsalon.Scheduling.Api.csproj",
    "$Kapsalon/backend/Kapsalon.Tenant.Api/Kapsalon.Tenant.Api.csproj"
)

foreach ($project in $projects) {
    dotnet publish $project -c Release -o $Output /p:UseAppHost=false /p:GenerateRuntimeConfigurationFiles=true
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}

if (Test-Path $Zip) {
    Remove-Item -Force $Zip
}
Compress-Archive -Path (Join-Path $Output "*") -DestinationPath $Zip -Force

Write-Host "Created $Zip"
