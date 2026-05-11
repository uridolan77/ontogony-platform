# Conexus Event Emission Sketch

Conexus is Python/FastAPI, so do not import .NET packages directly.

Instead, create a tiny Python emitter that sends the same envelope shape to Athanor or an event ingestion endpoint.

```python
async def emit_llm_response_completed(request_log):
    envelope = {
        "eventId": f"evt_{uuid.uuid4().hex}",
        "eventType": "llm.response.completed",
        "source": "conexus://gateway",
        "occurredAt": datetime.utcnow().isoformat() + "Z",
        "traceId": request_log.request_id,
        "protocol": "conexus",
        "schemaVersion": "1.0",
        "payload": {
            "projectId": request_log.project_id,
            "provider": request_log.provider,
            "model": request_log.model,
            "latencyMs": request_log.latency_ms,
            "inputTokens": request_log.input_tokens,
            "outputTokens": request_log.output_tokens,
            "costUsd": request_log.cost_usd,
            "status": request_log.status,
            "errorCode": request_log.error_code,
        }
    }
    await httpx.post(EVENT_INGESTION_URL, json=envelope, headers={"X-Internal-Api-Key": INTERNAL_KEY})
```

Conexus should not emit prompt or response bodies by default. It should emit operational metadata, model/provider usage, errors, and trace IDs.
