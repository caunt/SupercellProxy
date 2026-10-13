using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Writers;

/// <summary>Writes the value when present and nothing at all when it is absent.</summary>
internal sealed class PresentEncoder<TValue>(IEncoder<TValue> value) : IEncoder<TValue?> where TValue : class
{
    public void Write(ProtocolEncoder encoder, TValue? item)
    {
        if (item is not null)
            value.Write(encoder, item);
    }
}
