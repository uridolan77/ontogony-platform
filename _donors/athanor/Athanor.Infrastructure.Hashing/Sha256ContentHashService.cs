using System.Security.Cryptography;
using System.Text;
using Athanor.Application.Abstractions;
using Athanor.Domain.Hashing;

namespace Athanor.Infrastructure.Hashing;

public sealed class Sha256ContentHashService : IContentHashService
{
    public string ComputeSha256(object payload)
    {
        var json = CanonicalJson.Serialize(payload);
        return ComputeSha256(json);
    }

    public string ComputeSha256(string payload)
    {
        var bytes = Encoding.UTF8.GetBytes(payload);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    public string ComputeSha256(CanonicalHashInput input)
    {
        var text = input.Kind + '\u001e' + input.CanonicalJsonPayload;
        return ComputeSha256(text);
    }

    public string CanonicalizeJson(string json) => CanonicalJson.CanonicalizeJsonString(json);
}
