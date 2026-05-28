export type SamplingPolicyDecision =
  | 'Allowed'
  | 'AllowedWithWarnings'
  | 'Denied'
  | 'RequiresApproval';

export interface SamplingParameters {
  temperature?: number | null;
  topP?: number | null;
  candidateCount?: number | null;
  seed?: number | null;
  presencePenalty?: number | null;
  frequencyPenalty?: number | null;
}

export interface SamplingPolicyNotice {
  code: string;
  severity: 'info' | 'warning' | 'error';
  message: string;
  recommendedProfileId?: string | null;
}

export interface SamplingPolicyTrace {
  contractVersion: 'sampling.policy.trace.v0';
  resolutionId: string;
  requestedProfileId?: string | null;
  effectiveProfileId: string;
  decision: SamplingPolicyDecision;
  policyBasis: string;
  requestedParameters?: SamplingParameters | null;
  effectiveParameters: SamplingParameters;
  providerParameters?: Record<string, unknown> | null;
  warnings: SamplingPolicyNotice[];
  violations: SamplingPolicyNotice[];
}

export interface SamplingProfile {
  id: string;
  displayName: string;
  intent: string;
  temperature: number;
  topP: number;
  candidateCount: number;
  directExecutionAllowed: boolean;
  allowsTools: boolean;
  allowsSideEffects: boolean;
  determinismGuarantee: string;
  allowedTaskKinds: string[];
  blockedOutputContracts: string[];
  notes: string[];
}
