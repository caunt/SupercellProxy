using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.Screens;

/// <summary>Reports that the client opened or closed a screen element, such as a visited farm's roadside shop.</summary>
/// <param name="Origin">Where the event came from, such as <c>Other_Farm</c>, or an empty string when there is no origin; always the first field, as a length-prefixed UTF-8 string.</param>
/// <param name="Action">What happened to the element, <c>open</c> or <c>close</c>; the second field from release 1.73.81 on and the last one before it.</param>
/// <param name="Screen">The screen the element belongs to, such as <c>Other_Farm</c> while visiting another farm.</param>
/// <param name="Value">A var-int sent with the event whose meaning is not identified; the farm-visit and roadside shop events carry 0.</param>
/// <param name="Element">The element that was opened or closed, such as <c>Farm</c> or <c>roadsideshop</c>; the last field from release 1.73.81 on.</param>
public sealed record ClientScreenEventMessage(string Origin, string Action, string Screen, int Value, string Element)
    : CodecMessage<ClientScreenEventMessage>, ICodecProvider<ClientScreenEventMessage>
{
    /// <inheritdoc />
    public static ICodec<ClientScreenEventMessage> Codec { get; } = Codecs.Versioned(Release17381.Version,
        Codecs.Composite(Codecs.Utf8String,
            static message => message.Origin,
            Codecs.Utf8String,
            static message => message.Action,
            Codecs.Utf8String,
            static message => message.Screen,
            Codecs.VarInt,
            static message => message.Value,
            Codecs.Utf8String,
            static message => message.Element,
            static (origin, action, screen, value, element) => new ClientScreenEventMessage(origin, action, screen, value, element)),
        Codecs.Composite(Codecs.Utf8String,
            static message => message.Origin,
            Codecs.VarInt,
            static message => message.Value,
            Codecs.Utf8String,
            static message => message.Screen,
            Codecs.Utf8String,
            static message => message.Element,
            Codecs.Utf8String,
            static message => message.Action,
            static (origin, value, screen, element, action) => new ClientScreenEventMessage(origin, action, screen, value, element)));
}
