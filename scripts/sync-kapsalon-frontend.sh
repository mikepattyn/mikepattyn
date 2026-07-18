#!/usr/bin/env bash
set -euo pipefail

ENVIRONMENT="${1:?Environment required (Development|Staging|Production)}"
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
WEB="$ROOT/apps/kapsalon/apps/web"
BUILD_CONFIGURATION="$(echo "$ENVIRONMENT" | tr '[:upper:]' '[:lower:]')"
OUTPUT="$WEB/dist/web/browser"

cd "$WEB"

API_URL=$(aws ssm get-parameter --name "/Kapsalon/${ENVIRONMENT}/Backend/ApiUrl" --query 'Parameter.Value' --output text)
API_URL="${API_URL%/}"

sed -e "s|__API_BASE_URL__|${API_URL}|g" \
    -e "s|__TENANT_ID__|sabunandsteel|g" \
    -e "s|__ENABLE_DEMO_SHORTCUTS__|false|g" \
  src/environments/environment.template.ts \
  > "src/environments/environment.${BUILD_CONFIGURATION}.ts"

npm ci
npm run test:ci
npm run build -- --configuration "$BUILD_CONFIGURATION"

"$ROOT/scripts/sync-static-site.sh" Kapsalon "$ENVIRONMENT" "$OUTPUT"
