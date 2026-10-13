using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Sanctuary;

/// <summary>Fills a sanctuary habitat with food.</summary>
/// <param name="HabitatGlobalId">The sanctuary habitat game object id.</param>
public sealed record FeedSanctuaryHabitatCommand(int HabitatGlobalId) : CodecCommand<FeedSanctuaryHabitatCommand>, ICommandCodecProvider<FeedSanctuaryHabitatCommand>
{
    /// <inheritdoc />
    public static CommandCodec<FeedSanctuaryHabitatCommand> Codec { get; } = Codecs.Command(
        type: 218,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.HabitatGlobalId,
            static (habitatGlobalId) => new FeedSanctuaryHabitatCommand(habitatGlobalId)
        )
    );
}
