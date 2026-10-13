using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmPass.Season;

/// <summary>Updates the Farm Pass progress.</summary>
/// <param name="SeasonId">The season id.</param>
/// <param name="Points">The points.</param>
/// <param name="Level">The level.</param>
/// <param name="Tier">The tier.</param>
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
