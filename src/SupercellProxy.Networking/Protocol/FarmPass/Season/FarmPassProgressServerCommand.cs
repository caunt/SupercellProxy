using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Season;

/// <summary>Reports the player's Farm Pass progress as the server has it; the client logic does not act on it.</summary>
/// <param name="SeasonId">Live event id of the season.</param>
/// <param name="Points">Season points the player earned.</param>
/// <param name="Level">Pass level the player reached.</param>
/// <param name="Tier">Pass the player bought; its values are not identified.</param>
public sealed record FarmPassProgressServerCommand(int SeasonId, int Points, int Level, int Tier) : CodecServerCommand<FarmPassProgressServerCommand>, ICommandCodecProvider<FarmPassProgressServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<FarmPassProgressServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.SeasonId,
            Codecs.VarInt,
            static command => command.Points,
            Codecs.VarInt,
            static command => command.Level,
            Codecs.VarInt,
            static command => command.Tier,
            static (seasonId, points, level, tier) => new FarmPassProgressServerCommand(seasonId, points, level, tier)
        )
    );
}
