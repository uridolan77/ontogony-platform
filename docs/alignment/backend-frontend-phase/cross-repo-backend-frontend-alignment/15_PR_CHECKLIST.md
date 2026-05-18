# PR Checklist for Backend/Frontend Alignment

## General

- [ ] Uses Allagma, not Agentor, in current code/docs.
- [ ] Does not invent or imply unsupported routes.
- [ ] Updates docs/changelog.
- [ ] Tests added for success and failure.
- [ ] Redaction considered.

## Backend route PR

- [ ] OpenAPI path added.
- [ ] Request/response schemas typed.
- [ ] Auth/authorization behavior tested.
- [ ] Error envelope tested.
- [ ] Trace/correlation metadata included.
- [ ] Redaction tested.
- [ ] CI exports OpenAPI/provenance artifact.

## Frontend PR

- [ ] OpenAPI snapshot refreshed.
- [ ] `docs/openapi/PROVENANCE.md` updated.
- [ ] Generated clients updated.
- [ ] `npm run contracts:audit` passes.
- [ ] Route/E2E coverage catalog updated if UI changed.
- [ ] `npm run check:full` passes.
- [ ] Limitation banners updated honestly.
