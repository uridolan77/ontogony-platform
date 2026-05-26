# Suggested test cases

## `ontogony-ui`

```ts
it('requires labeled unknown states')
it('maps live connectivity to positive severity')
it('maps not_ready readiness to warning or critical')
it('does not allow fixture data source to count as live readiness')
it('renders not_applicable evidence separately from unresolved')
it('renders planned topology edge separately from validated edge')
```

## `ontogony-frontend`

```ts
it('does not render Home as healthy when Conexus readiness is not_ready')
it('renders fixture fallback as badge and not page heading')
it('renders compatibility unknown with missing metadata reason')
it('renders Evidence Spine partial with missing reason code')
it('renders Agent Interaction fixture replay as demo fixture not live evidence')
it('renders Allagma run unknown task type with label or hides it')
it('renders Settings missing credential source as not set')
```

## String scan

Run after build/test to catch forbidden terms in source or snapshots, then manually review expected exceptions in docs/tests.
