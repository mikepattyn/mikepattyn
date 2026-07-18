#!/usr/bin/env bash
set -euo pipefail

ENVIRONMENT="${1:?Environment required (Development|Staging|Production)}"
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
ZIP="$ROOT/infra/cdk/Mikepattyn.CDK/lambda/fish.zip"

"$ROOT/scripts/build-fish-lambda.sh"

for function_suffix in Authorizer Spots-Api Catches-Api Profile-Api Community-Api; do
  FUNCTION_NAME="Fish-${function_suffix}-${ENVIRONMENT}"
  echo "Updating ${FUNCTION_NAME}..."
  aws lambda update-function-code \
    --function-name "${FUNCTION_NAME}" \
    --zip-file "fileb://${ZIP}"
done

echo "Done: Fish backend ${ENVIRONMENT}"
