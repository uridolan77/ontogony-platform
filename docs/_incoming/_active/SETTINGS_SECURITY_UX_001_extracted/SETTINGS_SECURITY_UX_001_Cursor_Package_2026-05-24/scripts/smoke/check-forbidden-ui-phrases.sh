#!/usr/bin/env bash
set -euo pipefail

ROOT="${1:-.}"
phrases=(
  "unknown source"
  "Allagma defaults"
  "Kanon trusts headers"
  "secret-live-key"
)

failed=0
for phrase in "${phrases[@]}"; do
  if grep -RIn --exclude-dir=node_modules --exclude-dir=dist --exclude-dir=build --exclude-dir=coverage \
    --include='*.ts' --include='*.tsx' --include='*.md' --include='*.json' \
    "$phrase" "$ROOT"; then
    echo "Forbidden phrase found: $phrase" >&2
    failed=1
  fi
done

if [[ "$failed" -ne 0 ]]; then
  exit 1
fi

echo "No forbidden UI phrases found."
