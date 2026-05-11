using Microsoft.Extensions.Hosting;
using Ontogony.Security;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class OntogonyAuthenticationOptionsValidatorTests
{
    [Fact]
    public void Validate_Fails_When_Disabled_Mode_Used_In_Production_Without_Override()
    {
        var validator = new OntogonyAuthenticationOptionsValidator(new StubHostEnvironment("Production"));
        var options = new OntogonyAuthenticationOptions
        {
            Mode = OntogonyAuthenticationMode.Disabled,
            AllowDisabledOutsideDevelopment = false
        };

        var result = validator.Validate(null, options);

        Assert.True(result.Failed);
    }

    [Fact]
    public void Validate_Fails_For_Unvalidated_Jwt_In_Production_Without_Override()
    {
        var validator = new OntogonyAuthenticationOptionsValidator(new StubHostEnvironment("Production"));
        var options = new OntogonyAuthenticationOptions
        {
            Mode = OntogonyAuthenticationMode.Jwt,
            JwtAuthority = null,
            JwtAcceptUnvalidatedBearerTokens = true,
            JwtAllowUnvalidatedTokensOutsideDevelopment = false
        };

        var result = validator.Validate(null, options);

        Assert.True(result.Failed);
    }

    [Fact]
    public void Validate_Succeeds_For_Header_Mode_In_Production_When_Header_Is_Configured()
    {
        var validator = new OntogonyAuthenticationOptionsValidator(new StubHostEnvironment("Production"));
        var options = new OntogonyAuthenticationOptions
        {
            Mode = OntogonyAuthenticationMode.Header,
            HeaderActorIdHeaderName = "X-Ontogony-Actor-Id"
        };

        var result = validator.Validate(null, options);

        Assert.True(result.Succeeded);
    }

    private sealed class StubHostEnvironment(string name) : IHostEnvironment
    {
        public string EnvironmentName { get; set; } = name;
        public string ApplicationName { get; set; } = "Ontogony.Tests";
        public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
        public Microsoft.Extensions.FileProviders.IFileProvider ContentRootFileProvider { get; set; } =
            new Microsoft.Extensions.FileProviders.NullFileProvider();
    }
}
