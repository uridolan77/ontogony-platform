# No Fake Functionality Checklist

- [ ] Every visible action maps to a real backend route or is disabled.
- [ ] Every limitation banner maps to backend metadata or explicit missing route.
- [ ] Demo/fixture data is labeled as demo/fallback where used.
- [ ] No retry/cancel/replay-trigger button appears enabled unless backend supports it.
- [ ] Sandbox execute UI does not imply real external execution.
- [ ] Replay-safe skip is not shown as a new execution.
- [ ] Cross-service links gracefully degrade when target backend route is unavailable.
