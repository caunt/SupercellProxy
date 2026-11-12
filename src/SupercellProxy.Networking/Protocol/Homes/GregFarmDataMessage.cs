using SupercellProxy.Networking.Protocol.Serialization;

using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>Represents Greg's-farm snapshot carried by clientbound message 20699, loaded in native game mode 3.</summary>
/// <param name="Data">Greg's farm payload, decoded with the shared <see cref="OwnHomeDataMessage"/> layout that every home-load message uses; its home document describes Greg's farm.</param>
public sealed record GregFarmDataMessage(OwnHomeDataMessage Data) : CodecMessage<GregFarmDataMessage>, ICodecProvider<GregFarmDataMessage>
{
    /// <inheritdoc />
    public static ICodec<GregFarmDataMessage> Codec { get; } = Codecs.Composite(OwnHomeDataMessage.Codec, static message => message.Data, static data => new GregFarmDataMessage(data));
}
