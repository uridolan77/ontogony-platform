using Conexus.Application.Sampling;
using Conexus.Domain.Sampling;
using Microsoft.AspNetCore.Mvc;

namespace Conexus.Api.Controllers;

[ApiController]
[Route("llm/v0")]
public sealed class SamplingPolicyController : ControllerBase
{
    private readonly ISamplingProfileRegistry _registry;
    private readonly ISamplingPolicyResolver _resolver;

    public SamplingPolicyController(
        ISamplingProfileRegistry registry,
        ISamplingPolicyResolver resolver)
    {
        _registry = registry;
        _resolver = resolver;
    }

    [HttpGet("sampling-profiles")]
    public ActionResult<IReadOnlyCollection<SamplingProfile>> ListProfiles()
        => Ok(_registry.List());

    [HttpGet("sampling-profiles/{profileId}")]
    public ActionResult<SamplingProfile> GetProfile(string profileId)
    {
        var profile = _registry.TryGet(profileId);
        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpPost("sampling-policy/resolve")]
    public async Task<ActionResult<SamplingPolicyResolution>> Resolve(
        [FromBody] SamplingPolicyResolveRequest request,
        CancellationToken cancellationToken)
    {
        var resolution = await _resolver.ResolveAsync(request, cancellationToken);
        return resolution.Decision == SamplingPolicyDecision.Denied
            ? BadRequest(resolution)
            : Ok(resolution);
    }

    [HttpPost("sampling-policy/validate")]
    public async Task<ActionResult<SamplingPolicyResolution>> Validate(
        [FromBody] SamplingPolicyResolveRequest request,
        CancellationToken cancellationToken)
    {
        // In v0 validation and resolution share the same pure path; no provider call is executed.
        var resolution = await _resolver.ResolveAsync(request, cancellationToken);
        return Ok(resolution);
    }
}
