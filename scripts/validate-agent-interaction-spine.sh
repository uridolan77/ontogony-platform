#!/usr/bin/env bash
# PLAT-AGUI-000 — validates agent interaction spine artifacts (delegates to PowerShell).
set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "${SCRIPT_DIR}/.." && pwd)"
if command -v pwsh >/dev/null 2>&1; then
  pwsh -NoProfile -File "${SCRIPT_DIR}/validate-agent-interaction-spine.ps1" -RepoRoot "${REPO_ROOT}" "$@"
elif command -v powershell >/dev/null 2>&1; then
  powershell -NoProfile -File "${SCRIPT_DIR}/validate-agent-interaction-spine.ps1" -RepoRoot "${REPO_ROOT}" "$@"
else
  echo "validate-agent-interaction-spine.sh requires pwsh or powershell." >&2
  exit 1
fi
