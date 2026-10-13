using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Calendar;

/// <summary>Opens a calendar event gift box.</summary>
/// <param name="GiftBoxGlobalId">The calendar gift box data id.</param>
public sealed record OpenCalendarGiftBoxCommand(int GiftBoxGlobalId) : CodecCommand<OpenCalendarGiftBoxCommand>, ICommandCodecProvider<OpenCalendarGiftBoxCommand>
{
    /// <inheritdoc />
    public static CommandCodec<OpenCalendarGiftBoxCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.GiftBoxGlobalId,
            static (giftBoxGlobalId) => new OpenCalendarGiftBoxCommand(giftBoxGlobalId)
        ),
        CommandLayout.MetadataFirst
    );
}
