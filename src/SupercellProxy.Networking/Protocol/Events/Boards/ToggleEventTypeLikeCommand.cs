using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Boards;

/// <summary>Toggles the liked state shared by events of the selected event's type.</summary>
public sealed record ToggleEventTypeLikeCommand(int EventId) : CodecCommand<ToggleEventTypeLikeCommand>, ICommandCodecProvider<ToggleEventTypeLikeCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ToggleEventTypeLikeCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.EventId, static eventId => new ToggleEventTypeLikeCommand(eventId)),
        CommandLayout.MetadataFirst
    );
}
