// Example migration fragment for Agentor.Api Program.cs
using Ontogony.Observability;
using Ontogony.Errors;
using Ontogony.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOntogonyObservability(options =>
{
    options.ServiceName = "Agentor.Api";
    options.ServiceVersion = "1.0.0-rc.1";
});

builder.Services.AddOntogonyErrors(options =>
{
    // Map Agentor exceptions in Agentor, not in the shared package.
});

builder.Services.AddOntogonyIntegrationHttpClient(
    "conexus",
    sp => new HttpIntegrationOptions
    {
        Mode = IntegrationAdapterMode.Http,
        BaseUrl = builder.Configuration["Agentor:Integrations:Conexus:Http:BaseUrl"],
        TimeoutSeconds = 30
    });

var app = builder.Build();

app.UseOntogonyExceptionHandling();
app.UseOntogonyRequestTracing();

// app.UseAuthentication();
// app.UseAuthorization();
// app.MapGroup("/api/v1").RequireAuthorization();
// app.Run();
