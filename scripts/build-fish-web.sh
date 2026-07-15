#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
MOBILE="$ROOT/apps/fishi-tracking-app/mobile"
OUTPUT="$MOBILE/build/web"

cd "$MOBILE"
flutter build web --release

echo "Flutter web build output: $OUTPUT"
echo "Sync to S3 using AWS CLI and the bucket from SSM /Fish/{Environment}/Frontend/WebBucket"
