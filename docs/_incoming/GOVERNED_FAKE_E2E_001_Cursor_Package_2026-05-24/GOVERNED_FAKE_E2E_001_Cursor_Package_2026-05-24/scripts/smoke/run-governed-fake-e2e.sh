#!/usr/bin/env bash
set -euo pipefail

ALLAGMA_BASE_URL="${ALLAGMA_BASE_URL:-http://localhost:5083}"

payload='{
  "ontologyVersionId": "gaming-core@0.1.0",
  "actorId": "local-operator",
  "actorType": "human",
  "actorRoles": ["Admin"],
  "objective": "GOVERNED-FAKE-E2E-001: summarize player risk using local fake provider.",
  "context": { "playerId": "123" },
  "modelPurpose": "summarize-player-risk"
}'

echo "Starting governed fake run..."
curl -sS \
  -X POST \
  "$ALLAGMA_BASE_URL/allagma/v0/runs" \
  -H "Content-Type: application/json" \
  -d "$payload" | tee /tmp/gov_fake_run.json

echo
echo "Inspect /tmp/gov_fake_run.json and paste runId into Evidence Spine."
