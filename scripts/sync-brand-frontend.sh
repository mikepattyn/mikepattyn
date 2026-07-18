#!/usr/bin/env bash
set -euo pipefail

# Usage: sync-brand-frontend.sh <AppName> <AppDir>
# AppName: Mikepattyn | AlienButNice

APP_NAME="${1:?AppName required (Mikepattyn|AlienButNice)}"
APP_DIR="${2:?App directory required}"
ENVIRONMENT="Production"
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
SOURCE="$ROOT/${APP_DIR}"
OUTPUT="$SOURCE/dist"

if [[ ! -d "$SOURCE" ]]; then
  echo "App directory not found: $SOURCE" >&2
  exit 1
fi

cd "$SOURCE"
npm ci
npm run build

"$ROOT/scripts/sync-static-site.sh" "$APP_NAME" "$ENVIRONMENT" "$OUTPUT"
