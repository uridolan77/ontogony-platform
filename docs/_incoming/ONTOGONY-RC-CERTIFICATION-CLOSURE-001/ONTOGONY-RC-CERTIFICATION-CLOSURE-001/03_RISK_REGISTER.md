# Risk Register

| Risk | Severity | Owner | Mitigation |
|---|---:|---|---|
| Port lock and docker compose disagree | Critical | Platform / Allagma | Service identity preflight; fail before trigger calls |
| Aisthesis cert points to wrong producer port | Critical | Aisthesis / Platform | Use runtime lock generated env; no hardcoded defaults |
| Metabole 502 hides downstream Kanon/Conexus failure | High | Metabole | Include downstream status and error envelope in trigger response |
| Allagma 500 hides missing service-token/config | High | Allagma | Return normalized orchestration error envelope with trace/correlation |
| ReplayTarget pack fix leaks app types into contracts | High | Kanon | Keep DTOs contract-only; no Infrastructure/Application references |
| Conexus metric test is flaky rather than broken | Medium | Conexus | Add deterministic meter listener flush/wait helper |
| Full rerun is green locally but not in CI | High | Platform | Commit generated artifacts and exact commands; add CI optional nightly |
| Overclaiming RC/prod readiness | Critical | System | Closeout must say RC-candidate only if live PASS; production not claimed |
