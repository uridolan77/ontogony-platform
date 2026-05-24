Yes. The frontend has become **functionally powerful but cognitively overloaded**. That is normal after all the truth/evidence/contract work: we added the right information, but now the console needs a **UX consolidation pass**.

The goal should not be to remove capabilities. The goal is to **change the default presentation**:

```text
From: everything visible, all the time
To: primary operator path first, evidence/details/debug on demand
```

## Diagnosis

The console now exposes many layers at once:

```text
service health
readiness truth
contract truth
runtime posture
domain context
provider posture
evidence links
cross-service links
fixtures/live/generated/imported labels
audit exports
diagnostics exports
route/API parity language
manual DTO/contract language
```

That is too much for normal operation.

The route catalog itself shows why. For example, the Allagma audit page pulls many hooks and clients across Allagma, Conexus, Kanon, trace correlation, Evidence Spine, quota, route preview, model calls, route decisions, and semantic graph; it also lists many evidence components and cross-service links.  The system pages do the same: Home, Topology, Evidence Spine, Agent Interaction, and Settings all carry live/fallback/generated truth, evidence builders, redaction notes, service tokens, and operator next actions. 

That richness is valuable. But it should not all be first-level UI.

# UX direction

## 1. Introduce three operator modes

Add a global console mode selector:

```text
Simple
Operator
Expert
```

### Simple

For day-to-day use.

Show:

```text
status
primary action
main table/result
critical warnings only
```

Hide:

```text
contract badges
raw identifiers
API route names
debug artifacts
most evidence/export controls
```

### Operator

Default for you and local-alpha operators.

Show:

```text
truth badges
domain context
evidence links
service-specific controls
important warnings
```

Keep advanced sections collapsed.

### Expert

For debugging and repo validation.

Show:

```text
all identifiers
source attempts
contract status
raw bundle preview
API route names
generated artifact status
diagnostic/export metadata
```

This preserves all the current work without forcing it into every user’s face.

---

## 2. Standardize every page into the same visual hierarchy

Every page should follow this layout:

```text
Page header
  Title
  One-line purpose
  Current context chips
  Primary action

Status strip
  3–5 high-priority signals only

Main workspace
  The thing the operator came to do

Evidence / details drawer
  Collapsed by default

Advanced / diagnostics drawer
  Collapsed by default
```

The page header should answer:

```text
Where am I?
What is this page for?
What is the current domain/run/provider/evaluation?
What should I do next?
```

Not:

```text
Here are 19 system facts and all possible warning states.
```

---

## 3. Replace badge sprawl with compact “signal groups”

Right now many pages have too many chips. Use grouped signal strips.

### Example

Instead of:

```text
Connectivity live
Readiness ready
Contract valid
Data source live
Runtime posture blocked
Sandbox disabled
Generated artifact fresh
Fixture disabled
Domain gaming-core@0.1.0
Conexus alias summarize-player-risk
```

Show:

```text
System: Live · Ready · Contract valid
Context: gaming-core@0.1.0 · summarize-player-risk
Safety: external calls blocked
```

Then add:

```text
View details
```

which opens a small drawer/table with all individual signals.

Rule:

```text
No card should show more than 3 visible badges by default.
```

Everything else goes behind “More signals.”

---

## 4. Separate “operator truth” from “developer truth”

This is the biggest cleanup principle.

### Operator truth

Useful while using the console:

```text
live / partial / unavailable
ready / not ready
domain selected
provider selected
run completed / waiting / failed
evidence available / missing
```

### Developer truth

Useful while validating the system:

```text
OpenAPI snapshot
route inventory
manual DTO shim
generated artifact
contract parity
runtime lock
fixture fallback source
```

Developer truth should be visible in Expert mode, contract pages, diagnostics, and release-readiness pages — not on every normal workflow page.

---

# Page-by-page cleanup plan

## Home

Current role: cockpit.

Recommended default:

```text
1. System status summary
2. Current domain + current model alias
3. Primary actions:
   - Start governed run
   - Open Evidence Spine
   - Open Agent Interaction
   - Open Settings
4. Active issues list
5. Recent evidence / last governed proof
```

Hide by default:

```text
protocol truth cockpit
full runtime posture
diagnostic export details
compatibility artifact internals
all non-critical badges
```

Home should become the “what should I do now?” page, not the “everything the system knows” page.

---

## Settings

Current risk: it can become a security/debug wall.

Recommended structure:

```text
Operator identity
Service connections
Credential storage
Execution posture
Model naming glossary
Diagnostics export
Advanced
```

Default view should show only:

```text
configured / not configured
session / local browser
local-alpha risk
test connection result
```

Move these into collapsible help:

```text
source taxonomy
raw credential-source explanation
diagnostic privacy fields
model glossary details
actor capability estimates
```

Settings should feel like a control panel, not a legal disclaimer.

---

## Topology

Current role: system map.

Recommended default:

```text
Graph/map first
Node health second
Selected node details in side panel
```

Do not show all service truth columns up front. Instead:

```text
Kanon     Ready
Conexus   Not ready · 1 issue
Allagma   Ready
```

Click a node to show:

```text
connectivity
readiness
contract
failures
base URL
last checked
```

---

## Release Readiness

This page is now truthful, but it is still conceptually heavy.

Recommended default:

```text
RC posture: Not assessed
Why: live validation not attached
Artifact status: generated scorecard only
Blocking gaps: N
Next action: run runtime-lock governed fake E2E
```

Then below:

```text
Route readiness table
Known caveats
Evidence links
Artifact metadata
```

Keep “artifact vs RC certification” visible, but avoid repeating it in every card.

---

## Evidence Spine

Current role: powerful resolver.

Recommended layout:

```text
Input / lookup bar
Resolution summary
Graph visualization / node list
Missing links
Source attempts
Export
```

Default should show:

```text
root
verdict
node count
missing count
best next action
graph
```

Collapse:

```text
all source attempts
raw source details
copy IDs
export preview
identifier taxonomy explanations
```

For normal use, “source attempts” should become:

```text
5 lookups · 4 succeeded · 1 failed
View lookup log
```

not a full list always visible.

---

## Agent Interaction

This page is one of the most important and also one of the easiest to overload.

Recommended default:

```text
Run summary
Timeline groups
Selected event details
Message stream
Provider summary
Evidence links
```

Collapse:

```text
raw payload disclosure
export bundle
all IDs
missing-data reason table
debug classification
```

Use tabs or segmented controls:

```text
Timeline
Messages
Provider
Evidence
Export
```

The operator should not see every panel at once.

---

## Allagma Start Run

This should be simple.

Default:

```text
Scenario / purpose
Domain
Model purpose
Input
Start
```

Move to Advanced:

```text
actor headers
correlation/trace override
manual ontology override
raw request preview
preset internals
```

The domain switcher already helps; do not make the form explain the whole system.

---

## Allagma Run Detail / Audit Journey

The audit page is inherently complex. Treat it like a case file.

Use sections:

```text
Run summary
Timeline
Decisions
Model calls
Human gates
Evidence
Export
```

The route catalog shows this page currently aggregates many cross-service clients and evidence panels.  That means the UI should not render all of them as equal cards.

Recommended:

```text
Top: status, purpose, domain, model alias, trace/correlation
Middle: timeline
Right drawer: selected event evidence
Bottom/Tab: advanced audit export
```

---

## Evaluations

Good direction after EVAL-EVIDENCE-QUALITY, but still reduce dashboard density.

Default:

```text
What can this page prove?
Current page only / fixture / live
Coverage summary
Evaluation matrix
Rows
```

Use one compact honesty banner:

```text
This dashboard summarizes the current page only. Metadata coverage is partial.
```

Do not repeat “current page only” on every card if the banner is already visible; card labels can stay concise.

---

## Kanon pages

Kanon now has strong parity/contract docs, but operator pages should not read like contract docs.

### Domain Packs

Default:

```text
Active packs
Available packs
Lifecycle actions
```

Move to details:

```text
inventory counts
disk/persisted/active explanation
generated-looking pack warning
route/API coverage
```

### Source Bindings

Default:

```text
Filter by ontology
Quality summary
Review queue
Bindings table
Create/test binding
```

Move to drawers:

```text
coverage gaps
contradictions
approval history
test warning metadata
raw source/target schemas
```

### Assistance

Make the boundary clear once:

```text
Draft-only assistance. Human review required.
```

Then stop repeating it in every result card.

---

## Conexus pages

Conexus should be organized around jobs:

```text
Chat test
Routing
Provider posture
Observability
Usage/quota
Projects/keys
```

Avoid mixing:

```text
provider config
model alias
route decision
model call
quota
cost
diagnostics
```

all on the same card.

For route preview:

```text
Input: alias + request profile
Output: selected provider/model + reason
Advanced: all warnings, raw route decision, provider inventory references
```

---

# Navigation cleanup

The console should not feel like “six repos turned into pages.”

Reorganize top-level navigation around operator jobs:

```text
Operate
  Home
  Start Run
  Runs
  Agent Interaction

Evidence
  Evidence Spine
  Replay Evidence
  Audit Journey

Semantic Authority
  Kanon Overview
  Domain Packs
  Source Bindings
  Decisions
  Policies
  Facts & Plans

Model Gateway
  Conexus Chat
  Routing
  Observability
  Providers
  Usage / Quota

Evaluation
  Evaluations
  Datasets
  Baseline Comparisons

System
  Topology
  Release Readiness
  Settings
  Contracts / Diagnostics
```

This is much friendlier than exposing every service as a separate mental universe.

---

# Copywriting rules

The console needs a copy diet.

## Rule 1: one purpose sentence per page

Bad:

```text
This page resolves cross-service evidence graphs from live Allagma, Kanon, and Conexus APIs...
```

Good:

```text
Resolve a run, trace, decision, or model call into one evidence graph.
```

Put the long version in “How this works.”

## Rule 2: warnings must be actionable

Bad:

```text
Contract partial, generated artifact stale, live fallback active.
```

Good:

```text
This page is using generated data. Run `npm run readiness:sync` to refresh it.
```

## Rule 3: no repeated safety paragraphs

If a warning appears in a page banner, do not repeat it in every card.

## Rule 4: rename debug labels

Examples:

```text
manual DTO shim       → temporary typed adapter
generated_only        → generated artifact
fixture_only          → demo data
live_with_fallback    → live with demo fallback
backend_only          → not used by this page
ServerOnly            → backend client only
```

Keep exact technical names in Expert mode.

---

# Component-level UX primitives to introduce

Without changing core code behavior, create reusable presentation patterns:

## 1. `OperatorPageHeader`

Fields:

```text
title
purpose
primaryAction
contextChips
mode
```

## 2. `SignalSummaryStrip`

Groups badges into:

```text
System
Context
Evidence
Safety
Contract
```

## 3. `ProgressiveDetail`

Standard disclosure section:

```text
Summary line
View details
Expanded technical table
```

## 4. `EvidenceDrawer`

One consistent place for:

```text
IDs
Evidence Spine links
source attempts
exports
raw previews
```

## 5. `AdvancedDiagnosticsPanel`

Always collapsed unless Expert mode.

## 6. `OperatorNextActionCard`

Every page gets one clear recommendation.

Example:

```text
Next: start a governed fake run to verify Allagma → Kanon → Conexus.
```

---

# Proposed implementation phases

## UX-CLEANUP-001 — UX inventory and page classification

No UI changes yet.

Deliverables:

```text
docs/ux/CONSOLE_PAGE_UX_AUDIT.md
docs/ux/CONSOLE_INFORMATION_HIERARCHY.md
```

For each page:

```text
primary job
secondary jobs
current visible badges
current visible warnings
current action count
what should be default
what should move to details
what should move to Expert mode
```

## UX-CLEANUP-002 — Shared page hierarchy

Add shared layout primitives:

```text
OperatorPageHeader
SignalSummaryStrip
ProgressiveDetail
EvidenceDrawer
AdvancedDiagnosticsPanel
OperatorNextActionCard
```

Apply first to:

```text
Home
Settings
Release Readiness
Topology
```

These are the best low-risk pages.

## UX-CLEANUP-003 — Evidence workbench simplification

Apply to:

```text
Evidence Spine
Agent Interaction
Replay Evidence
Run Audit Journey
```

Introduce tabs/drawers:

```text
Summary
Timeline
Evidence
Diagnostics
Export
```

## UX-CLEANUP-004 — Service page simplification

Apply to:

```text
Kanon Domain Packs
Kanon Source Bindings
Conexus Routing / Observability
Allagma Evaluations
Allagma Start Run
```

## UX-CLEANUP-005 — Navigation restructuring

Reorganize navigation by operator job rather than repo/service boundaries.

## UX-CLEANUP-006 — Expert mode

Add global mode:

```text
Simple / Operator / Expert
```

Default: Operator.

Expert mode reveals:

```text
raw IDs
contract/debug badges
route names
source attempts
generated artifacts
manual DTO / parity details
```

---

# What not to do

Do not remove the truth/evidence work. It is important.

Do not hide critical warnings.

Do not make everything pretty but less trustworthy.

Do not collapse semantic distinctions that matter, such as:

```text
live vs fixture
domain vs Conexus alias
provider key vs provider model
readiness vs connectivity
evidence chain vs metadata coverage
```

The UX goal is:

```text
same truth, better hierarchy
```

# Recommended next package

I would create:

```text
CONSOLE-UX-TIGHTENING-001
```

Scope:

```text
Audit only + design system rules.
No major code changes.
No page rewrites.
```

Acceptance:

```text
Every console page has:
- primary operator job
- secondary/debug details
- badge/warning inventory
- proposed default view
- proposed advanced view
- copy cleanup notes
```

Then implement in slices.

## Bottom line

The console is now technically mature but visually over-disclosing. The next step is a **progressive disclosure UX pass**: fewer visible badges, one clear next action, consistent page hierarchy, evidence/debug in drawers, and a global Simple / Operator / Expert mode.

That will make the system feel like an operator console instead of a living architecture report.
