using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.ScalarPayloads;

/// Carries the three restored values from clientbound message 26385.
public sealed record Clientbound26385Message : IMessage
{
    /// Gets the first decoded bit-packed flag.
    public bool FlagA { get; init; }

    /// Gets the second decoded bit-packed flag.
    public bool FlagB { get; init; }
    /// Gets the decoded signed var-length value.
    public int Value { get; init; }

    /// Decodes clientbound message 26385.
    public static Clientbound26385Message Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new Clientbound26385Message
        {
            Value = stream.ReadVarInt(),
            FlagA = stream.ReadBoolean(),
            FlagB = stream.ReadBoolean(),
        };
    }

    /// Encodes clientbound message 26385.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteVarInt(Value);
        stream.WriteBoolean(FlagA);
        stream.WriteBoolean(FlagB);
    }
}
