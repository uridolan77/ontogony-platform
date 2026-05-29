import http from 'k6/http';
import { check } from 'k6';

export const options = { vus: 5, duration: '20s' };

const services = [
  __ENV.KANON_BASE_URL || 'http://localhost:5081',
  __ENV.CONEXUS_BASE_URL || 'http://localhost:5082',
  __ENV.ALLAGMA_BASE_URL || 'http://localhost:5083',
  __ENV.METABOLE_BASE_URL || 'http://localhost:5084',
  __ENV.AISTHESIS_BASE_URL || 'http://localhost:5085'
];

export default function () {
  for (const base of services) {
    const res = http.get(`${base}/health`);
    check(res, { [`${base} health success`]: r => r.status >= 200 && r.status < 300 });
  }
}
