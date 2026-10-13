using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing.Lobsters;

/// <summary>Finishes a growing lobster immediately for diamonds.</summary>
/// <param name="LobsterGlobalId">The lobster game object id.</param>
public sealed record SpeedUpLobsterCommand(int LobsterGlobalId) : CodecCommand<SpeedUpLobsterCommand>, ICommandCodecProvider<SpeedUpLobsterCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpLobsterCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.LobsterGlobalId, static (lobsterGlobalId) => new SpeedUpLobsterCommand(lobsterGlobalId))
    );
}
