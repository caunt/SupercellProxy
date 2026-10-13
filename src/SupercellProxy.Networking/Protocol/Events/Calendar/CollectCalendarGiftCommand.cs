using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Calendar;

/// <summary>Collects a calendar event gift box.</summary>
/// <param name="GiftBoxObjectGlobalId">The gift box object id.</param>
public sealed record CollectCalendarGiftCommand(int GiftBoxObjectGlobalId) : CodecCommand<CollectCalendarGiftCommand>, ICommandCodecProvider<CollectCalendarGiftCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectCalendarGiftCommand> Codec { get; } = Codecs.Command(
        type: 516,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.GiftBoxObjectGlobalId,
            static (giftBoxObjectGlobalId) => new CollectCalendarGiftCommand(giftBoxObjectGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
