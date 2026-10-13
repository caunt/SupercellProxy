using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Membership;

/// <summary>Ends the valley map game and resets the last position.</summary>
/// <param name="MapGameId">The map game id.</param>
/// <param name="LastPosition">The last position.</param>
public sealed record EndMapGameServerCommand(LongId? MapGameId, int LastPosition) : CodecServerCommand<EndMapGameServerCommand>, ICommandCodecProvider<EndMapGameServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<EndMapGameServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.OptionalLongId,
            static command => command.MapGameId,
            Codecs.VarInt,
            static command => command.LastPosition,
            static (mapGameId, lastPosition) => new EndMapGameServerCommand(mapGameId, lastPosition)
        )
    );
}
