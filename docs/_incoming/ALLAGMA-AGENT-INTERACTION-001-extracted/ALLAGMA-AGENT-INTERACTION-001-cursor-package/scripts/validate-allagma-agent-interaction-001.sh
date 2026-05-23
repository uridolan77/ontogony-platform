#!/usr/bin/env bash
set -euo pipefail

FRONTEND_ROOT="${FRONTEND_ROOT:-/mnt/c/dev/ontogony-frontend}"
ALLAGMA_BASE_URL="${ALLAGMA_BASE_URL:-http://localhost:5083}"
RUN_ID="${RUN_ID:-}"

echo "ALLAGMA-AGENT-INTERACTION-001 validation"
echo "Frontend root: $FRONTEND_ROOT"
echo "Allagma base URL: $ALLAGMA_BASE_URL"

if [ ! -d "$FRONTEND_ROOT" ]; then
  echo "Frontend root not found: $FRONTEND_ROOT" >&2
  exit 1
fi

pushd "$FRONTEND_ROOT" >/dev/null
if [ -f package.json ]; then
  echo "Running frontend tests..."
  npm test -- --runInBand || npm test
else
  echo "No package.json found in $FRONTEND_ROOT"
fi
popd >/dev/null

echo "Checking Allagma health..."
if command -v curl >/dev/null 2>&1; then
  curl -fsS "$ALLAGMA_BASE_URL/health" || true
  echo
fi

if [ -n "$RUN_ID" ] && command -v curl >/dev/null 2>&1; then
  echo "Checking live run events for $RUN_ID..."
  curl -fsS "$ALLAGMA_BASE_URL/allagma/v0/runs/$RUN_ID/events" \
    -o "allagma-agent-interaction-events-$RUN_ID.json" || true
  echo "Saved allagma-agent-interaction-events-$RUN_ID.json if lookup succeeded."
fi

cat <<'CHECKS'
Manual checks still required:
- Open Agent Interaction and confirm live_lookup mode for a real run.
- Confirm fixture mode shows: Demo fixture — not live evidence.
- Confirm run list has no raw fake provider response summary or unlabeled unknown.
- Confirm exported bundle has mode/dataSource/sourceAttempts/missing and no duplicate sections.
CHECKS
