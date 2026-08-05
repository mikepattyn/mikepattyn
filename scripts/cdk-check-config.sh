#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
CONFIG="$ROOT/infra/cdk/Mikepattyn.CDK.Constructs/Constants.Deployment.cs"

if [[ ! -f "$CONFIG" ]]; then
  echo "Copy Constants.Deployment.cs.example → Constants.Deployment.cs first" >&2
  exit 1
fi
