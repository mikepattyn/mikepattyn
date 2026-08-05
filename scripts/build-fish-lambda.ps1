#!/usr/bin/env pwsh
$ErrorActionPreference = "Stop"

$Root = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$Output = Join-Path $Root "infra/cdk/Mikepattyn.CDK/lambda/publish-fish"
$Zip = Join-Path $Root "infra/cdk/Mikepattyn.CDK/lambda/fish.zip"
$Fish = Join-Path $Root "apps/fishi-tracking-app"

if (Test-Path $Output) {
    Remove-Item -Recurse -Force $Output
}
New-Item -ItemType Directory -Force -Path $Output, (Split-Path $Zip -Parent) | Out-Null

$projects = @(
    "$Fish/backend/src/Fish.Shared/Fish.Shared.csproj",
    "$Fish/backend/src/Fish.Auth/Fish.Auth.csproj",
    "$Fish/backend/src/Fish.Spots.Api/Fish.Spots.Api.csproj",
    "$Fish/backend/src/Fish.Catches.Api/Fish.Catches.Api.csproj",
    "$Fish/backend/src/Fish.Profile.Api/Fish.Profile.Api.csproj",
    "$Fish/backend/src/Fish.Community.Api/Fish.Community.Api.csproj"
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
