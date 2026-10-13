using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>A tail field native reads only while payload bytes remain.</summary>
internal sealed class TrailingCodec<TValue>(ICodec<TValue> codec, TValue absent, Func<TValue, bool> isPresent) : ICodec<TValue>
{
    public TValue Read(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return stream.Position < stream.Length ? codec.Read(stream) : absent;
    }

    public void Write(ProtocolEncoder encoder, TValue value)
    {
        if (isPresent(value))
            codec.Write(encoder, value);
    }
}
