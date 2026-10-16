using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.FarmLayouts;

/// <summary>Saves the changes of an edit-mode layout.</summary>
/// <param name="SlotIndex">The edited layout slot index.</param>
public sealed record SaveLayoutChangesCommand(int SlotIndex) : CodecCommand<SaveLayoutChangesCommand>, ICommandCodecProvider<SaveLayoutChangesCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SaveLayoutChangesCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.SlotIndex, static slotIndex => new SaveLayoutChangesCommand(slotIndex))
    );
}
