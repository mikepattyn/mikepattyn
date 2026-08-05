#!/usr/bin/env pwsh
# Sync a built static site to S3 and invalidate CloudFront.
# Usage: sync-static-site.ps1 <AppName> <Environment> <SourceDir> [BucketParamName]
#
# BucketParamName defaults to BucketName (Fish edge uses WebBucket).

param(
    [Parameter(Mandatory = $true, Position = 0)][string]$AppName,
    [Parameter(Mandatory = $true, Position = 1)][string]$Environment,
    [Parameter(Mandatory = $true, Position = 2)][string]$SourceDir,
    [Parameter(Position = 3)][string]$BucketParam = "BucketName"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $SourceDir -PathType Container)) {
    Write-Error "Source directory not found: $SourceDir"
    exit 1
}

$Prefix = "/$AppName/$Environment/Frontend"
$BucketName = aws ssm get-parameter --name "$Prefix/$BucketParam" --query "Parameter.Value" --output text
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$DistributionId = aws ssm get-parameter --name "$Prefix/DistributionId" --query "Parameter.Value" --output text
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "Syncing $SourceDir → s3://$BucketName/"
aws s3 sync $SourceDir "s3://$BucketName/" --delete
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "Invalidating CloudFront distribution $DistributionId"
aws cloudfront create-invalidation --distribution-id $DistributionId --paths "/*"
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "Done: $AppName $Environment"
