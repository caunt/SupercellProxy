using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.CountyFair;

/// <summary>Records that the county fair screen was opened.</summary>
/// <param name="EventId">County fair event whose screen was opened.</param>
public sealed record MarkCountyFairOpenedCommand(int EventId) : CodecCommand<MarkCountyFairOpenedCommand>, ICommandCodecProvider<MarkCountyFairOpenedCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkCountyFairOpenedCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.EventId, static eventId => new MarkCountyFairOpenedCommand(eventId)),
        CommandLayout.MetadataFirst
    );
}
