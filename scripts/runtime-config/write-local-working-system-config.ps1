# Platform entry point for Docker-local operator runtime config generation.
& (Join-Path $PSScriptRoot "..\local-working-system\write-operator-runtime-config.ps1") @args
