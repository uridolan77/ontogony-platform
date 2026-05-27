Yes. This is the right next move.

Given the download issue, I’m giving it as a **Cursor-ready package prompt** you can paste directly into Cursor from:

```text
C:\dev\metabole-dotnet
```

This package assumes the current foundation-alpha state: lifecycle, actor context, idempotency, error envelope, Postgres durability, SQL Server metadata guardrails, and adapter stubs are already in place. The repo itself now recommends this next direction: **ProgressPlay/gaming schema profiling into SLOD candidates, Kanon decision records, and Conexus advisory mapping suggestions**. 

````markdown
# METABOLE-SQLSERVER-SLOD-PROFILING-001 — Ambitious Deepening Package

You are working in:

```text
C:\dev\metabole-dotnet
````

Repository:

```text
uridolan77/metabole-dotnet
```

## Mission

Deepen Metabole from a foundation-alpha Data Transformation Plane into a serious gaming-domain schema understanding and SLOD mapping candidate engine.

This package must take SQL Server metadata profiles and produce evidence-rich, operator-reviewable, Kanon-validatable SLOD mapping candidates for gaming / iGaming / casino / sportsbook / ProgressPlay-style schemas.

Metabole must remain within its boundary:

Metabole owns:

```text
source schema extraction
schema profiling
domain-shape inference
SLOD mapping candidate generation
mapping evidence
lineage
data-quality signals
operator-review payloads
```

Metabole does **not** own:

```text
semantic truth / ontology authority       → Kanon
LLM provider/model routing                → Conexus
agentic workflow orchestration            → Allagma
shared mechanics                          → Ontogony.Platform, gradually
```

All LLM/Conexus output remains advisory unless validated by Kanon.

---

# 0. Package name

```text
METABOLE-SQLSERVER-SLOD-PROFILING-001
```

Create a package working folder:

```text
docs/_incoming_active/METABOLE-SQLSERVER-SLOD-PROFILING-001/
```

Add:

```text
00_PACKAGE_MANIFEST.md
01_IMPLEMENTATION_PLAN.md
02_ACCEPTANCE_CHECKLIST.md
03_REVIEW_PROMPT.md
04_CURSOR_CONTINUATION_PROMPT.md
```

Also create final completion notes at the end:

```text
docs/implementation/METABOLE_SQLSERVER_SLOD_PROFILING_001_COMPLETION_REPORT.md
```

---

# 1. Strategic objective

Today Metabole can extract and profile schemas. This package must make it useful for real gaming-domain semantic mapping.

The target workflow:

```text
SQL Server metadata
  → ExtractedSchema
  → SchemaProfile
  → GamingDomainProfile
  → SLOD mapping candidate graph
  → Candidate confidence/evidence
  → Conexus advisory explanation/refinement
  → Kanon semantic validation request
  → Operator review payload
  → Durable run/evidence/events
```

The output should help an operator answer:

```text
Which source tables/columns probably represent Player, Account, Wallet, Transaction, Game, Bonus, KYC, Session, Risk, Compliance, Payment, Sportsbook Bet, Casino Round, Affiliate, Brand, Campaign, Device, Geo, Login, Limit, Exclusion, and Audit concepts?
```

---

# 2. Core deliverables

Implement the following workstreams:

```text
METABOLE-GAMING-DOMAIN-PROFILE-001
METABOLE-SLOD-CANDIDATE-MODEL-001
METABOLE-SLOD-HEURISTICS-001
METABOLE-SLOD-EVIDENCE-001
METABOLE-SLOD-CONEXUS-ADVISORY-001
METABOLE-SLOD-KANON-VALIDATION-001
METABOLE-SLOD-OPERATOR-REVIEW-001
METABOLE-SLOD-API-001
METABOLE-SLOD-DURABILITY-001
METABOLE-SLOD-TESTING-001
METABOLE-SLOD-DOCS-001
```

Do not try to make this a full transformation engine yet. This package is about **semantic profiling and mapping candidate generation**, not executing data movement.

---

# 3. New domain concepts

Add new domain/application models. Prefer clear immutable records.

Suggested locations:

```text
src/Metabole.Domain/Gaming/
src/Metabole.Domain/Slod/
src/Metabole.Application/Slod/
src/Metabole.Application/Gaming/
```

## 3.1 GamingDomainProfile

Create:

```text
src/Metabole.Domain/Gaming/GamingDomainProfile.cs
```

Suggested shape:

```csharp
public sealed record GamingDomainProfile(
    string ProfileId,
    string SourceRef,
    string SourceFingerprint,
    IReadOnlyList<GamingDomainObjectProfile> Objects,
    IReadOnlyList<GamingRelationshipProfile> Relationships,
    GamingDomainProfileSummary Summary,
    DateTimeOffset CreatedAtUtc);
```

## 3.2 GamingDomainObjectProfile

```csharp
public sealed record GamingDomainObjectProfile(
    string ObjectProfileId,
    string SourceTableRef,
    string ObjectKind,
    double Confidence,
    IReadOnlyList<string> EvidenceSignals,
    IReadOnlyList<GamingFieldProfile> Fields,
    IReadOnlyList<string> RelatedTableRefs,
    string ReviewStatus);
```

Canonical object kinds for this package:

```text
player
account
wallet
transaction
payment
bonus
game
casino_round
sportsbook_bet
session
login
kyc
risk
limit
self_exclusion
compliance
affiliate
brand
campaign
device
geo
audit
unknown
```

## 3.3 GamingFieldProfile

```csharp
public sealed record GamingFieldProfile(
    string SourceFieldRef,
    string FieldKind,
    string? SuggestedSlodPath,
    double Confidence,
    IReadOnlyList<string> EvidenceSignals,
    bool IsSensitive,
    string ReviewStatus);
```

Field kinds:

```text
identifier
foreign_key
name
email
phone
date_of_birth
created_at
updated_at
currency
amount
balance
status
country
ip_address
device
game_id
bet_amount
win_amount
deposit_amount
withdrawal_amount
bonus_amount
kyc_status
risk_score
limit_amount
exclusion_status
audit_timestamp
unknown
```

## 3.4 SLOD mapping candidate model

Create:

```text
src/Metabole.Domain/Slod/SlodMappingCandidate.cs
```

Suggested shape:

```csharp
public sealed record SlodMappingCandidate(
    string CandidateId,
    string SourceRef,
    string SourceObjectRef,
    string? SourceFieldRef,
    string TargetSlodPath,
    string MappingKind,
    string CandidateType,
    double Confidence,
    IReadOnlyList<string> EvidenceSignals,
    IReadOnlyList<string> SupportingProfileRefs,
    IReadOnlyList<string> Warnings,
    string AuthorityStatus,
    string ReviewStatus,
    string SuggestionSource);
```

MappingKind:

```text
table_to_object
column_to_property
relationship_to_association
enum_to_value_set
status_to_lifecycle_state
amount_to_money_field
timestamp_to_temporal_field
sensitive_field_classification
```

CandidateType:

```text
heuristic
conexus_advisory
operator_seeded
kanon_validated
kanon_rejected
```

AuthorityStatus:

```text
unvalidated
kanon_accepted
kanon_rejected
needs_review
conflicting
```

ReviewStatus:

```text
new
auto_accepted_candidate
needs_operator_review
operator_accepted
operator_rejected
superseded
```

---

# 4. Gaming-domain heuristic engine

Create a service:

```text
src/Metabole.Application/Gaming/GamingDomainProfileBuilder.cs
```

Input:

```text
SchemaProfile
```

Output:

```text
GamingDomainProfile
```

The builder must infer table/object kinds using deterministic heuristics.

## 4.1 Table/object heuristics

Use table names, column names, PK/FK hints, and relationship shape.

Example table-name signals:

```text
Player, Players, Customer, Customers, User, Users, Member, Members
  → player

Account, Accounts, UserAccount, PlayerAccount
  → account

Wallet, Wallets, Balance, Balances, Ledger
  → wallet

Transaction, Transactions, PaymentTransaction, WalletTransaction, LedgerEntry
  → transaction

Payment, Payments, Deposit, Deposits, Withdrawal, Withdrawals, Cashier
  → payment

Bonus, Bonuses, Promotion, Promotions, CampaignBonus
  → bonus

Game, Games, GameRound, Round, CasinoRound
  → game / casino_round

Bet, Bets, Wager, Wagers, SportsBet, BettingTicket
  → sportsbook_bet

Session, Sessions, LoginSession, GameSession
  → session

Login, Logins, AuthLog, AuthenticationLog
  → login

Kyc, Verification, Document, Identity, DueDiligence
  → kyc

Risk, Fraud, AML, Alert, Score
  → risk

Limit, Limits, DepositLimit, LossLimit, WagerLimit, RealityCheck
  → limit

SelfExclusion, Exclusion, CoolOff, ResponsibleGaming
  → self_exclusion

Brand, Brands, WhiteLabel, Skin
  → brand

Affiliate, Affiliates, Partner, Tracker
  → affiliate

Audit, AuditLog, ChangeLog, History
  → audit
```

## 4.2 Column/field heuristics

Column examples:

```text
PlayerId, CustomerId, UserId, MemberId
  → identifier / player.id

Email, EmailAddress
  → email / player.email / sensitive

Phone, Mobile, PhoneNumber
  → phone / sensitive

DOB, DateOfBirth, BirthDate
  → date_of_birth / sensitive

FirstName, LastName, FullName
  → name / sensitive

Country, CountryCode
  → country

Currency, CurrencyCode
  → currency

Balance, CurrentBalance, AvailableBalance
  → balance / wallet.balance

Amount, TransactionAmount, BetAmount, WinAmount
  → amount, bet_amount, win_amount depending table context

DepositAmount
  → deposit_amount

WithdrawalAmount
  → withdrawal_amount

BonusAmount
  → bonus_amount

Status, State
  → status

CreatedAt, CreatedDate, CreatedOn, InsertDate
  → created_at

UpdatedAt, ModifiedAt, LastUpdated
  → updated_at

IPAddress, Ip, LoginIp, RegistrationIp
  → ip_address / sensitive

DeviceId, DeviceFingerprint, UserAgent
  → device / sensitive

RiskScore, FraudScore, AMLScore
  → risk_score

KycStatus, VerificationStatus
  → kyc_status
```

## 4.3 Confidence scoring

Implement deterministic confidence scoring.

Use weighted signals:

```text
table_name_exact_match        +0.35
table_name_partial_match      +0.20
primary_key_name_match        +0.15
foreign_key_relationship      +0.15
column_cluster_match          +0.20
sensitive_field_cluster       +0.10
amount/currency/status combo  +0.15
temporal columns present      +0.05
ambiguous competing object    -0.20
generic table name            -0.15
```

Clamp to:

```text
0.0 <= confidence <= 1.0
```

Suggested thresholds:

```text
>= 0.85 → auto_accepted_candidate
>= 0.65 → needs_operator_review
<  0.65 → low_confidence / unknown
```

Do not silently drop low-confidence candidates. Keep them as `unknown` or `needs_operator_review` with warnings.

---

# 5. SLOD path vocabulary

Create a v0 gaming SLOD vocabulary doc:

```text
docs/contracts/GAMING_SLOD_V0.md
```

Also create code:

```text
src/Metabole.Domain/Slod/GamingSlodPaths.cs
```

Suggested v0 paths:

```text
gaming.player.id
gaming.player.externalId
gaming.player.username
gaming.player.email
gaming.player.phone
gaming.player.firstName
gaming.player.lastName
gaming.player.dateOfBirth
gaming.player.country
gaming.player.registrationDate
gaming.player.status

gaming.account.id
gaming.account.playerId
gaming.account.brandId
gaming.account.currency
gaming.account.status
gaming.account.createdAt

gaming.wallet.id
gaming.wallet.playerId
gaming.wallet.currency
gaming.wallet.balance
gaming.wallet.availableBalance
gaming.wallet.lockedBalance
gaming.wallet.updatedAt

gaming.transaction.id
gaming.transaction.playerId
gaming.transaction.walletId
gaming.transaction.type
gaming.transaction.amount
gaming.transaction.currency
gaming.transaction.status
gaming.transaction.createdAt

gaming.payment.id
gaming.payment.playerId
gaming.payment.type
gaming.payment.provider
gaming.payment.amount
gaming.payment.currency
gaming.payment.status
gaming.payment.createdAt

gaming.bonus.id
gaming.bonus.playerId
gaming.bonus.campaignId
gaming.bonus.amount
gaming.bonus.status
gaming.bonus.createdAt
gaming.bonus.expiresAt

gaming.game.id
gaming.game.provider
gaming.game.name
gaming.game.category

gaming.casinoRound.id
gaming.casinoRound.playerId
gaming.casinoRound.gameId
gaming.casinoRound.betAmount
gaming.casinoRound.winAmount
gaming.casinoRound.currency
gaming.casinoRound.startedAt
gaming.casinoRound.endedAt

gaming.sportsbookBet.id
gaming.sportsbookBet.playerId
gaming.sportsbookBet.stakeAmount
gaming.sportsbookBet.payoutAmount
gaming.sportsbookBet.currency
gaming.sportsbookBet.status
gaming.sportsbookBet.placedAt
gaming.sportsbookBet.settledAt

gaming.session.id
gaming.session.playerId
gaming.session.startedAt
gaming.session.endedAt
gaming.session.ipAddress
gaming.session.deviceId

gaming.kyc.id
gaming.kyc.playerId
gaming.kyc.status
gaming.kyc.documentType
gaming.kyc.updatedAt

gaming.risk.playerId
gaming.risk.score
gaming.risk.alertType
gaming.risk.status
gaming.risk.createdAt

gaming.limit.playerId
gaming.limit.type
gaming.limit.amount
gaming.limit.period
gaming.limit.createdAt

gaming.selfExclusion.playerId
gaming.selfExclusion.status
gaming.selfExclusion.startedAt
gaming.selfExclusion.endsAt

gaming.brand.id
gaming.brand.name

gaming.affiliate.id
gaming.affiliate.name
gaming.affiliate.trackerCode

gaming.audit.id
gaming.audit.actorId
gaming.audit.action
gaming.audit.entityRef
gaming.audit.createdAt
```

This is not Kanon’s ontology truth. It is Metabole’s candidate vocabulary for mapping suggestions. Kanon remains authority.

---

# 6. SLOD candidate generation service

Create:

```text
src/Metabole.Application/Slod/SlodMappingCandidateGenerator.cs
```

Input:

```text
SchemaProfile
GamingDomainProfile
targetSlodModelRef
```

Output:

```text
IReadOnlyList<SlodMappingCandidate>
```

Rules:

1. Generate table-to-object candidates for object profiles.
2. Generate column-to-property candidates for field profiles.
3. Generate relationship-to-association candidates from source FK candidates.
4. Include source profile refs for every candidate.
5. Include evidence signals.
6. Include warnings for ambiguity:

   * multiple possible object kinds
   * generic names like `Id`, `Status`, `Type`, `Amount`
   * sensitive fields
   * missing primary key
   * missing player relationship
   * table has amount but no currency
   * table has transaction-like name but no timestamp
7. Do not create `kanon_accepted` status locally. Everything starts `unvalidated`.

---

# 7. SLOD profiling pipeline

Add a new application service:

```text
src/Metabole.Application/Services/StartSlodProfilingPipelineService.cs
```

New request contract:

```text
src/Metabole.Application/Contracts/StartSlodProfilingPipelineRequest.cs
```

Suggested shape:

```csharp
public sealed record StartSlodProfilingPipelineRequest(
    SourceConnectorSpec Connector,
    string TargetSlodModelRef,
    bool IncludeConexusAdvisory = true,
    bool IncludeKanonValidation = true,
    IReadOnlyDictionary<string, string>? Options = null);
```

New response:

```csharp
public sealed record SlodProfilingPipelineResponse(
    string RunId,
    string Status,
    string PipelineKind,
    string SourceRef,
    string TargetSlodModelRef,
    string? EvidenceId,
    int ObjectCandidateCount,
    int FieldCandidateCount,
    int RelationshipCandidateCount,
    int NeedsReviewCount,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? CompletedAtUtc);
```

Pipeline kind:

```text
slod_profile
```

Flow:

```text
Accepted
→ ExtractingSchema
→ ProfilingSchema
→ GeneratingMappingCandidates
→ AwaitingSemanticValidation
→ Completed
```

You may reuse existing lifecycle statuses.

Pipeline behavior:

```text
1. Extract schema.
2. Create base SchemaProfile.
3. Build GamingDomainProfile.
4. Generate SlodMappingCandidates.
5. Optionally call Conexus for advisory refinement/explanation.
6. Optionally call Kanon for semantic validation.
7. Merge advisory/validation results without overwriting deterministic evidence.
8. Create enhanced LineageEvidence.
9. Save run.
```

Because `PipelineRun` currently stores `SchemaProfile`, `SemanticMappingCandidate`, and `SemanticMappingValidation`, decide one of two strategies:

Preferred strategy:

```text
Add Slod-specific fields to PipelineRun:
  GamingDomainProfile?
  SlodMappingCandidates
  SlodMappingValidations?
```

Acceptable strategy:

```text
Store SLOD candidates inside existing mapping candidate shape only if no information is lost.
```

Preferred is better. Do not force a rich SLOD candidate into a generic `SemanticMappingCandidate` if it loses evidence/warnings/review state.

---

# 8. API routes

Add v0 routes:

```text
POST /metabole/v0/pipeline-runs/slod-profile
GET  /metabole/v0/pipeline-runs/{runId}/gaming-domain-profile
GET  /metabole/v0/pipeline-runs/{runId}/slod-candidates
GET  /metabole/v0/pipeline-runs/{runId}/operator-review
POST /metabole/v0/slod-candidates
```

## 8.1 POST /pipeline-runs/slod-profile

Starts full SLOD profiling pipeline.

Support idempotency exactly like schema-profile pipeline.

Use operation kind:

```text
slod_profile_pipeline
```

## 8.2 GET /gaming-domain-profile

Returns `GamingDomainProfile` for a run.

If missing:

```text
metabole.gaming_domain_profile.not_found
```

## 8.3 GET /slod-candidates

Returns candidates with optional filters:

```text
?objectKind=player
?reviewStatus=needs_operator_review
?authorityStatus=unvalidated
?minConfidence=0.65
```

Do simple in-memory filtering after loading the run.

## 8.4 GET /operator-review

Returns operator-oriented review payload.

Suggested shape:

```csharp
public sealed record SlodOperatorReviewPayload(
    string RunId,
    string SourceRef,
    string TargetSlodModelRef,
    IReadOnlyList<SlodOperatorReviewObjectGroup> Groups,
    SlodOperatorReviewSummary Summary);
```

Group by object kind/source table.

Each group should include:

```text
source table
suggested object kind
confidence
evidence signals
warnings
field candidates
relationship candidates
kanon validations if present
conexus advisory notes if present
```

## 8.5 POST /slod-candidates

Ad-hoc endpoint:

```text
SchemaProfile + options
  → GamingDomainProfile
  → SLOD candidates
```

Useful for frontend/operator previews without creating full pipeline run.

---

# 9. Kanon validation integration

Do not assume Kanon routes already exist. Add Metabole-side contract and adapter methods behind feature mode.

Create or extend:

```text
src/Metabole.Application/Abstractions/ISlodSemanticValidationClient.cs
src/Metabole.Infrastructure/KanonHttpSlodValidationClient.cs
src/Metabole.Infrastructure/FakeKanonSlodValidationClient.cs
```

Proposed Kanon route:

```text
POST /ontology/v0/metabole/slod-candidates/validate
```

Request:

```csharp
public sealed record KanonSlodValidationRequest(
    string RunId,
    string TraceId,
    string CorrelationId,
    ActorContext Actor,
    string SourceFingerprint,
    string TargetSlodModelRef,
    IReadOnlyList<SlodMappingCandidate> Candidates);
```

Response:

```csharp
public sealed record KanonSlodValidationResponse(
    IReadOnlyList<SlodMappingValidation> Validations);
```

Validation shape:

```csharp
public sealed record SlodMappingValidation(
    string CandidateId,
    string Authority,
    string Decision,
    string? DecisionId,
    double Confidence,
    IReadOnlyList<string> Reasons,
    IReadOnlyList<string> Warnings);
```

Rules:

* Fake Kanon can accept high-confidence candidates and mark ambiguous candidates as `needs_review`.
* HTTP Kanon failures map to stable error code:

  * `metabole.adapter.kanon.slod_validation_failed`
* If Kanon route is unavailable and mode is fake/default, pipeline continues.
* If HTTP mode is enabled and Kanon fails, use existing adapter error semantics.

---

# 10. Conexus advisory integration

Do not let Metabole pick models. Conexus owns routing.

Create or extend:

```text
src/Metabole.Application/Abstractions/ISlodAdvisoryClient.cs
src/Metabole.Infrastructure/ConexusHttpSlodAdvisoryClient.cs
src/Metabole.Infrastructure/FakeConexusSlodAdvisoryClient.cs
```

Proposed Conexus route:

```text
POST /conexus/v0/metabole/slod-candidates/advise
```

Request:

```csharp
public sealed record ConexusSlodAdvisoryRequest(
    string RunId,
    string TraceId,
    string CorrelationId,
    ActorContext Actor,
    string SourceFingerprint,
    string TargetSlodModelRef,
    GamingDomainProfile DomainProfile,
    IReadOnlyList<SlodMappingCandidate> Candidates);
```

Response:

```csharp
public sealed record ConexusSlodAdvisoryResponse(
    IReadOnlyList<SlodCandidateAdvisory> Advisories);
```

Advisory shape:

```csharp
public sealed record SlodCandidateAdvisory(
    string CandidateId,
    string AdvisoryId,
    string Summary,
    IReadOnlyList<string> SuggestedEvidenceAdditions,
    IReadOnlyList<string> SuggestedWarnings,
    double? SuggestedConfidenceAdjustment);
```

Rules:

* Conexus advisory must not directly mark candidates as accepted.
* Advisory may add explanations, warnings, and confidence adjustment suggestions.
* Deterministic evidence must remain separate from LLM advisory evidence.
* Candidate output should preserve:

  * deterministicConfidence
  * advisoryConfidenceAdjustment
  * finalConfidence
  * suggestionSource

---

# 11. Evidence model extension

Extend evidence for SLOD profiling.

Create:

```text
src/Metabole.Domain/Slod/SlodProfilingEvidence.cs
```

Suggested shape:

```csharp
public sealed record SlodProfilingEvidence(
    string EvidenceId,
    string RunId,
    string SourceFingerprint,
    string SchemaProfileId,
    string GamingDomainProfileId,
    IReadOnlyList<string> SlodCandidateIds,
    IReadOnlyList<string> KanonDecisionIds,
    IReadOnlyList<string> ConexusAdvisoryIds,
    IReadOnlyList<string> WarningCodes,
    string TargetSlodModelRef,
    string RedactionPolicy,
    DateTimeOffset CreatedAtUtc,
    string? ActorId,
    string TraceId,
    string CorrelationId);
```

Add to run/evidence output.

Evidence must never include:

```text
connection strings
raw credentials
SQL text from user request
full raw data rows
secrets
provider/model names
```

---

# 12. Operator review model

Create:

```text
src/Metabole.Application/Slod/SlodOperatorReviewBuilder.cs
src/Metabole.Application/Contracts/SlodOperatorReviewPayload.cs
```

Review payload must be optimized for the future frontend/operator console.

Group by:

```text
object kind
source table
confidence band
review status
```

Confidence bands:

```text
high      >= 0.85
medium    >= 0.65 and < 0.85
low       < 0.65
```

Include summary:

```text
total candidates
high confidence
medium confidence
low confidence
needs review
sensitive fields
kanon accepted
kanon rejected
kanon needs review
conexus advisories
warnings
```

Operator review should make ambiguity explicit, for example:

```text
Table dbo.Transactions looks like both payment and wallet transaction.
Column Amount is generic; table context suggests transaction.amount.
Column Status needs enum/value-set review.
Column Email is sensitive.
Table PlayerLimits suggests responsible-gaming limit object.
```

---

# 13. Durability extension

If `PipelineRun` is extended with new SLOD fields, ensure both in-memory and Postgres JSON serialization roundtrip.

Postgres may continue storing the full run payload as JSONB, but add optional normalized table if reasonable:

```text
metabole_slod_candidates
```

Suggested table:

```sql
CREATE TABLE IF NOT EXISTS metabole_slod_candidates (
    candidate_id text PRIMARY KEY,
    run_id text NOT NULL,
    source_object_ref text NOT NULL,
    source_field_ref text NULL,
    target_slod_path text NOT NULL,
    mapping_kind text NOT NULL,
    confidence double precision NOT NULL,
    authority_status text NOT NULL,
    review_status text NOT NULL,
    payload jsonb NOT NULL,
    created_at timestamptz NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS ix_metabole_slod_candidates_run_id
    ON metabole_slod_candidates (run_id);

CREATE INDEX IF NOT EXISTS ix_metabole_slod_candidates_target_slod_path
    ON metabole_slod_candidates (target_slod_path);

CREATE INDEX IF NOT EXISTS ix_metabole_slod_candidates_review_status
    ON metabole_slod_candidates (review_status);
```

Do not make Postgres required for default tests.

---

# 14. Error codes

Add stable error codes:

```text
metabole.slod.profile.not_found
metabole.slod.candidates.not_found
metabole.slod.operator_review.not_found
metabole.slod.target_model_missing
metabole.slod.candidate_generation_failed
metabole.gaming_domain_profile.not_found
metabole.gaming_domain_profile.generation_failed
metabole.adapter.kanon.slod_validation_failed
metabole.adapter.conexus.slod_advisory_failed
```

Add stages:

```text
gaming_domain_profile
slod_candidate_generation
slod_advisory
slod_validation
slod_operator_review
read_slod_candidates
read_gaming_domain_profile
```

---

# 15. Documentation

Create:

```text
docs/contracts/GAMING_SLOD_V0.md
docs/contracts/SLOD_MAPPING_CANDIDATE_V0.md
docs/contracts/GAMING_DOMAIN_PROFILE_V0.md
docs/contracts/SLOD_OPERATOR_REVIEW_V0.md
docs/contracts/METABOLE_SLOD_ADAPTER_CONTRACTS_V0.md
docs/architecture/METABOLE_SLOD_PROFILING_ARCHITECTURE.md
docs/runbooks/METABOLE_SLOD_PROFILING_LOCAL_RUNBOOK.md
docs/testing/METABOLE_SLOD_PROFILING_TEST_MATRIX.md
```

Update:

```text
README.md
docs/system/METABOLE_SYSTEM_MATRIX.md
docs/status/METABOLE_FOUNDATION_ALPHA_STATUS.md
docs/system/METABOLE_ROUTE_INVENTORY.md
```

README should add:

```text
SLOD profiling pipeline
Gaming-domain profile
SLOD mapping candidates
Operator review payload
Kanon/Conexus SLOD adapter contracts
```

Be honest that:

```text
- SLOD v0 is candidate vocabulary, not final ontology truth.
- Kanon remains semantic authority.
- Conexus remains advisory.
- ProgressPlay-specific mapping is heuristic-first and review-oriented.
- No production ETL/data movement is implemented by this package.
```

---

# 16. Tests

Add tests across relevant projects.

## 16.1 Domain tests

```text
tests/Metabole.Domain.Tests/GamingDomainProfileTests.cs
tests/Metabole.Domain.Tests/SlodMappingCandidateTests.cs
```

Cover:

```text
candidate confidence clamp
review status threshold behavior
sensitive field classification
authority status defaults to unvalidated
candidate IDs are stable/deterministic
```

## 16.2 Application tests

```text
tests/Metabole.Application.Tests/GamingDomainProfileBuilderTests.cs
tests/Metabole.Application.Tests/SlodMappingCandidateGeneratorTests.cs
tests/Metabole.Application.Tests/SlodOperatorReviewBuilderTests.cs
tests/Metabole.Application.Tests/StartSlodProfilingPipelineServiceTests.cs
```

Cover:

```text
Players table -> player object
Customers table -> player object
Wallets table -> wallet object
Transactions table -> transaction object
Deposits/Withdrawals -> payment objects
CasinoRounds -> casino_round object
Bets/Wagers -> sportsbook_bet object
KYC/Verification -> kyc object
Risk/Fraud/AML -> risk object
PlayerLimits -> limit object
SelfExclusion -> self_exclusion object
Email/phone/DOB/IP/device classified sensitive
Amount + Currency pair improves confidence
Generic Status produces warning
Missing Currency on amount table produces warning
FK from Transactions.PlayerId to Players.PlayerId creates relationship candidate
High-confidence candidates get reviewStatus auto_accepted_candidate
Medium confidence candidates get needs_operator_review
Low-confidence candidates are preserved, not dropped
```

## 16.3 API tests

```text
tests/Metabole.Api.Tests/SlodProfilingEndpointTests.cs
```

Cover:

```text
POST /metabole/v0/pipeline-runs/slod-profile returns Accepted
GET /gaming-domain-profile returns profile
GET /slod-candidates returns candidates
GET /slod-candidates?objectKind=player filters
GET /operator-review returns grouped payload
missing run returns stable error envelope
idempotency replay works for slod-profile route
```

## 16.4 Adapter tests

```text
tests/Metabole.Tests/SlodAdapterContractTests.cs
```

Use fake HTTP handler / in-process stub.

Cover:

```text
Conexus SLOD advisory sends correct route
Conexus payload includes actor/trace/correlation/source fingerprint
Conexus payload excludes credentialRef/connection string
Kanon SLOD validation sends correct route
Kanon payload includes candidates
Kanon non-success maps stable error
Conexus non-success maps stable error
```

## 16.5 Architecture tests

Extend forbidden dependency tests:

```text
No direct Kanon/Conexus/Allagma project references
No provider/model names
No raw OpenAI/Anthropic/Azure AI SDK references
No SQL text accepted from request body
```

## 16.6 Test data fixtures

Create fixtures:

```text
tests/Metabole.Tests/Fixtures/gaming-schema-basic.json
tests/Metabole.Tests/Fixtures/gaming-schema-payments.json
tests/Metabole.Tests/Fixtures/gaming-schema-risk-kyc.json
tests/Metabole.Tests/Fixtures/gaming-schema-sportsbook.json
```

Fixtures can deserialize to `ExtractedSchema` or `SchemaProfile`.

---

# 17. Route inventory

Update route inventory to include:

```text
POST /metabole/v0/pipeline-runs/slod-profile
GET  /metabole/v0/pipeline-runs/{runId}/gaming-domain-profile
GET  /metabole/v0/pipeline-runs/{runId}/slod-candidates
GET  /metabole/v0/pipeline-runs/{runId}/operator-review
POST /metabole/v0/slod-candidates
```

Ensure architecture route inventory tests include these.

---

# 18. Acceptance criteria

The package is complete only when:

```powershell
dotnet restore
dotnet build --configuration Release
dotnet test --configuration Release
```

pass.

Also verify:

```text
- New SLOD profiling pipeline exists.
- New API route POST /metabole/v0/pipeline-runs/slod-profile exists.
- GamingDomainProfile is generated from SchemaProfile.
- SlodMappingCandidate objects are generated with evidence, warnings, confidence, authority status, and review status.
- Operator review payload groups candidates by source table/object kind.
- Sensitive fields are classified and surfaced.
- Candidate generation is deterministic.
- Low-confidence candidates are preserved with warnings.
- Kanon SLOD validation adapter is contract-defined and fake implementation exists.
- Conexus SLOD advisory adapter is contract-defined and fake implementation exists.
- HTTP adapter payloads include actor/trace/correlation and exclude credentials.
- Idempotency works for SLOD profiling route.
- Stable error envelopes exist for missing SLOD artifacts.
- Postgres serialization still roundtrips extended run payloads.
- Route inventory docs/tests are updated.
- README is updated honestly.
- No LLM provider/model names are hard-coded.
- No Metabole code claims Kanon authority.
```

---

# 19. Suggested implementation order

Use this order:

```text
1. Docs/contracts first.
2. Domain records: GamingDomainProfile, SlodMappingCandidate, evidence/review records.
3. GamingSlodPaths vocabulary.
4. GamingDomainProfileBuilder heuristics.
5. SlodMappingCandidateGenerator.
6. SlodOperatorReviewBuilder.
7. Extend PipelineRun carefully.
8. Add StartSlodProfilingPipelineService.
9. Add fake Kanon/Conexus SLOD clients.
10. Add HTTP Kanon/Conexus SLOD clients.
11. Add API routes.
12. Add Postgres JSON roundtrip and optional normalized SLOD candidate table.
13. Add tests.
14. Update docs, README, route inventory, system matrix.
15. Run full Release gate.
16. Write completion report.
```

---

# 20. Completion report

At the end create:

```text
docs/implementation/METABOLE_SQLSERVER_SLOD_PROFILING_001_COMPLETION_REPORT.md
```

Include:

```text
Summary
Files changed
Routes added
Contracts added
Domain models added
Heuristics implemented
Tests added
Commands run
Known limitations
Next recommended package
```

Recommended next package after this:

```text
METABOLE-SLOD-OPERATOR-WORKBENCH-001
```

or, if backend depth is preferred first:

```text
METABOLE-REPLAY-BUNDLE-001
```

The first would expose this through the Ontogony frontend/operator console. The second would make Metabole’s evidence spine reconstructable and replay-capable.

```

Start with the package docs, then implement in small passes. Do not skip tests.
```
