using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.MessageEncoding;

/// <summary>A message whose payload is read and written by its declared codec.</summary>
/// <typeparam name="TSelf">The message record.</typeparam>
public abstract record CodecMessage<TSelf> : IMessage where TSelf : CodecMessage<TSelf>, ICodecProvider<TSelf>
{
    /// <inheritdoc />
    public void Encode(MessageStream stream)
    {
        TSelf.Codec.Write(stream, (TSelf)this);
    }
}
