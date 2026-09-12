using System.Diagnostics.CodeAnalysis;

using SupercellProxy.Networking.Protocol.CommandEncoding;

namespace SupercellProxy.Capture;

internal sealed class CaptureCommandDataResolver : ICommandDataResolver
{
    private ICommandDataResolver? _resolver;

    public bool TryResolveString(int globalIdentifier, string fieldName, [NotNullWhen(true)] out string? value)
    {
        ICommandDataResolver resolver = Volatile.Read(ref _resolver)
            ?? throw new InvalidOperationException(message: "The capture command data resolver has not been initialized.");

        return resolver.TryResolveString(globalIdentifier, fieldName, out value);
    }

    internal void Initialize(ICommandDataResolver resolver)
    {
        ArgumentNullException.ThrowIfNull(resolver);

        if (Interlocked.CompareExchange(ref _resolver, resolver, comparand: null) is not null)
            throw new InvalidOperationException(message: "The capture command data resolver is already initialized.");
    }
}
