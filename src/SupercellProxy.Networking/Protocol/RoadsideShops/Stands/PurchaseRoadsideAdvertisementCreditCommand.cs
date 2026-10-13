using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops.Stands;

/// <summary>Buys a roadside advertisement credit with diamonds.</summary>
public sealed record PurchaseRoadsideAdvertisementCreditCommand : CodecCommand<PurchaseRoadsideAdvertisementCreditCommand>, ICommandCodecProvider<PurchaseRoadsideAdvertisementCreditCommand>
{
    /// <inheritdoc />
    public static CommandCodec<PurchaseRoadsideAdvertisementCreditCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new PurchaseRoadsideAdvertisementCreditCommand()));
}
