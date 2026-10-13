using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.Screens;

/// <summary>Reports that the client opened or closed a screen element, such as a visited farm's roadside shop.</summary>
public sealed record ClientScreenEventMessage(string Origin, string Action, string Screen, int Value, string Element)
    : CodecMessage<ClientScreenEventMessage>, ICodecProvider<ClientScreenEventMessage>
{
    /// <inheritdoc />
    public static ICodec<ClientScreenEventMessage> Codec { get; } = Codecs.Versioned(
        Release17381.Version,
        Codecs.Composite(
            Codecs.String,
            static message => message.Origin,
            Codecs.String,
            static message => message.Action,
            Codecs.String,
            static message => message.Screen,
            Codecs.VarInt,
            static message => message.Value,
            Codecs.String,
            static message => message.Element,
            static (origin, action, screen, value, element) => new ClientScreenEventMessage(origin, action, screen, value, element)
        ),
        Codecs.Composite(
            Codecs.String,
            static message => message.Origin,
            Codecs.VarInt,
            static message => message.Value,
            Codecs.String,
            static message => message.Screen,
            Codecs.String,
            static message => message.Element,
            Codecs.String,
            static message => message.Action,
            static (origin, value, screen, element, action) => new ClientScreenEventMessage(origin, action, screen, value, element)
        )
    );
}
