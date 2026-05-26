# Executive Brief

## Problem

Ontogony already has many ingredients of an evidence-rich agent platform: runs, traces, model calls, decisions, semantic graphs, review items, source bindings, and operator context. The current gap is that these artifacts do not automatically prove that a decision is reconstructable.

A trace says something happened. A reconstructability report says whether a later investigator can answer the governance questions that matter:

```text
What input evidence led to the decision?
Which policy or semantic authority applied?
Who or what acted?
What authorization envelope permitted it?
What safe reasoning/rationale evidence exists?
What action was emitted?
What state changed afterward?
```

## Target capability

Add a cross-service Decision Reconstructability Spine that converts existing evidence fragments into a per-decision report.

The report should be:

- property-level, not just a single score;
- explicit about missing evidence;
- linked to source fragments;
- safe about reasoning evidence;
- testable through golden fixtures;
- visible in the operator console.

## Why now

This package fits naturally after Evidence Spine work. It turns the Evidence Spine from a navigational graph into a governance-grade diagnostic layer.

## Scope

This is a vertical slice, not a complete rewrite of Ontogony telemetry.

In scope:

- contracts;
- classifier;
- diagnostics;
- API/read endpoints;
- representative cross-service fragments;
- console panel;
- golden tests.

Out of scope:

- capturing hidden chain-of-thought;
- replacing existing run/trace/model-call records;
- implementing a full external incident forensics product;
- adding statistical benchmark infrastructure;
- making all legacy actions fully reconstructable in one pass.
