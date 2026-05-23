#!/usr/bin/env bash
set -euo pipefail

CONEXUS_URL="${CONEXUS_URL:-http://localhost:5082}"
KANON_URL="${KANON_URL:-http://localhost:5081}"
ALLAGMA_URL="${ALLAGMA_URL:-http://localhost:5083}"

probe() {
  local service="$1"
  local base="$2"

  local health ready
  health="$(curl -fsS --max-time 5 "$base/health" || true)"
  ready="$(curl -fsS --max-time 5 "$base/ready" || true)"

  local connectivity="offline"
  local health_schema="missing"
  local ready_schema="missing"
  local health_status="unknown"
  local ready_status="unknown"
  local version=""

  if [[ -n "$health" ]]; then
    connectivity="live"
    health_schema="$(echo "$health" | python -c 'import sys,json; print(json.load(sys.stdin).get("schemaVersion","missing"))' 2>/dev/null || echo invalid)"
    health_status="$(echo "$health" | python -c 'import sys,json; print(json.load(sys.stdin).get("status","unknown"))' 2>/dev/null || echo unknown)"
    version="$(echo "$health" | python -c 'import sys,json; print(json.load(sys.stdin).get("version",""))' 2>/dev/null || echo '')"
  fi

  if [[ -n "$ready" ]]; then
    ready_schema="$(echo "$ready" | python -c 'import sys,json; print(json.load(sys.stdin).get("schemaVersion","missing"))' 2>/dev/null || echo invalid)"
    ready_status="$(echo "$ready" | python -c 'import sys,json; print(json.load(sys.stdin).get("status","unknown"))' 2>/dev/null || echo unknown)"
  fi

  printf "%-8s %-8s health=%-9s healthSchema=%-10s ready=%-10s readySchema=%-10s version=%s\n" \
    "$service" "$connectivity" "$health_status" "$health_schema" "$ready_status" "$ready_schema" "$version"
}

probe "conexus" "$CONEXUS_URL"
probe "kanon" "$KANON_URL"
probe "allagma" "$ALLAGMA_URL"
