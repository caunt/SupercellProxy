using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Carries two town passenger values; the client ignores it.</summary>
/// <param name="Value0">First number; its meaning is not identified.</param>
/// <param name="Value1">Second number; its meaning is not identified.</param>
public sealed record TownPassengerStateMessage(int Value0, int Value1) : CodecMessage<TownPassengerStateMessage>, ICodecProvider<TownPassengerStateMessage>
{
    /// <inheritdoc />
    public static ICodec<TownPassengerStateMessage> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static message => message.Value0,
        Codecs.VarInt,
        static message => message.Value1,
        static (value0, value1) => new TownPassengerStateMessage(value0, value1)
    );
}
