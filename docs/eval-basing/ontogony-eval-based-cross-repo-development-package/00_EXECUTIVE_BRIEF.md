# 00 — Executive Brief

## Diagnosis

The four repos are structurally well-separated:

- **Ontogony.Platform** owns reusable mechanics.
- **Kanon.NET** owns semantic authority and decisions.
- **Conexus.NET** owns model access and route/provider policy.
- **Allagma.NET** owns governed execution.

The missing capability is not “more agents.” The missing capability is **eval-based topology control**.

Today the system can run a governed vertical slice, emit events, perform Kanon-mediated policy checks, call Conexus, pause for human gates, and produce audit evidence. But it does not yet make these choices explicit:

```text
What kind of task is this?
Which execution topology should be used?
Why is this topology safer/better than a simpler baseline?
How was success scored?
Which failure mode occurred?
Did model/provider choice improve or degrade outcome?
```

## Target outcome

After this program, every meaningful Allagma run should be able to answer:

```text
Task classification: sequential / parallelizable / exploratory / tool-heavy / high-risk / human-gated
Selected topology: single_workflow / centralized_orchestrator / parallel_review / hybrid_validation
Kanon authorization: allow / deny / human_gate
Conexus route decision: alias, provider, model, fallback chain, capability profile
Baseline comparison: pass/fail, quality score, cost, latency, replayability
Evidence: trace ids, decision ids, model call ids, route decision ids, eval ids
```

## Strategic position

The system should become an **eval-driven orchestration platform**, not an “agent swarm platform.”

The next major phase should be named:

```text
SYSTEM-EVAL-BASELINE — Eval-based topology and baseline hardening
```

## Recommended sequencing

1. Add neutral evaluation contracts to `ontogony-platform`.
2. Add task classification and topology-selection events to `allagma-dotnet`.
3. Add topology-policy evaluation to `kanon-dotnet`.
4. Add route-decision evidence and capability profiles to `conexus-dotnet`.
5. Add first eval harness and baseline comparison in `allagma-dotnet`.
6. Add cross-repo smoke that proves the whole loop.
