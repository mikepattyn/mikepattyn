#!/usr/bin/env pwsh
$ErrorActionPreference = "Stop"

$Root = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$CoverageDir = Join-Path $Root "infra/cdk/coverage"
$ResultsDir = Join-Path $CoverageDir "results"

if (Test-Path $ResultsDir) {
    Remove-Item -Recurse -Force $ResultsDir
}
New-Item -ItemType Directory -Force -Path $ResultsDir | Out-Null

$LambdaDir = Join-Path $Root "infra/cdk/Mikepattyn.CDK/lambda"
$KapsalonZip = Join-Path $LambdaDir "kapsalon.zip"
$FishZip = Join-Path $LambdaDir "fish.zip"

function New-PlaceholderZip {
    param(
        [Parameter(Mandatory = $true)][string]$ZipPath,
        [Parameter(Mandatory = $true)][string]$PlaceholderName
    )

    if (Test-Path $ZipPath) {
        return
    }

    New-Item -ItemType Directory -Force -Path (Split-Path $ZipPath -Parent) | Out-Null
    $placeholderPath = Join-Path (Split-Path $ZipPath -Parent) $PlaceholderName
    Set-Content -Path $placeholderPath -Value "placeholder"
    if (Test-Path $ZipPath) {
        Remove-Item -Force $ZipPath
    }
    Compress-Archive -Path $placeholderPath -DestinationPath $ZipPath -Force
}

New-PlaceholderZip -ZipPath $KapsalonZip -PlaceholderName "placeholder.txt"
New-PlaceholderZip -ZipPath $FishZip -PlaceholderName "placeholder-fish.txt"

$constructsTests = Join-Path $Root "infra/cdk/Mikepattyn.CDK.Constructs.Tests/Mikepattyn.CDK.Constructs.Tests.csproj"
$e2eTests = Join-Path $Root "infra/cdk/Mikepattyn.CDK.E2E.Tests/Mikepattyn.CDK.E2E.Tests.csproj"

dotnet test $constructsTests `
    --configuration Release `
    --collect:"XPlat Code Coverage" `
    --results-directory $ResultsDir `
    -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura,opencover
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

dotnet test $e2eTests --configuration Release
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

$reportgenerator = Get-Command reportgenerator -ErrorAction SilentlyContinue
if ($reportgenerator) {
    $reports = Join-Path $ResultsDir "**/coverage.cobertura.xml"
    $htmlDir = Join-Path $CoverageDir "html"
    & reportgenerator `
        "-reports:$reports" `
        "-targetdir:$htmlDir" `
        "-reporttypes:Html;TextSummary;Cobertura" `
        "-filefilters:-*Tests*;-*Testing*;-*Program.cs;-*Props.cs;-*Constants*.cs;-*GlobalUsings.cs"
    if ($LASTEXITCODE -ne 0) {
        exit $LASTEXITCODE
    }

    $summary = Join-Path $htmlDir "Summary.txt"
    if (Test-Path $summary) {
        Get-Content $summary
    }
}
else {
    Write-Host "Install reportgenerator for per-file coverage tables:"
    Write-Host "  dotnet tool install -g dotnet-reportgenerator-globaltool"
    Get-ChildItem -Path $ResultsDir -Recurse -Filter "coverage.cobertura.xml" |
        ForEach-Object { $_.FullName }
}
