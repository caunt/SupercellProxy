using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Writers;

/// <summary>Writes a presence flag followed by the value when present.</summary>
internal sealed class OptionalEncoder<TValue>(IEncoder<TValue> value) : IEncoder<TValue?> where TValue : class
{
    public void Write(ProtocolEncoder encoder, TValue? item)
    {
        encoder.WriteBoolean(item is not null);

        if (item is not null)
            value.Write(encoder, item);
    }
}
