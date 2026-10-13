using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes.Requests;

/// <summary>Requests Greg's town (native game mode 8) using a native language-table index.</summary>
/// <param name="LanguageIndex">The language index.</param>
public sealed record RequestGregTownMessage(int LanguageIndex) : CodecMessage<RequestGregTownMessage>, ICodecProvider<RequestGregTownMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestGregTownMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.LanguageIndex, static (languageIndex) => new RequestGregTownMessage(languageIndex));
}
