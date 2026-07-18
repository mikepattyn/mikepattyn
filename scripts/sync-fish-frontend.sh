#!/usr/bin/env bash
set -euo pipefail

ENVIRONMENT="${1:?Environment required (Development|Staging|Production)}"
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
OUTPUT="$ROOT/apps/fishi-tracking-app/mobile/build/web"

"$ROOT/scripts/build-fish-web.sh"
"$ROOT/scripts/sync-static-site.sh" Fish "$ENVIRONMENT" "$OUTPUT" WebBucket
