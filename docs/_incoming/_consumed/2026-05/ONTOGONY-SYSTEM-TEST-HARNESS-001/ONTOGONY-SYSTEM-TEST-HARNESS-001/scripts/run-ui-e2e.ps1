$ErrorActionPreference = "Stop"
Write-Host "Running Ontogony Playwright UI tests..."
Push-Location "$PSScriptRoot/../tests/ui-playwright"
try {
  npm install
  npx playwright install --with-deps
  npm test
} finally {
  Pop-Location
}
