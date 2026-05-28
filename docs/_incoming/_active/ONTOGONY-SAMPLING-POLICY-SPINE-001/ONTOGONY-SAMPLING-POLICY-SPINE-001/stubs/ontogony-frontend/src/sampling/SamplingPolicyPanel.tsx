import type { SamplingPolicyTrace } from './samplingPolicyTypes';

interface Props {
  sampling?: SamplingPolicyTrace | null;
}

export function SamplingPolicyPanel({ sampling }: Props) {
  if (!sampling) {
    return <div className="text-sm text-muted-foreground">No sampling policy trace recorded.</div>;
  }

  const hasIssues = sampling.violations.length > 0 || sampling.warnings.length > 0;

  return (
    <section className="rounded-2xl border p-4 shadow-sm">
      <div className="flex items-center justify-between gap-3">
        <div>
          <h3 className="text-sm font-semibold">Sampling policy</h3>
          <p className="text-xs text-muted-foreground">{sampling.policyBasis}</p>
        </div>
        <span className="rounded-full border px-3 py-1 text-xs font-medium">
          {sampling.effectiveProfileId} · {sampling.decision}
        </span>
      </div>

      <dl className="mt-4 grid grid-cols-2 gap-3 text-sm md:grid-cols-4">
        <div>
          <dt className="text-muted-foreground">Requested</dt>
          <dd className="font-medium">{sampling.requestedProfileId ?? 'auto'}</dd>
        </div>
        <div>
          <dt className="text-muted-foreground">Temperature</dt>
          <dd className="font-medium">{sampling.effectiveParameters.temperature ?? '—'}</dd>
        </div>
        <div>
          <dt className="text-muted-foreground">Top-p</dt>
          <dd className="font-medium">{sampling.effectiveParameters.topP ?? '—'}</dd>
        </div>
        <div>
          <dt className="text-muted-foreground">Candidates</dt>
          <dd className="font-medium">{sampling.effectiveParameters.candidateCount ?? '—'}</dd>
        </div>
      </dl>

      {hasIssues && (
        <div className="mt-4 space-y-2">
          {[...sampling.violations, ...sampling.warnings].map((notice) => (
            <div key={`${notice.severity}-${notice.code}`} className="rounded-xl border p-3 text-sm">
              <div className="font-medium">{notice.code}</div>
              <div className="text-muted-foreground">{notice.message}</div>
            </div>
          ))}
        </div>
      )}
    </section>
  );
}
