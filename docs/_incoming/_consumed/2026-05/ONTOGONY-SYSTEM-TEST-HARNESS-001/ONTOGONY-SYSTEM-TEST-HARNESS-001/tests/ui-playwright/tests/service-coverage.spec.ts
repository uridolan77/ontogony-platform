import { test, expect } from '@playwright/test';

const serviceKeywords = [
  'Allagma',
  'Kanon',
  'Conexus',
  'Metabole',
  'Aisthesis'
];

test('console loads and exposes service-oriented navigation or content', async ({ page }) => {
  await page.goto('/');
  await expect(page).toHaveTitle(/Ontogony|Vite|Console|React/i);

  const body = await page.locator('body').innerText();
  const found = serviceKeywords.filter(keyword => body.toLowerCase().includes(keyword.toLowerCase()));

  // Starter assertion: final calibrated UI tests should navigate exact routes/components.
  expect(found.length, `Expected at least one service keyword on shell; found ${found.join(', ')}`).toBeGreaterThanOrEqual(1);
});

test('console should not expose obvious service secrets in rendered HTML', async ({ page }) => {
  await page.goto('/');
  const html = await page.content();
  expect(html).not.toContain('dev-allagma-service-token');
  expect(html).not.toContain('dev-conexus-project-key');
  expect(html).not.toContain('dev-kanon-service-token');
});
