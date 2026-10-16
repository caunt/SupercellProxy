using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Tasks;

/// <summary>Buys an extra event task slot with diamonds.</summary>
/// <param name="EventId">Task event whose task list gets the extra slot.</param>
public sealed record BuyEventTaskSlotCommand(int EventId) : CodecCommand<BuyEventTaskSlotCommand>, ICommandCodecProvider<BuyEventTaskSlotCommand>
{
    /// <inheritdoc />
    public static CommandCodec<BuyEventTaskSlotCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.EventId, static eventId => new BuyEventTaskSlotCommand(eventId)),
        CommandLayout.MetadataFirst
    );
}
