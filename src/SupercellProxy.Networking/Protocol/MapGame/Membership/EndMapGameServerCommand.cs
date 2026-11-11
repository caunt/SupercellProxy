using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Membership;

/// <summary>Ends the avatar's Valley game.</summary>
/// <param name="MapGameId">Id of the Valley board that ended.</param>
/// <param name="ChickenCoins">The ended game's Chicken coins; native stores them in that game's wallet (0x1004881c0) unless negative.</param>
public sealed record EndMapGameServerCommand(LongId? MapGameId, int ChickenCoins) : CodecServerCommand<EndMapGameServerCommand>, ICommandCodecProvider<EndMapGameServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<EndMapGameServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.OptionalLongId,
            static command => command.MapGameId,
            Codecs.VarInt,
            static command => command.ChickenCoins,
            static (mapGameId, chickenCoins) => new EndMapGameServerCommand(mapGameId, chickenCoins)
        )
    );
}
