# Missing edge reason catalog

Use stable, understandable reasons:

```text
not_found
service_unconfigured
service_unreachable
browser_blocked
actor_forbidden
route_not_exposed
id_not_present_in_source
ambiguous_identifier
filtered_lookup_empty
capability_not_supported
```

Each missing edge should include:
- expected source
- attempted lookup
- reason
- suggested next step if known
