using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.FarmPassGifts;

/// <summary>Claims a Farm Pass gift a neighbor sent.</summary>
/// <param name="GiftId">The gift id.</param>
public sealed record ClaimNeighborhoodFarmPassGiftCommand(LongId GiftId) : CodecCommand<ClaimNeighborhoodFarmPassGiftCommand>, ICommandCodecProvider<ClaimNeighborhoodFarmPassGiftCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimNeighborhoodFarmPassGiftCommand> Codec { get; } = Codecs.Command(
        type: 334,
        Codecs.Composite(Codecs.LongId, static command => command.GiftId, static (giftId) => new ClaimNeighborhoodFarmPassGiftCommand(giftId))
    );
}
