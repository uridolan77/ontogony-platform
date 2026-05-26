#!/usr/bin/env bash
set -euo pipefail

ALLAGMA_BASE_URL="${ALLAGMA_BASE_URL:-http://localhost:5083}"
CONEXUS_BASE_URL="${CONEXUS_BASE_URL:-http://localhost:5082}"
KANON_BASE_URL="${KANON_BASE_URL:-http://localhost:5081}"
OUTPUT_DIR="${OUTPUT_DIR:-artifacts/evidence-spine-real-001}"

mkdir -p "$OUTPUT_DIR"

echo "EVIDENCE-SPINE-REAL-001 validation skeleton"
echo "Allagma: $ALLAGMA_BASE_URL"
echo "Conexus: $CONEXUS_BASE_URL"
echo "Kanon: $KANON_BASE_URL"

echo "1. Check service health/readiness manually or with repo scripts."
echo "2. Start a governed fake-provider run through Allagma."
echo "3. Capture allagmaRunId, planningDecisionId, conexusModelCallId, routeDecisionId."
echo "4. Resolve Evidence Spine in the frontend/test harness."
echo "5. Assert no generic source failures, no duplicate canonical nodes, and route decision structured result."

cat > "$OUTPUT_DIR/validation-skeleton.json" <<JSON
{
  "generatedAt": "$(date -u +%Y-%m-%dT%H:%M:%SZ)",
  "workItem": "EVIDENCE-SPINE-REAL-001",
  "allagmaBaseUrl": "$ALLAGMA_BASE_URL",
  "conexusBaseUrl": "$CONEXUS_BASE_URL",
  "kanonBaseUrl": "$KANON_BASE_URL",
  "status": "skeleton",
  "next": "Adapt to actual local-stack auth and evidence resolver test harness."
}
JSON
