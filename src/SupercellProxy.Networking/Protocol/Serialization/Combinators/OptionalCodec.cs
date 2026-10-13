using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>A reference value preceded by a presence flag.</summary>
internal sealed class OptionalCodec<TValue>(ICodec<TValue> codec) : ICodec<TValue?> where TValue : class
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

        if (value is not null)
            codec.Write(encoder, value);
    }
}
