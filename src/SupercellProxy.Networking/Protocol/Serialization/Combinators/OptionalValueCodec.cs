using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>A value type preceded by a presence flag.</summary>
internal sealed class OptionalValueCodec<TValue>(ICodec<TValue> codec) : ICodec<TValue?> where TValue : struct
{
    public TValue? Read(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return stream.ReadBoolean() ? codec.Read(stream) : null;
    }

    public void Write(ProtocolEncoder encoder, TValue? value)
    {
        ArgumentNullException.ThrowIfNull(encoder);
        encoder.WriteBoolean(value is not null);

        if (value is { } present)
            codec.Write(encoder, present);
    }
}
