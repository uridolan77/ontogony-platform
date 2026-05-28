# Risk register

| Risk | Severity | Mitigation |
|---|---:|---|
| Fixture smoke mistaken for live proof | High | Script statuses must distinguish Fixture/PASS from Live/NOT_RUN/PASS |
| Aisthesis becomes semantic authority | High | Boundary docs and forbidden-dependency tests; no Kanon logic in Aisthesis |
| Edge spoofing makes traces look complete | High | Edge auth hardening and relation ownership |
| Evaluation lost due to fire-and-forget | Medium/High | Durable evaluation jobs or accepted blocker |
| Matrix v2 over-requires optional edges | Medium | Profile-aware requiredWhen/notApplicableReason |
| Producer repos drift from Aisthesis contracts | Medium | Producer emitter contract check script |
| Retention breaks reconstructability | High | Tombstone/redaction semantics and erasure audit evidence |
| OTel leaks payloads/secrets | High | Attribute allowlist; no raw payload export |
| Frontend claims live support without backend validation | Medium | Frontend contract smoke against running API |
| Production-ready claim appears too early | High | Closeout classifications prohibit production claim |
