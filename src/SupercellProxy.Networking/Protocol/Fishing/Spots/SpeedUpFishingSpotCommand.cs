using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing.Spots;

/// <summary>Finishes the waiting state of a fishing spot, paying diamonds when requested.</summary>
/// <param name="SpotObjectGlobalId">The fishing spot object id.</param>
/// <param name="UseDiamonds">Whether the speed-up is paid with diamonds.</param>
public sealed record SpeedUpFishingSpotCommand(int SpotObjectGlobalId, bool UseDiamonds) : CodecCommand<SpeedUpFishingSpotCommand>, ICommandCodecProvider<SpeedUpFishingSpotCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpFishingSpotCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.SpotObjectGlobalId,
            Codecs.Boolean,
            static command => command.UseDiamonds,
            static (spotObjectGlobalId, useDiamonds) => new SpeedUpFishingSpotCommand(spotObjectGlobalId, useDiamonds)
        )
    );
}
