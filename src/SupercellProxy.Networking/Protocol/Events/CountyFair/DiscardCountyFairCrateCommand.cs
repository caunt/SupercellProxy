using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.CountyFair;

/// <summary>Discards a county fair crate.</summary>
/// <param name="EventId">The county fair event id.</param>
/// <param name="CrateIndex">The crate slot.</param>
public sealed record DiscardCountyFairCrateCommand(int EventId, int CrateIndex) : CodecCommand<DiscardCountyFairCrateCommand>, ICommandCodecProvider<DiscardCountyFairCrateCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DiscardCountyFairCrateCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.EventId,
            Codecs.VarInt,
            static command => command.CrateIndex,
            static (eventId, crateIndex) => new DiscardCountyFairCrateCommand(eventId, crateIndex)
        ),
        CommandLayout.MetadataFirst
    );
}
