using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Discards the unsaved changes of an edit-mode layout.</summary>
/// <param name="SlotIndex">The edited layout slot index.</param>
public sealed record DiscardLayoutChangesCommand(int SlotIndex) : CodecCommand<DiscardLayoutChangesCommand>, ICommandCodecProvider<DiscardLayoutChangesCommand>
{
    /// <inheritdoc />
    public static CommandCodec<DiscardLayoutChangesCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.SlotIndex, static slotIndex => new DiscardLayoutChangesCommand(slotIndex))
    );
}
