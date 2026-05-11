using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Ontogony.Security;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public class OntogonySecurityPr6Tests
{
    public class ActorTypesTests
    {
        [Fact]
        public void ActorTypeConstants_AreCorrect()
        {
            Assert.Equal("human", OntogonyActorTypes.Human);
            Assert.Equal("service", OntogonyActorTypes.Service);
            Assert.Equal("agent", OntogonyActorTypes.Agent);
        }
    }

    /// <summary>
    /// OntogonyRoleNames tests - verify generic role constants and validation.
    /// </summary>
    public class RoleNamesTests
    {
        [Fact]
        public void RoleConstants_AreCorrect()
        {
            Assert.Equal("human-operator", OntogonyRoleNames.HumanOperator);
            Assert.Equal("service", OntogonyRoleNames.Service);
            Assert.Equal("agent", OntogonyRoleNames.Agent);
            Assert.Equal("admin", OntogonyRoleNames.Admin);
        }

        [Fact]
        public void AllRoles_ContainsAllGenericRoles()
        {
            Assert.Contains(OntogonyRoleNames.HumanOperator, OntogonyRoleNames.AllRoles);
            Assert.Contains(OntogonyRoleNames.Service, OntogonyRoleNames.AllRoles);
            Assert.Contains(OntogonyRoleNames.Agent, OntogonyRoleNames.AllRoles);
            Assert.Contains(OntogonyRoleNames.Admin, OntogonyRoleNames.AllRoles);
            Assert.Equal(4, OntogonyRoleNames.AllRoles.Length);
        }

        [Fact]
        public void AreAllValid_WithValidRoles_ReturnsTrue()
        {
            var valid = OntogonyRoleNames.AreAllValid(
                OntogonyRoleNames.HumanOperator,
                OntogonyRoleNames.Admin);

            Assert.True(valid);
        }

        [Fact]
        public void AreAllValid_WithInvalidRole_ReturnsFalse()
        {
            var invalid = OntogonyRoleNames.AreAllValid(
                OntogonyRoleNames.HumanOperator,
                "GovernanceApprover"); // Not a generic role

            Assert.False(invalid);
        }

        [Fact]
        public void AreAllValid_WithEmptyArray_ReturnsFalse()
        {
            Assert.False(OntogonyRoleNames.AreAllValid());
        }

        [Fact]
        public void AreAllValid_IsCaseInsensitive()
        {
            var valid = OntogonyRoleNames.AreAllValid(
                "HUMAN-OPERATOR",
                "Service");

            Assert.True(valid);
        }
    }

    /// <summary>
    /// OntogonySecurityHeaders tests - verify header constants and utilities.
    /// </summary>
    public class SecurityHeadersTests
    {
        [Fact]
        public void HeaderConstants_AreCorrect()
        {
            Assert.Equal("Authorization", OntogonySecurityHeaders.Authorization);
            Assert.Equal("X-Ontogony-Actor-Id", OntogonySecurityHeaders.ActorId);
            Assert.Equal("X-Ontogony-Roles", OntogonySecurityHeaders.Roles);
            Assert.Equal("X-Ontogony-Actor-Type", OntogonySecurityHeaders.ActorType);
            Assert.Equal("Bearer ", OntogonySecurityHeaders.BearerPrefix);
        }

        [Fact]
        public void ExtractBearerToken_WithValidToken_ReturnsToken()
        {
            var auth = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";

            var token = OntogonySecurityHeaders.ExtractBearerToken(auth);

            Assert.Equal("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...", token);
        }

        [Fact]
        public void ExtractBearerToken_WithMissingBearer_ReturnsNull()
        {
            var token = OntogonySecurityHeaders.ExtractBearerToken("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...");

            Assert.Null(token);
        }

        [Fact]
        public void ExtractBearerToken_WithEmptyString_ReturnsNull()
        {
            Assert.Null(OntogonySecurityHeaders.ExtractBearerToken(""));
        }

        [Fact]
        public void ExtractBearerToken_WithNullString_ReturnsNull()
        {
            Assert.Null(OntogonySecurityHeaders.ExtractBearerToken(null!));
        }

        [Fact]
        public void ExtractBearerToken_WithBearerOnly_ReturnsNull()
        {
            Assert.Null(OntogonySecurityHeaders.ExtractBearerToken("Bearer "));
        }

        [Fact]
        public void ExtractBearerToken_CaseInsensitiveBearer()
        {
            var token = OntogonySecurityHeaders.ExtractBearerToken("bearer token123");

            Assert.Equal("token123", token);
        }

        [Fact]
        public void ExtractBearerToken_TrimsWhitespace()
        {
            var token = OntogonySecurityHeaders.ExtractBearerToken("Bearer   token456  ");

            Assert.Equal("token456", token);
        }
    }

    /// <summary>
    /// ClaimsCurrentActorAccessor tests - verify JWT claims extraction.
    /// </summary>
    public class ClaimsCurrentActorAccessorTests
    {
        private readonly HttpContextAccessor _contextAccessor;
        private readonly ClaimsCurrentActorAccessor _accessor;

        public ClaimsCurrentActorAccessorTests()
        {
            _contextAccessor = new HttpContextAccessor();
            _accessor = new ClaimsCurrentActorAccessor(_contextAccessor);
        }

        [Fact]
        public void Current_WithNoUser_ReturnsNull()
        {
            _contextAccessor.HttpContext = new DefaultHttpContext();

            Assert.Null(_accessor.Current);
        }

        [Fact]
        public void Current_WithUnauthenticatedUser_ReturnsNull()
        {
            var context = new DefaultHttpContext();
            context.User = new ClaimsPrincipal();

            _contextAccessor.HttpContext = context;

            Assert.Null(_accessor.Current);
        }

        [Fact]
        public void Current_WithAuthenticatedUser_ReturnsActor()
        {
            var claims = new[]
            {
                new Claim("sub", "user-123"),
                new Claim(ClaimTypes.Role, OntogonyRoleNames.HumanOperator)
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);

            var context = new DefaultHttpContext { User = principal };
            _contextAccessor.HttpContext = context;

            var actor = _accessor.Current;

            Assert.NotNull(actor);
            Assert.Equal("user-123", actor.ActorId);
            Assert.Equal(OntogonyActorTypes.Service, actor.ActorType); // Default actor type
            Assert.Contains(OntogonyRoleNames.HumanOperator, actor.Roles);
        }

        [Fact]
        public void Current_WithActorTypeClaim_UsesActorType()
        {
            var claims = new[]
            {
                new Claim("sub", "agent-1"),
                new Claim("actor_type", OntogonyRoleNames.Agent),
                new Claim(ClaimTypes.Role, OntogonyRoleNames.Agent)
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);

            var context = new DefaultHttpContext { User = principal };
            _contextAccessor.HttpContext = context;

            var actor = _accessor.Current;

            Assert.NotNull(actor);
            Assert.Equal(OntogonyActorTypes.Agent, actor.ActorType);
        }

        [Fact]
        public void Current_WithMultipleRoles_ExtractivesAll()
        {
            var claims = new[]
            {
                new Claim("sub", "user-1"),
                new Claim(ClaimTypes.Role, OntogonyRoleNames.HumanOperator),
                new Claim(ClaimTypes.Role, OntogonyRoleNames.Admin)
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);

            var context = new DefaultHttpContext { User = principal };
            _contextAccessor.HttpContext = context;

            var actor = _accessor.Current;

            Assert.NotNull(actor);
            Assert.Equal(2, actor.Roles.Length);
            Assert.Contains(OntogonyRoleNames.HumanOperator, actor.Roles);
            Assert.Contains(OntogonyRoleNames.Admin, actor.Roles);
        }

        [Fact]
        public void Current_WithTenantIdClaim_ExtractsTenantId()
        {
            var claims = new[]
            {
                new Claim("sub", "user-1"),
                new Claim("tenant_id", "tenant-123"),
                new Claim(ClaimTypes.Role, OntogonyRoleNames.Service)
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);

            var context = new DefaultHttpContext { User = principal };
            _contextAccessor.HttpContext = context;

            var actor = _accessor.Current;

            Assert.NotNull(actor);
            Assert.Equal("tenant-123", actor.TenantId);
        }

        [Fact]
        public void Current_WithStrictRoleValidation_FiltersToGenericRoles()
        {
            var options = new ClaimsCurrentActorAccessorOptions { StrictRoleValidation = true };
            var accessor = new ClaimsCurrentActorAccessor(_contextAccessor, options);

            var claims = new[]
            {
                new Claim("sub", "user-1"),
                new Claim(ClaimTypes.Role, OntogonyRoleNames.HumanOperator),
                new Claim(ClaimTypes.Role, "custom-role") // Not in AllRoles
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);

            var context = new DefaultHttpContext { User = principal };
            _contextAccessor.HttpContext = context;

            var actor = accessor.Current;

            Assert.NotNull(actor);
            Assert.Single(actor.Roles); // Only the generic role
            Assert.Equal(OntogonyRoleNames.HumanOperator, actor.Roles[0]);
        }

        [Fact]
        public void Current_WithoutStrictRoleValidation_AllowsCustomRoles()
        {
            var options = new ClaimsCurrentActorAccessorOptions { StrictRoleValidation = false };
            var accessor = new ClaimsCurrentActorAccessor(_contextAccessor, options);

            var claims = new[]
            {
                new Claim("sub", "user-1"),
                new Claim(ClaimTypes.Role, "custom-role")
            };
            var identity = new ClaimsIdentity(claims, "test");
            var principal = new ClaimsPrincipal(identity);

            var context = new DefaultHttpContext { User = principal };
            _contextAccessor.HttpContext = context;

            var actor = accessor.Current;

            Assert.NotNull(actor);
            Assert.Single(actor.Roles);
            Assert.Equal("custom-role", actor.Roles[0]);
        }
    }

    /// <summary>
    /// ServiceIdentityCurrentActorAccessor tests - verify service-to-service auth.
    /// </summary>
    public class ServiceIdentityCurrentActorAccessorTests
    {
        private readonly HttpContextAccessor _contextAccessor;

        public ServiceIdentityCurrentActorAccessorTests()
        {
            _contextAccessor = new HttpContextAccessor();
        }

        [Fact]
        public void Current_WithNoServiceIdHeader_ReturnsNull()
        {
            _contextAccessor.HttpContext = new DefaultHttpContext();

            var accessor = new ServiceIdentityCurrentActorAccessor(_contextAccessor);
            Assert.Null(accessor.Current);
        }

        [Fact]
        public void Current_WithServiceId_ReturnsServiceActor()
        {
            var context = new DefaultHttpContext();
            context.Request.Headers[OntogonyServiceIdentityHeaders.ServiceId] = "service-agentor";
            context.Request.Headers["X-Ontogony-Tenant-Id"] = "tenant-123";
            context.Request.Headers["X-Ontogony-Workspace-Id"] = "workspace-456";
            context.Request.Headers["X-Ontogony-Project-Id"] = "project-789";

            _contextAccessor.HttpContext = context;

            var accessor = new ServiceIdentityCurrentActorAccessor(_contextAccessor);
            var actor = accessor.Current;

            Assert.NotNull(actor);
            Assert.Equal("service-agentor", actor.ActorId);
            Assert.Equal(OntogonyActorTypes.Service, actor.ActorType);
            Assert.Contains(OntogonyRoleNames.Service, actor.Roles);
            Assert.Equal("tenant-123", actor.TenantId);
            Assert.Equal("workspace-456", actor.WorkspaceId);
            Assert.Equal("project-789", actor.ProjectId);
        }

        [Fact]
        public void Current_WithSignatureVerification_FailsWithoutSignature()
        {
            var options = new ServiceIdentityOptions { RequireSignatureVerification = true };
            var context = new DefaultHttpContext();
            context.Request.Headers[options.ServiceIdHeaderName] = "service-1";

            _contextAccessor.HttpContext = context;

            var accessor = new ServiceIdentityCurrentActorAccessor(_contextAccessor, options);
            Assert.Null(accessor.Current);
        }

        [Fact]
        public void Current_WithSignatureVerification_SucceedsWithValidSignature()
        {
            var options = new ServiceIdentityOptions
            {
                RequireSignatureVerification = true,
                ServiceSecrets = new Dictionary<string, string> { { "service-1", "secret-1" } }
            };
            var context = new DefaultHttpContext();
            context.Request.Headers[OntogonySecurityHeaders.ActorId] = "service-1";
            context.Request.Headers[options.ServiceIdHeaderName] = "service-1";
            context.Request.Headers[options.SignatureHeaderName] = "secret-1";

            _contextAccessor.HttpContext = context;

            var accessor = new ServiceIdentityCurrentActorAccessor(_contextAccessor, options);
            var actor = accessor.Current;

            Assert.NotNull(actor);
            Assert.Equal("service-1", actor.ActorId);
        }
    }

    public class HeaderCurrentActorAccessorTests
    {
        [Fact]
        public void Current_UsesSecurityHeaderConstants_ForRolesAndActorType()
        {
            var contextAccessor = new HttpContextAccessor();
            var context = new DefaultHttpContext();
            context.Request.Headers[OntogonySecurityHeaders.ActorId] = "actor-123";
            context.Request.Headers[OntogonySecurityHeaders.ActorType] = OntogonyActorTypes.Human;
            context.Request.Headers[OntogonySecurityHeaders.Roles] = "human-operator,admin";

            contextAccessor.HttpContext = context;

            var accessor = new HeaderCurrentActorAccessor(contextAccessor);
            var actor = accessor.Current;

            Assert.NotNull(actor);
            Assert.Equal("actor-123", actor.ActorId);
            Assert.Equal(OntogonyActorTypes.Human, actor.ActorType);
            Assert.Contains(OntogonyRoleNames.HumanOperator, actor.Roles);
            Assert.Contains(OntogonyRoleNames.Admin, actor.Roles);
        }

        [Fact]
        public void Current_WithoutActorTypeHeader_DefaultsToServiceActorType()
        {
            var contextAccessor = new HttpContextAccessor();
            var context = new DefaultHttpContext();
            context.Request.Headers[OntogonySecurityHeaders.ActorId] = "svc-1";
            context.Request.Headers[OntogonySecurityHeaders.Roles] = OntogonyRoleNames.Service;

            contextAccessor.HttpContext = context;

            var accessor = new HeaderCurrentActorAccessor(contextAccessor);
            var actor = accessor.Current;

            Assert.NotNull(actor);
            Assert.Equal(OntogonyActorTypes.Service, actor.ActorType);
        }
    }

    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddOntogonyClaimsActorContext_RegistersClaimsAccessor()
        {
            var services = new ServiceCollection();
            services.AddOntogonyClaimsActorContext(options => options.StrictRoleValidation = false);

            using var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();

            var accessor = scope.ServiceProvider.GetRequiredService<ICurrentActorAccessor>();
            Assert.IsType<ClaimsCurrentActorAccessor>(accessor);
        }

        [Fact]
        public void AddOntogonyServiceIdentityActorContext_RegistersServiceIdentityAccessor()
        {
            var services = new ServiceCollection();
            services.AddOntogonyServiceIdentityActorContext(options => options.RequireSignatureVerification = true);

            using var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();

            var accessor = scope.ServiceProvider.GetRequiredService<ICurrentActorAccessor>();
            Assert.IsType<ServiceIdentityCurrentActorAccessor>(accessor);
        }
    }

    /// <summary>
    /// OntogonyAuthenticationOptions validation tests.
    /// </summary>
    public class AuthenticationOptionsTests
    {
        [Fact]
        public void DefaultOptions_AreHeader()
        {
            var options = new OntogonyAuthenticationOptions();

            Assert.Equal(OntogonyAuthenticationMode.Header, options.Mode);
        }

        [Fact]
        public void Validate_DisabledMode_ThrowsInProduction()
        {
            var options = new OntogonyAuthenticationOptions
            {
                Mode = OntogonyAuthenticationMode.Disabled,
                AllowDisabledOutsideDevelopment = false
            };

            var ex = Assert.Throws<InvalidOperationException>(() => options.Validate("Production"));
            Assert.Contains("Disabled", ex.Message);
        }

        [Fact]
        public void Validate_DisabledMode_AllowedWithFlag()
        {
            var options = new OntogonyAuthenticationOptions
            {
                Mode = OntogonyAuthenticationMode.Disabled,
                AllowDisabledOutsideDevelopment = true
            };

            // Should not throw
            options.Validate("Production");
        }

        [Fact]
        public void Validate_HeaderMode_RequiresActorIdHeaderName()
        {
            var options = new OntogonyAuthenticationOptions
            {
                Mode = OntogonyAuthenticationMode.Header,
                HeaderActorIdHeaderName = ""
            };

            var ex = Assert.Throws<InvalidOperationException>(() => options.Validate("Development"));
            Assert.Contains("HeaderActorIdHeaderName", ex.Message);
        }

        [Fact]
        public void Validate_JwtMode_RequiresAuthorityOrUnvalidated()
        {
            var options = new OntogonyAuthenticationOptions
            {
                Mode = OntogonyAuthenticationMode.Jwt,
                JwtAuthority = null,
                JwtAcceptUnvalidatedBearerTokens = false
            };

            var ex = Assert.Throws<InvalidOperationException>(() => options.Validate("Development"));
            Assert.Contains("JwtAuthority", ex.Message);
        }

        [Fact]
        public void Validate_JwtUnvalidated_BlockedInProduction()
        {
            var options = new OntogonyAuthenticationOptions
            {
                Mode = OntogonyAuthenticationMode.Jwt,
                JwtAuthority = null,
                JwtAcceptUnvalidatedBearerTokens = true,
                JwtAllowUnvalidatedTokensOutsideDevelopment = false
            };

            var ex = Assert.Throws<InvalidOperationException>(() => options.Validate("Production"));
            Assert.Contains("unvalidated", ex.Message.ToLowerInvariant());
        }

        [Fact]
        public void Validate_JwtUnvalidated_AllowedWithFlag()
        {
            var options = new OntogonyAuthenticationOptions
            {
                Mode = OntogonyAuthenticationMode.Jwt,
                JwtAuthority = null,
                JwtAcceptUnvalidatedBearerTokens = true,
                JwtAllowUnvalidatedTokensOutsideDevelopment = true
            };

            // Should not throw
            options.Validate("Production");
        }

        [Fact]
        public void Validate_JwtMode_SucceedsWithAuthority()
        {
            var options = new OntogonyAuthenticationOptions
            {
                Mode = OntogonyAuthenticationMode.Jwt,
                JwtAuthority = "https://login.microsoftonline.com/tenant/v2.0",
                JwtAcceptUnvalidatedBearerTokens = false
            };

            // Should not throw
            options.Validate("Production");
        }
    }
}
