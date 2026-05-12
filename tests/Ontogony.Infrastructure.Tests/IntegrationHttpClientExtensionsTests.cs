using Microsoft.Extensions.DependencyInjection;
using Ontogony.Http;
using Ontogony.Primitives;
using Xunit;

namespace Ontogony.Infrastructure.Tests;

public sealed class IntegrationHttpClientExtensionsTests
{
    [Fact]
    public async Task AddOntogonyIntegrationHttpClient_Uses_Registered_Retry_Classifier()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IRetryClassifier, CustomClassifier>();

        services
            .AddOntogonyIntegrationHttpClient("tests", _ => new HttpIntegrationOptions { BaseUrl = "https://example.test" })
            .ConfigurePrimaryHttpMessageHandler(() => new SequenceHandler(
                new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest),
                new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IHttpClientFactory>();

        using var client = factory.CreateClient("tests");
        var response = await client.GetAsync("retry");

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public void AddOntogonyIntegrationHttpClient_Registers_Default_Retry_Classifier()
    {
        var services = new ServiceCollection();

        services.AddOntogonyIntegrationHttpClient("tests", _ => new HttpIntegrationOptions { BaseUrl = "https://example.test" });

        using var provider = services.BuildServiceProvider();

        Assert.IsType<DefaultRetryClassifier>(provider.GetRequiredService<IRetryClassifier>());
    }

    private sealed class CustomClassifier : IRetryClassifier
    {
        public RetryDecision ShouldRetry(HttpRequestMessage request, HttpResponseMessage? response, Exception? exception)
        {
            if (response?.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return RetryDecision.Retry;
            }

            return RetryDecision.DoNotRetry;
        }
    }

    private sealed class SequenceHandler : HttpMessageHandler
    {
        private readonly Queue<HttpResponseMessage> _responses;

        public SequenceHandler(params HttpResponseMessage[] responses)
        {
            _responses = new Queue<HttpResponseMessage>(responses);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_responses.Dequeue());
        }
    }
}