using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.ScalarPayloads;

/// Carries the decoded scalar value from clientbound message 20155.
public sealed record Clientbound20155Message : IMessage
{
    /// Gets the decoded signed var-length value.
    public int Value { get; init; }

    /// Decodes clientbound message 20155.
    public static Clientbound20155Message Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new Clientbound20155Message { Value = stream.ReadVarInt() };
    }

    /// Encodes clientbound message 20155.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteVarInt(Value);
    }
}
