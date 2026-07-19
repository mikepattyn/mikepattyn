#!/usr/bin/env bash
# Smoke test Fish serverless API (requires deployed Dev stack or local env vars).
set -euo pipefail

API_URL="${FISH_API_URL:-}"
TABLE_NAME="${FISH_TABLE_NAME:-Fish-Application-Table-Development}"
TOKEN="${AUTHRESS_DEV_TOKEN:-}"

if [ -z "$API_URL" ]; then
  echo "Set FISH_API_URL (e.g. https://gofish.mikepattyn.nl/api or API Gateway stage URL + /api)"
  exit 1
fi

echo "GET $API_URL/health"
curl -sf "$API_URL/health" | head -c 200
echo

if [ -n "$TOKEN" ]; then
  echo "GET $API_URL/profile (authorized)"
  curl -sf -H "Authorization: Bearer $TOKEN" "$API_URL/profile" | head -c 200
  echo
else
  echo "Skip authorized /profile (set AUTHRESS_DEV_TOKEN to test)"
fi

echo "Seed (optional): dotnet run --project apps/fishi-tracking-app/backend/src/Fish.Seed -- --table-name $TABLE_NAME"
echo "Smoke checks passed."
