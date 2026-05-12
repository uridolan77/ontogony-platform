using Microsoft.AspNetCore.Http.Json;
using Ontogony.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOntogonyServiceDefaults(builder.Configuration, options =>
{
    options.ServiceName = "minimal-hosting-example";
    options.ServiceVersion = "0.1.0";
    options.UseServiceIdentityBodyHashPreload = false;
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.WriteIndented = true;
});

var app = builder.Build();

app.UseOntogonyServiceDefaults();
app.MapOntogonyHealthEndpoints();

app.MapGet("/", (HttpContext context) =>
{
    return Results.Json(new
    {
        message = "Minimal API with Ontogony hosting defaults.",
        traceId = context.Items.TryGetValue("Ontogony.TraceId", out var value) ? value?.ToString() : null
    });
});

app.Run();
