using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>Carries another player's Town, its owner, and the visiting player.</summary>
public sealed record OtherTrainStationDataMessage : OtherHomeDataMessage, ICodecProvider<OtherTrainStationDataMessage>
{
    private OtherTrainStationDataMessage(OtherHomeDataMessage data) : base(data)
    {
    }

    /// <inheritdoc />
    public static new ICodec<OtherTrainStationDataMessage> Codec { get; } = Codecs.Map(OtherHomeDataMessage.Codec, static message => new OtherTrainStationDataMessage(message), static message => message);
}
