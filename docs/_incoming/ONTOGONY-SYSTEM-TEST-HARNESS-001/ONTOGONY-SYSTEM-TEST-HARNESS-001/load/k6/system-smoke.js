import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  vus: 2,
  duration: '30s',
  thresholds: {
    http_req_failed: ['rate<0.05'],
    http_req_duration: ['p(95)<1000']
  }
};

const ALLAGMA = __ENV.ALLAGMA_BASE_URL || 'http://localhost:5083';
const TOKEN = __ENV.ALLAGMA_SERVICE_TOKEN || 'dev-allagma-service-token';

export default function () {
  const scenarioId = `k6-smoke-${Date.now()}-${__VU}-${__ITER}`;
  const payload = JSON.stringify({
    domain: 'gaming-core',
    intent: 'SummarizePlayerRisk',
    input: { playerId: 'sys-test-k6-player', riskSignals: ['load_smoke'] }
  });

  const res = http.post(`${ALLAGMA}/allagma/v0/runs`, payload, {
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${TOKEN}`,
      'X-Correlation-ID': scenarioId,
      'Idempotency-Key': scenarioId
    }
  });

  check(res, {
    'status is 2xx or calibrated client error': r => (r.status >= 200 && r.status < 300) || [400, 404, 422, 501].includes(r.status),
    'body is present': r => !!r.body
  });

  sleep(1);
}
