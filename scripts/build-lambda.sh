#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
OUTPUT="$ROOT/infra/cdk/Mikepattyn.CDK/lambda/publish"
ZIP="$ROOT/infra/cdk/Mikepattyn.CDK/lambda/kapsalon.zip"
KAPSALON="$ROOT/apps/kapsalon"

rm -rf "$OUTPUT"
mkdir -p "$OUTPUT" "$(dirname "$ZIP")"

for project in \
  "$KAPSALON/backend/Kapsalon.Shared/Kapsalon.Shared.csproj" \
  "$KAPSALON/backend/Kapsalon.Auth/Kapsalon.Auth.csproj" \
  "$KAPSALON/backend/Kapsalon.Identity.Api/Kapsalon.Identity.Api.csproj" \
  "$KAPSALON/backend/Kapsalon.Scheduling.Api/Kapsalon.Scheduling.Api.csproj" \
  "$KAPSALON/backend/Kapsalon.Tenant.Api/Kapsalon.Tenant.Api.csproj"
do
  dotnet publish "$project" -c Release -o "$OUTPUT" /p:UseAppHost=false /p:GenerateRuntimeConfigurationFiles=true
done

rm -f "$ZIP"
(cd "$OUTPUT" && zip -r "$ZIP" .)

echo "Created $ZIP"
