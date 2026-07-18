#!/usr/bin/env bash
set -euo pipefail

ENVIRONMENT="${1:?Environment required (Development|Staging|Production)}"
ROOT="$(cd "$(dirname "$0")/.." && pwd)"
ZIP="$ROOT/infra/cdk/Mikepattyn.CDK/lambda/kapsalon.zip"

"$ROOT/scripts/build-lambda.sh"

for function_suffix in Authorizer Scheduling-Api Identity-Api Tenant-Api; do
  FUNCTION_NAME="Kapsalon-${function_suffix}-${ENVIRONMENT}"
  echo "Updating ${FUNCTION_NAME}..."
  aws lambda update-function-code \
    --function-name "${FUNCTION_NAME}" \
    --zip-file "fileb://${ZIP}"
done

echo "Done: Kapsalon backend ${ENVIRONMENT}"
