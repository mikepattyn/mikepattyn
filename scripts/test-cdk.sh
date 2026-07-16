#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
COVERAGE_DIR="$ROOT/infra/cdk/coverage"
RESULTS_DIR="$COVERAGE_DIR/results"

rm -rf "$RESULTS_DIR"
mkdir -p "$RESULTS_DIR"

LAMBDA_ZIP="$ROOT/infra/cdk/Mikepattyn.CDK/lambda/kapsalon.zip"
FISH_LAMBDA_ZIP="$ROOT/infra/cdk/Mikepattyn.CDK/lambda/fish.zip"
if [ ! -f "$LAMBDA_ZIP" ]; then
  mkdir -p "$(dirname "$LAMBDA_ZIP")"
  echo placeholder > "$(dirname "$LAMBDA_ZIP")/placeholder.txt"
  (cd "$(dirname "$LAMBDA_ZIP")" && zip -q kapsalon.zip placeholder.txt)
fi
if [ ! -f "$FISH_LAMBDA_ZIP" ]; then
  mkdir -p "$(dirname "$FISH_LAMBDA_ZIP")"
  echo placeholder > "$(dirname "$FISH_LAMBDA_ZIP")/placeholder-fish.txt"
  (cd "$(dirname "$FISH_LAMBDA_ZIP")" && zip -q fish.zip placeholder-fish.txt)
fi

dotnet test "$ROOT/infra/cdk/Mikepattyn.CDK.Constructs.Tests/Mikepattyn.CDK.Constructs.Tests.csproj" \
  --configuration Release \
  --collect:"XPlat Code Coverage" \
  --results-directory "$RESULTS_DIR" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura,opencover

if command -v reportgenerator >/dev/null 2>&1; then
  reportgenerator \
    "-reports:$RESULTS_DIR/**/coverage.cobertura.xml" \
    "-targetdir:$COVERAGE_DIR/html" \
    "-reporttypes:Html;TextSummary;Cobertura" \
    "-filefilters:-*Tests*;-*Testing*;-*Program.cs;-*Props.cs;-*Constants*.cs;-*GlobalUsings.cs"
  cat "$COVERAGE_DIR/html/Summary.txt"
else
  echo "Install reportgenerator for per-file coverage tables:"
  echo "  dotnet tool install -g dotnet-reportgenerator-globaltool"
  find "$RESULTS_DIR" -name 'coverage.cobertura.xml' -print
fi
