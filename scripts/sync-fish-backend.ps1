#!/usr/bin/env pwsh

param(
    [Parameter(Mandatory = $true, Position = 0)][string]$Environment
)

$ErrorActionPreference = "Stop"

$Root = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path
$Zip = Join-Path $Root "infra/cdk/Mikepattyn.CDK/lambda/fish.zip"

& (Join-Path $PSScriptRoot "build-fish-lambda.ps1")
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$functionSuffixes = @("Authorizer", "Spots-Api", "Catches-Api", "Profile-Api", "Community-Api")
foreach ($suffix in $functionSuffixes) {
    $FunctionName = "Fish-$suffix-$Environment"
    Write-Host "Updating $FunctionName..."
    $zipUri = "fileb://$($Zip -replace '\\', '/')"
    aws lambda update-function-code --function-name $FunctionName --zip-file $zipUri
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}

Write-Host "Done: Fish backend $Environment"
