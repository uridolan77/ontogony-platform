using System.Net;

namespace Ontogony.Testing;

public sealed class RecordingHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _responder;
    private readonly List<HttpRequestMessage> _requests = [];

    public RecordingHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>? responder = null)
    {
        _responder = responder ?? ((request, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            RequestMessage = request
        }));
    }

    public IReadOnlyList<HttpRequestMessage> Requests => _requests;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _requests.Add(request);
        return await _responder(request, cancellationToken).ConfigureAwait(false);
    }
}