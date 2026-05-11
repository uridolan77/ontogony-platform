using Athanor.Application.Abstractions;

namespace Athanor.Infrastructure.Identity;

public sealed class GuidIdGenerator : IIdGenerator
{
    public Guid NewId() => Guid.NewGuid();
}
