using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing.Spots;

/// <summary>Restocks a fishing spot with fish fry and advances the fish farmer achievement counter.</summary>
/// <param name="SpotObjectGlobalId">The fishing spot object id.</param>
public sealed record RestockFishingSpotCommand(int SpotObjectGlobalId) : CodecCommand<RestockFishingSpotCommand>, ICommandCodecProvider<RestockFishingSpotCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RestockFishingSpotCommand> Codec { get; } = Codecs.Command(
        type: 113,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.SpotObjectGlobalId,
            static (spotObjectGlobalId) => new RestockFishingSpotCommand(spotObjectGlobalId)
        )
    );
}
