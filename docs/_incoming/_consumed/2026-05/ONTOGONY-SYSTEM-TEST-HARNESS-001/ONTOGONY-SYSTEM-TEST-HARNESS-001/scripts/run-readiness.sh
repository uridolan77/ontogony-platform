#!/usr/bin/env bash
set -euo pipefail

echo "Running Ontogony readiness checks..."
declare -A services=(
  [Kanon]="${KANON_BASE_URL:-http://localhost:5081}"
  [Conexus]="${CONEXUS_BASE_URL:-http://localhost:5082}"
  [Allagma]="${ALLAGMA_BASE_URL:-http://localhost:5083}"
  [Metabole]="${METABOLE_BASE_URL:-http://localhost:5084}"
  [Aisthesis]="${AISTHESIS_BASE_URL:-http://localhost:5085}"
)

for name in "${!services[@]}"; do
  url="${services[$name]%/}/health"
  echo "GET $url"
  curl -fsS "$url" >/dev/null
  echo "  $name ok"
done
