using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>A payload native rejects when bytes remain after its last field.</summary>
internal sealed class CompleteCodec<TValue>(ICodec<TValue> codec, string trailingDataMessage) : ICodec<TValue>
{
    public TValue Read(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        TValue value = codec.Read(stream);

        return stream.Position != stream.Length ? throw new InvalidDataException(trailingDataMessage) : value;
    }

    public void Write(ProtocolEncoder encoder, TValue value)
    {
        codec.Write(encoder, value);
    }
}
