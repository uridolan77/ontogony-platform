# ErrorMiddlewareExample

This is a documentation-only example for Ontogony.Errors middleware adoption.

Status: Documentation-only (not compiled by Ontogony.Platform.sln).

## Goal

Show middleware ordering and error mapping composition.

## Sample

```csharp
builder.Services.AddOntogonyErrors(options =>
{
    options.Map<ValidationException>(HttpStatusCode.BadRequest, "ValidationFailed", "The request is invalid.");
});

var app = builder.Build();
app.UseOntogonyRequestTracing();
app.UseOntogonyExceptionHandling();
```

## Do Not Do This

- Do not expose internal exception messages unless explicitly intended.
- Do not register service-specific exception types inside Ontogony.Platform packages.
