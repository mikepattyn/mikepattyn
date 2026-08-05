#!/usr/bin/env pwsh

param(
    [Parameter(Mandatory = $true, Position = 0)][string]$Environment
)

$ErrorActionPreference = "Stop"

$Root = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$Zip = Join-Path $Root "infra/cdk/Mikepattyn.CDK/lambda/kapsalon.zip"

& (Join-Path $PSScriptRoot "build-lambda.ps1")
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$functionSuffixes = @("Authorizer", "Scheduling-Api", "Identity-Api", "Tenant-Api")
foreach ($suffix in $functionSuffixes) {
    $FunctionName = "Kapsalon-$suffix-$Environment"
    Write-Host "Updating $FunctionName..."
    $zipUri = "fileb://$($Zip -replace '\\', '/')"
    aws lambda update-function-code --function-name $FunctionName --zip-file $zipUri
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}

Write-Host "Done: Kapsalon backend $Environment"
