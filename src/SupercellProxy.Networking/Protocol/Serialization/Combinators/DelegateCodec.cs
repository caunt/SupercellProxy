using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Serialization.Combinators;

/// <summary>A codec defined by a read and a write function.</summary>
internal sealed class DelegateCodec<TValue>(Func<MessageStream, TValue> read, Action<ProtocolEncoder, TValue> write) : ICodec<TValue>
{
    public TValue Read(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return read(stream);
    }

    public void Write(ProtocolEncoder encoder, TValue value)
    {
        ArgumentNullException.ThrowIfNull(encoder);
        write(encoder, value);
    }
}
