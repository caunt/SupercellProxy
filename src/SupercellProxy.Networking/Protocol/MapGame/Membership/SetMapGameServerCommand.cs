using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Membership;

/// <summary>Joins the avatar to a valley map game, or leaves the current one.</summary>
/// <param name="MapGameId">The map game id.</param>
public sealed record SetMapGameServerCommand(LongId? MapGameId) : CodecServerCommand<SetMapGameServerCommand>, ICommandCodecProvider<SetMapGameServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SetMapGameServerCommand> Codec { get; } = Codecs.Command(
        type: 272,
        Codecs.Composite(Codecs.OptionalLongId, static command => command.MapGameId, static (mapGameId) => new SetMapGameServerCommand(mapGameId))
    );
}
