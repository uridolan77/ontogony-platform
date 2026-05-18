# Frontend Adapter Model

## Suggested TypeScript models

```ts
export type SandboxEvidenceStatus = "none" | "dry_run_only" | "execute_completed" | "execute_failed" | "replay_safe";

export interface SandboxEvidenceViewModel {
  status: SandboxEvidenceStatus;
  hasEvidence: boolean;
  dryRunSideEffectId?: string;
  executeSideEffectId?: string;
  markerRelativePath?: string;
  effectFingerprint?: string;
  executorRef?: string;
  hasReplaySafeExecute: boolean;
  ledgerRows: SideEffectLedgerRowViewModel[];
  warnings: string[];
}

export interface SideEffectLedgerRowViewModel {
  sideEffectId: string;
  phase: "dry_run" | "execute" | string;
  status: string;
  effectFingerprint?: string;
  kanonDecisionId?: string;
  registryVersion?: string;
  executorRef?: string;
  externalRefs?: Record<string, string>;
  traceId?: string;
  recordedAtUtc?: string;
  updatedAtUtc?: string;
  failureClass?: string;
  failureMessage?: string;
}

export interface SandboxExecuteTimelineEventViewModel {
  eventType: string;
  label: string;
  severity: "info" | "success" | "warning" | "error" | "neutral";
  runId?: string;
  toolIntentId?: string;
  sideEffectId?: string;
  effectFingerprint?: string;
  reason?: string;
  failureClass?: string;
}
```

## Adapter rules

`SandboxEvidence` absent -> `status = "none"`. Dry-run row present, execute absent -> `dry_run_only`. Completed/replay-safe execute -> `execute_completed` or `replay_safe`. Failed execute row -> `execute_failed`. Marker path from `SandboxEvidence.MarkerRelativePath`, else external refs fallback. Never expose raw marker content.
