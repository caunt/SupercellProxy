using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing.Lobsters;

/// <summary>Makes a lobster the active one of its pool.</summary>
/// <param name="PoolGlobalId">The lobster pool game object id.</param>
/// <param name="LobsterGlobalId">The lobster game object id.</param>
public sealed record SelectLobsterCommand(int PoolGlobalId, int LobsterGlobalId) : CodecCommand<SelectLobsterCommand>, ICommandCodecProvider<SelectLobsterCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SelectLobsterCommand> Codec { get; } = Codecs.Command(
        type: 120,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.PoolGlobalId,
            Codecs.VarInt,
            static command => command.LobsterGlobalId,
            static (poolGlobalId, lobsterGlobalId) => new SelectLobsterCommand(poolGlobalId, lobsterGlobalId)
        )
    );
}
