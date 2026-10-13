using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.CountyFair;

/// <summary>Revives the broken county fair streak.</summary>
/// <param name="EventId">The county fair event id.</param>
public sealed record ReviveCountyFairStreakCommand(int EventId) : CodecCommand<ReviveCountyFairStreakCommand>, ICommandCodecProvider<ReviveCountyFairStreakCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ReviveCountyFairStreakCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.EventId, static (eventId) => new ReviveCountyFairStreakCommand(eventId)),
        CommandLayout.MetadataFirst
    );
}
