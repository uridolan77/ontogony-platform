$ErrorActionPreference = "Stop"
if ($env:RUN_LOAD_TESTS -ne "true") {
  Write-Host "RUN_LOAD_TESTS is not true. Skipping load smoke."
  exit 0
}
k6 run "$PSScriptRoot/../load/k6/system-smoke.js"
