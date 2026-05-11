using System.Net;

namespace Ontogony.Testing;

public sealed class StubHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>> _steps = new();

    public int CallCount { get; private set; }

    public void EnqueueResponse(HttpResponseMessage response)
    {
        ArgumentNullException.ThrowIfNull(response);
        _steps.Enqueue((_, _) => Task.FromResult(response));
    }

    public void EnqueueException(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        _steps.Enqueue((_, _) => Task.FromException<HttpResponseMessage>(exception));
    }

    public void EnqueueStep(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> step)
    {
        ArgumentNullException.ThrowIfNull(step);
        _steps.Enqueue(step);
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        CallCount++;
        if (_steps.Count == 0)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotImplemented)
            {
                RequestMessage = request
            });
        }

        var step = _steps.Dequeue();
        return step(request, cancellationToken);
    }
}