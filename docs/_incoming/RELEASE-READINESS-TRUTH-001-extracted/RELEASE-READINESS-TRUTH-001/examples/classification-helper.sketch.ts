// Sketch only. Adapt to existing repo conventions.

type DataSource = 'live' | 'live_with_fallback' | 'fixture_only' | 'generated_only' | 'unknown';
type ArtifactStatus = 'ready' | 'partial' | 'gap' | 'unknown';
type ReleaseImpact =
  | 'release_candidate'
  | 'needs_live_validation'
  | 'needs_review'
  | 'demo_only'
  | 'unresolved'
  | 'not_release_ready';

interface ClassifyRouteInput {
  route: string;
  dataSource: DataSource;
  artifactStatus: ArtifactStatus;
  liveValidationAttached: boolean;
}

interface ClassifyRouteOutput {
  releaseImpact: ReleaseImpact;
  isBlocking: boolean;
  reason: string;
  nextAction: string;
}

export function classifyReleaseReadinessRoute(input: ClassifyRouteInput): ClassifyRouteOutput {
  if (input.dataSource === 'fixture_only' || input.dataSource === 'generated_only') {
    return {
      releaseImpact: 'demo_only',
      isBlocking: false,
      reason: 'Generated or fixture-backed route; not live release evidence.',
      nextAction: 'Exclude from RC posture or attach live validation.',
    };
  }

  if (input.dataSource === 'unknown') {
    return {
      releaseImpact: 'unresolved',
      isBlocking: true,
      reason: 'Data source is not classified.',
      nextAction: 'Classify as live, live_with_fallback, fixture_only, generated_only, or unsupported.',
    };
  }

  if (input.artifactStatus === 'gap') {
    return {
      releaseImpact: 'not_release_ready',
      isBlocking: true,
      reason: 'Generated artifact marks this route as a gap.',
      nextAction: 'Implement or explicitly defer this route before release planning.',
    };
  }

  if (input.dataSource === 'live_with_fallback') {
    return {
      releaseImpact: 'needs_review',
      isBlocking: true,
      reason: 'Live data is mixed with fallback values.',
      nextAction: 'Replace fallback with live data or document fallback as non-critical.',
    };
  }

  if (input.dataSource === 'live' && input.artifactStatus === 'ready' && input.liveValidationAttached) {
    return {
      releaseImpact: 'release_candidate',
      isBlocking: false,
      reason: 'Route is live-backed and live validation is attached.',
      nextAction: 'No action required for this route.',
    };
  }

  return {
    releaseImpact: 'needs_live_validation',
    isBlocking: true,
    reason: 'Artifact status alone cannot certify release readiness.',
    nextAction: 'Attach live validation before counting this route toward RC posture.',
  };
}
