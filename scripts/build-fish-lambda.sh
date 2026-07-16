#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
OUTPUT="$ROOT/infra/cdk/Mikepattyn.CDK/lambda/publish-fish"
ZIP="$ROOT/infra/cdk/Mikepattyn.CDK/lambda/fish.zip"
FISH="$ROOT/apps/fishi-tracking-app"

rm -rf "$OUTPUT"
mkdir -p "$OUTPUT" "$(dirname "$ZIP")"

for project in \
  "$FISH/backend/src/Fish.Shared/Fish.Shared.csproj" \
  "$FISH/backend/src/Fish.Auth/Fish.Auth.csproj" \
  "$FISH/backend/src/Fish.Spots.Api/Fish.Spots.Api.csproj" \
  "$FISH/backend/src/Fish.Catches.Api/Fish.Catches.Api.csproj" \
  "$FISH/backend/src/Fish.Profile.Api/Fish.Profile.Api.csproj" \
  "$FISH/backend/src/Fish.Community.Api/Fish.Community.Api.csproj"
do
  dotnet publish "$project" -c Release -o "$OUTPUT" /p:UseAppHost=false /p:GenerateRuntimeConfigurationFiles=true
done

rm -f "$ZIP"
(cd "$OUTPUT" && zip -r "$ZIP" .)

echo "Created $ZIP"
