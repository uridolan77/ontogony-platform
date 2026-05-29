#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PLATFORM_ROOT="$(cd "$SCRIPT_DIR/../.." && pwd)"
COMPOSE_ROOT="$PLATFORM_ROOT/docker/local-working-system"
FRONTEND_ROOT="$(cd "$PLATFORM_ROOT/../ontogony-frontend" && pwd)"
GENERATOR="$FRONTEND_ROOT/scripts/runtime-config/generate-operator-runtime-config.mjs"

ENV_FILE="${ENV_FILE:-}"
if [[ -z "$ENV_FILE" ]]; then
  if [[ -f "$COMPOSE_ROOT/.env" ]]; then
    ENV_FILE="$COMPOSE_ROOT/.env"
  else
    ENV_FILE="$COMPOSE_ROOT/.env.example"
  fi
fi

read_env() {
  local key="$1"
  local default="$2"
  if [[ -n "${!key:-}" ]]; then
    printf '%s' "${!key}"
    return
  fi
  if [[ -f "$ENV_FILE" ]]; then
    local line
    line="$(grep -E "^${key}=" "$ENV_FILE" | tail -n 1 || true)"
    if [[ -n "$line" ]]; then
      printf '%s' "${line#*=}"
      return
    fi
  fi
  printf '%s' "$default"
}

KANON_PORT="$(read_env KANON_HOST_PORT 5081)"
CONEXUS_PORT="$(read_env CONEXUS_HOST_PORT 5082)"
ALLAGMA_PORT="$(read_env ALLAGMA_HOST_PORT 5083)"
AISTHESIS_PORT="$(read_env AISTHESIS_HOST_PORT 5084)"
METABOLE_PORT="$(read_env METABOLE_HOST_PORT 5085)"

OUTPUT_PATH="${OUTPUT_PATH:-$COMPOSE_ROOT/generated/operator-runtime-config.json}"
PROFILE_ID="${FRONTEND_RUNTIME_PROFILE_ID:-docker-local-nginx}"
ENVIRONMENT_NAME="${FRONTEND_RUNTIME_ENVIRONMENT_NAME:-Local Docker}"
CONEXUS_URL="${FRONTEND_RUNTIME_CONEXUS_BASE_URL:-http://localhost:${CONEXUS_PORT}}"
KANON_URL="${FRONTEND_RUNTIME_KANON_BASE_URL:-http://localhost:${KANON_PORT}}"
ALLAGMA_URL="${FRONTEND_RUNTIME_ALLAGMA_BASE_URL:-http://localhost:${ALLAGMA_PORT}}"
AISTHESIS_URL="${FRONTEND_RUNTIME_AISTHESIS_BASE_URL:-http://localhost:${AISTHESIS_PORT}}"
METABOLE_URL="${FRONTEND_RUNTIME_METABOLE_BASE_URL:-http://localhost:${METABOLE_PORT}}"
ONTOLOGY="$(read_env FRONTEND_RUNTIME_KANON_ONTOLOGY_VERSION_ID "$(read_env FRONTEND_VITE_KANON_ONTOLOGY_VERSION_ID gaming-core@0.1.0)")"
PROJECT_ID="$(read_env FRONTEND_RUNTIME_CONEXUS_PROJECT_ID dev-project)"
MODEL_ALIAS="$(read_env FRONTEND_RUNTIME_CONEXUS_MODEL_ALIAS risk-summary-v0)"
GIT_SHA="$(read_env FRONTEND_VITE_GIT_SHA local)"

OVERRIDE="$COMPOSE_ROOT/config/operator-runtime-config.local.override.json"
if [[ -f "$OVERRIDE" ]]; then
  echo "Applying override: $OVERRIDE (manual merge: set FRONTEND_RUNTIME_* env vars or edit override before run)"
fi

mkdir -p "$(dirname "$OUTPUT_PATH")"
node "$GENERATOR" \
  --profile "$PROFILE_ID" \
  --out "$OUTPUT_PATH" \
  --environment-name "$ENVIRONMENT_NAME" \
  --conexus-url "$CONEXUS_URL" \
  --kanon-url "$KANON_URL" \
  --allagma-url "$ALLAGMA_URL" \
  --aisthesis-url "$AISTHESIS_URL" \
  --metabole-url "$METABOLE_URL" \
  --ontology "$ONTOLOGY" \
  --conexus-project-id "$PROJECT_ID" \
  --model-alias "$MODEL_ALIAS" \
  --build-git-sha "$GIT_SHA" \
  --build-source "ontogony-platform/docker/local-working-system" \
  --build-description "Docker local working system runtime defaults"

echo "Wrote operator runtime config to $OUTPUT_PATH (profile=$PROFILE_ID)"
