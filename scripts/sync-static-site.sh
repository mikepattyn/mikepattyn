#!/usr/bin/env bash
set -euo pipefail

# Sync a built static site to S3 and invalidate CloudFront.
# Usage: sync-static-site.sh <AppName> <Environment> <SourceDir> [BucketParamName]
#
# BucketParamName defaults to BucketName (Fish edge uses WebBucket).

APP_NAME="${1:?AppName required}"
ENVIRONMENT="${2:?Environment required}"
SOURCE_DIR="${3:?SourceDir required}"
BUCKET_PARAM="${4:-BucketName}"

if [[ ! -d "$SOURCE_DIR" ]]; then
  echo "Source directory not found: $SOURCE_DIR" >&2
  exit 1
fi

PREFIX="/${APP_NAME}/${ENVIRONMENT}/Frontend"
BUCKET_NAME=$(aws ssm get-parameter --name "${PREFIX}/${BUCKET_PARAM}" --query 'Parameter.Value' --output text)
DISTRIBUTION_ID=$(aws ssm get-parameter --name "${PREFIX}/DistributionId" --query 'Parameter.Value' --output text)

echo "Syncing ${SOURCE_DIR} → s3://${BUCKET_NAME}/"
aws s3 sync "$SOURCE_DIR" "s3://${BUCKET_NAME}/" --delete

echo "Invalidating CloudFront distribution ${DISTRIBUTION_ID}"
aws cloudfront create-invalidation \
  --distribution-id "$DISTRIBUTION_ID" \
  --paths "/*"

echo "Done: ${APP_NAME} ${ENVIRONMENT}"
