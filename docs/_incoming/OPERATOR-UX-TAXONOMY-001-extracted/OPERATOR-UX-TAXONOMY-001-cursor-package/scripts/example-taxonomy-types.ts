// Example TypeScript sketch only. Adapt names and file paths to the actual repos.

export type ConnectivityStatus = 'live' | 'degraded' | 'offline' | 'unknown';
export type ReadinessStatus = 'ready' | 'not_ready' | 'unknown';
export type ContractHealthStatus = 'valid' | 'warning' | 'invalid' | 'unknown';
export type OperatorUsabilityStatus = 'usable' | 'degraded' | 'blocked' | 'unknown';
export type EvidenceCompletenessStatus = 'resolved' | 'partial' | 'unresolved' | 'not_applicable' | 'unknown';
export type DataSourceStatus = 'live' | 'live_with_fallback' | 'fixture' | 'generated' | 'imported' | 'mock' | 'unknown';
export type AuthorityStatus = 'authoritative' | 'advisory' | 'demo' | 'inferred' | 'historical' | 'unknown';
export type TopologyEdgeStatus = 'validated' | 'degraded' | 'missing' | 'planned' | 'blocked' | 'unknown';

export type OperatorStatusSeverity = 'positive' | 'neutral' | 'info' | 'warning' | 'critical';

export interface OperatorStatusViewModel<TState extends string = string> {
  dimension: string;
  state: TState;
  label: string;
  severity: OperatorStatusSeverity;
  reason?: string;
  details?: string[];
  source?: string;
  checkedAt?: string;
  nextAction?: string;
}

export function labeledUnknown(subject: string, reason: string): OperatorStatusViewModel<'unknown'> {
  return {
    dimension: 'unknown',
    state: 'unknown',
    label: `${subject}: unknown`,
    reason,
    severity: 'warning'
  };
}

export function canCountAsLiveReadiness(source: DataSourceStatus): boolean {
  return source === 'live';
}

export function isEvidenceFailure(state: EvidenceCompletenessStatus): boolean {
  return state === 'partial' || state === 'unresolved';
}
