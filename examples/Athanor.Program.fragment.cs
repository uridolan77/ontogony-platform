// Example migration fragment for Athanor.Api Program.cs
using Ontogony.Observability;
using Ontogony.Errors;
using Ontogony.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOntogonyObservability(options =>
{
    options.ServiceName = "Athanor.Api";
    options.ServiceVersion = "0.1.0";
});

builder.Services.AddOntogonyErrors(options =>
{
    // Keep exception types in Athanor; map them here.
    // options.Map<EntityNotFoundException>(HttpStatusCode.NotFound, "NotFound");
    // options.Map<AuthorityDeniedException>(HttpStatusCode.Forbidden, "AuthorityDenied");
});

var app = builder.Build();

app.UseOntogonyRequestTracing();
app.UseOntogonyExceptionHandling();

// app.MapControllers();
// app.Run();
