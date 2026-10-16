using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes.Requests;

/// <summary>Requests Greg's farm using a native language-table index.</summary>
public sealed record RequestGregFarmMessage(int LanguageIndex = 0) : CodecMessage<RequestGregFarmMessage>, ICodecProvider<RequestGregFarmMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestGregFarmMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(Codecs.VarInt, static message => message.LanguageIndex, static languageIndex => new RequestGregFarmMessage(languageIndex)),
        trailingDataMessage: "The Greg farm request has trailing data."
    );
}
